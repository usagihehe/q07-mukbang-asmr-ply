using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Usaki;

public class BasketCtrl : MonoBehaviour
{
    [Header("Move")]
    [SerializeField]
    private ScrollRect _scrollRect;
    [SerializeField]
    private float _convertPos;
    [SerializeField]
    private float _minX;
    [SerializeField]
    private float _maxX;
    [SerializeField]
    private RectTransform _rectTransform;
    [SerializeField]
    private Button _button;
    [SerializeField]
    private List<SlotBasket> _basketSlots;
    [Header("Spring")]
    [SerializeField]
    private float _springFrequency = 10f;
    // Capped below 1: SpringUtils divides by alpha, which hits 0 at critical damping.
    [SerializeField, Range(0f, 0.99f)]
    private float _springDampingRatio = 0.5f;
    [SerializeField]
    private float _springSquashAmount = 0.15f;
    [SerializeField]
    private float _springKickVelocity = -2f;
    [Header("Fly icon")]
    // Kept outside the basket mask: Luna does not render masked graphics reliably.
    [SerializeField]
    private List<Image> _flyIcons;
    [SerializeField]
    private float _flyIconScale = 0.75f;
    [SerializeField]
    private float _timeMove = 0.75f;
    [SerializeField]
    private Ease _moveEase = Ease.InBack;
    [SerializeField]
    private float _timeFly = 0.75f;
    [SerializeField]
    private Vector3 _flyHigh = new Vector3(0f, 250f, 0f);
    [SerializeField]
    private Ease _fadeEase = Ease.InExpo;
    private SpringUtils.tDampedSpringMotionParams _springParams;
    private float _defaultScale;
    private float _currentScale;
    private float _scaleVelocity;
    private bool _canTakeOut;
    public List<SlotBasket> BasketSlots => _basketSlots;
    public List<SupermarketItemSO> Items => _basketSlots.FindAll(slot => slot.Item != null).ConvertAll(slot => slot.Item);
    public bool IsFull => PickNum == _basketSlots.Count;
    public int PickNum => _basketSlots.FindAll(slot => slot.Item != null).Count;

    private void Awake()
    {
        _springParams = new SpringUtils.tDampedSpringMotionParams();
        _defaultScale = _rectTransform.localScale.x;
        _currentScale = _defaultScale;

        _button.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayAudioClick();
            PlaySpring();
            Observer.Instance.NotifyWithData(ObserverTopic.OnTakeOut, this);
        });
    }

    private void OnEnable()
    {
        _canTakeOut = false;
        _currentScale = _defaultScale;
        _scaleVelocity = 0f;
        ClearBasket();
    }

    private void OnDisable()
    {
        ClearBasket();
    }

    private void Update()
    {
        SpringUtils.CalcDampedSpringMotionParams(ref _springParams, Time.deltaTime, _springFrequency * Mathf.PI, _springDampingRatio);
        SpringUtils.UpdateDampedSpringMotion(ref _currentScale, ref _scaleVelocity, _defaultScale, _springParams);
        _rectTransform.localScale = Vector3.one * _currentScale;
    }

    public void PlaySpring()
    {
        _currentScale -= _springSquashAmount;
        _scaleVelocity = _springKickVelocity;
    }

    public void ClearBasket()
    {
        foreach (var basket in _basketSlots)
        {
            basket.ClearSlot();
        }
        foreach (var flyIcon in _flyIcons)
        {
            flyIcon.transform.DOKill();
            flyIcon.DOKill();
            flyIcon.gameObject.SetActive(false);
        }
    }

    public bool PutInItem(SlotShelfCtrl fromSlot, Action onPutDone)
    {
        SlotBasket slot = GetSlot();
        if (slot == null)
            return false;
        else
        {
            _canTakeOut = false;
            GameObject shelfView = fromSlot.GetFirstView();
            shelfView.SetActive(false);
            slot.SetItem(fromSlot.Item);
            Image flyIcon = ShowFlyIcon(fromSlot.Item, shelfView.transform.position);
            flyIcon.transform.DOMove(slot.IconWorldPosition, _timeMove).SetEase(_moveEase)
                .OnComplete(() =>
                {
                    flyIcon.gameObject.SetActive(false);
                    slot.ShowIcon();
                    shelfView.SetActive(true);
                    PlaySpring();
                    onPutDone?.Invoke();
                    _canTakeOut = PickNum > 0;
                });
            return true;
        }
    }

    public bool PutOutItem()
    {
        if (_canTakeOut)
        {
            SlotBasket slot = _basketSlots[PickNum - 1];
            if (slot == null)
                return false;
            else
            {
                Image flyIcon = ShowFlyIcon(slot.Item, slot.IconWorldPosition);
                slot.ClearSlot();
                flyIcon.transform.DOMove(flyIcon.transform.position + _flyHigh, _timeFly)
                    .OnComplete(() => flyIcon.gameObject.SetActive(false));
                flyIcon.DOFade(0f, _timeFly).SetEase(_fadeEase);
                _canTakeOut = PickNum > 0;
            }
            return true;
        }
        return false;

    }

    private Image ShowFlyIcon(SupermarketItemSO item, Vector3 worldPosition)
    {
        Image flyIcon = _flyIcons.Find(icon => !icon.gameObject.activeSelf);
        // All busy: reuse the oldest and complete its tween so the slot it carries still gets its icon.
        if (flyIcon == null) flyIcon = _flyIcons[0];
        flyIcon.transform.DOKill(true);
        flyIcon.DOKill();

        flyIcon.sprite = item.GetIcon();
        flyIcon.SetNativeSize();
        flyIcon.transform.localScale = Vector3.one * item.OffsetScaleOnTable * _flyIconScale;
        flyIcon.transform.position = worldPosition;
        Color iconColor = flyIcon.color;
        iconColor.a = 1f;
        flyIcon.color = iconColor;
        flyIcon.gameObject.SetActive(true);
        return flyIcon;
    }

    private SlotBasket GetSlot()
    {
        foreach (var slot in BasketSlots)
        {
            if (slot.Item == null)
            {
                return slot;
            }
        }
        return null;
    }

}
