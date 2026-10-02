using DG.Tweening;
using Spine.Unity;
using UnityEngine;
using Usaki;

public class BlindBoxItemCtrl : MonoBehaviour
{
    [SerializeField] private SkeletonGraphic _blindBox;
    [SerializeField] private SupermarketItemMukbang _itemSlot;
    [SerializeField] private string _animOnOpen = "animation";
    [SerializeField] private string _defaultAnim = "idle";
    [SerializeField] private float _delayTap;
    [SerializeField] private int _numTapToOpen;
    private int _countTap;

    [Header("Jelly Settings")]
    [SerializeField] private float punchAmount = 0.2f; // Scale multiplier
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private int vibrato = 10;
    [SerializeField] private float elasticity = 0.5f;
    private Tween _jellyTween;

    private Animator _itemSlotAnimator;
    private Animator ItemSlotAnimator => _itemSlotAnimator;
    public bool IsOpen { get; private set; }
    public bool Init(SupermarketItemSO itemSo)
    {
        if (_itemSlot == null || itemSo == null) return false;
        gameObject.SetActive(true);
        _itemSlotAnimator = _itemSlot.GetComponent<Animator>();
        _blindBox.AnimationState.SetAnimation(0, _defaultAnim, true);
        _countTap = 0;
        IsOpen = false;
        return true;
    }

    public void OnTapBox()
    {
        if (_jellyTween != null && _jellyTween.IsActive())
        {
            _jellyTween.Kill();
        }
        // 2. Reset scale to original before starting new tween
        transform.localScale = Vector3.one;
        // We use Vector3.one * punchAmount to scale on all axes
        _jellyTween = transform.DOPunchScale(Vector3.one * punchAmount, duration, vibrato, elasticity).SetEase(Ease.OutQuad); // OutQuad works well with Punch, or leave it default

        _countTap++;
        if (_countTap == _numTapToOpen && !IsOpen)
        {
            OpenBox();
            IsOpen = true;
        }
    }

    private void OpenBox()
    {
        DOVirtual.DelayedCall(0.4f, () =>
        {
            ItemSlotAnimator.Play("open-blind-box");
            _itemSlot.IconParent.gameObject.SetActive(true);
        });
        _blindBox.AnimationState.SetAnimation(0, _animOnOpen, false).Complete += delegate
        {
            _blindBox.gameObject.SetActive(false);
        };

    }

    public void CloseBox()
    {
        IsOpen = false;
        _blindBox.AnimationState.SetAnimation(0, _defaultAnim, true);
    }

    private void OnValidate()
    {
        _blindBox = transform.GetChild(0).GetComponent<SkeletonGraphic>();
        _itemSlot = transform.parent.GetComponent<SupermarketItemMukbang>();

    }
}
