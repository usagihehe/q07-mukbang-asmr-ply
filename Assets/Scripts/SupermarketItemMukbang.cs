using DG.Tweening.Core.Easing;
using Spine.Unity;
using UnityEngine;
using UnityEngine.EventSystems;
using Usaki;

public class SupermarketItemMukbang : ItemMukbang
{
    [SerializeField] private float _delayHideTool;
    [SerializeField] private float _timeOpen;
    [SerializeField] private BlindBoxItemCtrl _blindBox;
    [SerializeField] protected Transform _iconParent;
    protected bool _isOpen;
    private bool _isOpenBlindBox;
    protected ToolCase _toolCase;
    private int _countOpen;
    public SupermarketItemSO Item { get; protected set; }
    public ToolCase ToolCase => _toolCase;
    public Transform IconParent => _iconParent;

    public virtual bool Init(SupermarketItemSO item)
    {
        if (item == null) return false;
        Item = item;
        gameObject.SetActive(true);
        //Blindbox item
        bool isBlindBox = false;
        if (_blindBox != null && isBlindBox)
        {
            _blindBox.Init(Item);
            _blindBox.CloseBox();
        }
        _isOpenBlindBox = !isBlindBox;
        _iconParent.gameObject.SetActive(!isBlindBox);
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

    public override EmotionType GetEmotion()
    {
        return Item.Emotion;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!CanPick()) return;
        if (!_isOpenBlindBox)
        {

            _blindBox.OnTapBox();
            _isOpenBlindBox = _blindBox.IsOpen;
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
        base.OnDrag(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (_isDone) return;
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

