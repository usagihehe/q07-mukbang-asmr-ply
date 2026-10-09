using System;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using Spine.Unity;
using UnityEngine;
using UnityEngine.EventSystems;
using Usaki;

public class SupermarketItemMukbang : ItemMukbang
{
    [SerializeField] private float _delayHideTool;
    [SerializeField] private float _timeOpen;
    [SerializeField] private PLY33.Blindbox.BlindBoxItemCtrl _blindBox;
    [SerializeField] protected Transform _iconParent;
    [SerializeField] private float _revealTime = 0.35f;
    [SerializeField] private Ease _revealEase = Ease.OutBack;
    protected bool _isOpen;
    private bool _isBlindBox;
    protected ToolCase _toolCase;
    private int _countOpen;
    public Action<SupermarketItemMukbang> onBlindBoxOpened;
    public SupermarketItemSO Item { get; protected set; }
    public ToolCase ToolCase => _toolCase;
    public Transform IconParent => _iconParent;
    public bool IsBlindBoxClosed => _isBlindBox && !_blindBox.IsOpen;

    public virtual bool Init(SupermarketItemSO item)
    {
        if (item == null) return false;
        Item = item;
        gameObject.SetActive(true);
        //Blindbox item
        _isBlindBox = _blindBox != null && SupermarketItemData.Instance.IsBlindBoxItem(Item);
        if (_blindBox != null) _blindBox.gameObject.SetActive(_isBlindBox);
        if (_isBlindBox) _blindBox.Init(Item);
        _iconParent.DOKill();
        _iconParent.gameObject.SetActive(!_isBlindBox);
        _iconParent.localScale = Vector3.one * Item.OffsetScaleOnTable;

        //Parent item
        _curStep = Item.Steps.Count - 1;
        _isDone = false;
        //Current item
        _isOpen = !Item.HasBox();
        _toolCase = Item.Tool == null ? ToolCase.Nothing : ToolCase.Spoon;
        _countOpen = 0;
        //UI Init

        _icon.gameObject.SetActive(true);
        _icon.sprite = Item.GetIcon();
        _icon.SetNativeSize();

        _tool.sprite = Item.Tool != null ? Item.Tool : Item.GetIcon();
        _tool.SetNativeSize();

        Vector2 spritePivot = _tool.sprite.pivot;
        Vector2 texSize = _tool.rectTransform.rect.size;
        _tool.rectTransform.pivot = new Vector2(
            spritePivot.x / texSize.x,
            spritePivot.y / texSize.y
        );

        return true;
    }

    /// <summary>Pops the food in from zero scale once its blind box has opened.</summary>
    public void RevealIcon()
    {
        _iconParent.gameObject.SetActive(true);
        _iconParent.localScale = Vector3.zero;
        _iconParent.DOScale(Item.OffsetScaleOnTable, _revealTime).SetEase(_revealEase).SetLink(gameObject);
    }

    public void NotifyBlindBoxOpened() => onBlindBoxOpened?.Invoke(this);

    public override EmotionType GetEmotion()
    {
        return Item.Emotion;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!CanPick()) return;
        if (IsBlindBoxClosed)
        {
            _blindBox.OnTapBox();
            return;
        }
        if (_isDone) return;
        if (!_isOpen) { OpenItem(); return; }
        base.OnPointerDown(eventData);
        Observer.Instance.NotifyWithData(ObserverTopic.OnSelectItem, this);
        HandleIconAt(_toolCase == ToolCase.Nothing ? _curStep : _curStep - 1, _toolCase != ToolCase.Nothing);
        HandleTool(false);
        ShowTool();
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (IsBlindBoxClosed) return;
        base.OnDrag(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (_isDone || IsBlindBoxClosed) return;
        base.OnPointerUp(eventData);
        Observer.Instance.NotifyWithData(ObserverTopic.OnDropItem, this);
        HandleIconAt(_curStep);
        HandleTool(false);
        HideTool();
    }

    public override void Consume()
    {
        _curStep--;
        onConsume?.Invoke(this);
        if (_curStep == 0)
        {
            if (Item.HasLastFrame())
            {
                HandleIconAt(0, Item.HasLastFrame());
                _isDone = true;
                onDone?.Invoke(this);
            }
        }
        else if (_curStep == -1)
        {
            if (!Item.HasLastFrame())
            {
                HandleIconAt(0, Item.HasLastFrame());
                _isDone = true;
                onDone?.Invoke(this);
                HideTool();
            }
        }
        HandleTool(true);
        if (_toolCase == ToolCase.Spoon)
        {
            HideTool();
        }
    }

    protected virtual void OpenItem()
    {
        _isOpen = true;
        _curStep--;
        HandleIconAt(_curStep);
        AudioManager.Instance.PlaySoundEffect(null);
    }

    protected void HandleIconAt(int index, bool active = true)
    {
        if (index >= 0 && index < Item.Steps.Count)
        {
            _icon.sprite = Item.Steps[index];
            _icon.SetNativeSize();
        }
        _icon.gameObject.SetActive(active);
    }

    private void HandleTool(bool eaten)
    {
        if (_isDone) { HideTool(); return; }
        _tool.sprite = _toolCase == ToolCase.Spoon ? Item.Tool : Item.Steps[_curStep];
        _tool.SetNativeSize();
    }

    public override void HideTool()
    {
        Observer.Instance.NotifyWithData(ObserverTopic.OnDropItem, this);
        base.HideTool();
    }
}

