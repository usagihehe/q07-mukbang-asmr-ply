using DG.Tweening;
using Spine.Unity;
using UnityEngine;

namespace PLY33.Blindbox
{
    /// <summary>Ball on the live table: opens after a number of taps and reveals the slot's food.</summary>
    public class BlindBoxItemCtrl : MonoBehaviour
    {
        [SerializeField]
        private GameObject _lightEffect;

        [SerializeField]
        private SkeletonGraphic _blindBox;

        [SerializeField]
        private SupermarketItemMukbang _itemSlot;

        [SerializeField]
        private SpringCtrl _spring;

        [SerializeField]
        private string _animOnOpen = "animation";

        [SerializeField]
        private string _defaultAnim = "idle";

        [SerializeField]
        private float _delayTap = 0.1f;

        [SerializeField]
        private int _numTapToOpen = 3;

        [SerializeField]
        private float _revealDelay = 1f;

        private bool _waitOpen;

        private int _countTap;

        private Animator _itemSlotAnimator;

        // Optional: PLY33 table slots have no Animator, the food then just appears when the ball opens.
        private Animator ItemSlotAnimator =>
            _itemSlotAnimator != null ? _itemSlotAnimator : _itemSlotAnimator = _itemSlot.GetComponent<Animator>();

        public bool IsOpen { get; private set; }

        public bool Init(SupermarketItemSO itemSo)
        {
            if (itemSo == null) return false;
            _countTap = 0;
            _waitOpen = false;
            // CloseBox first: it activates the ball, and an inactive SkeletonGraphic has no skeleton to skin.
            CloseBox();
            // SupermarketLivePanel.OnValidate calls Init in Edit Mode, where the Spine state is not built.
            if (Application.isPlaying) BlindboxSkin.Apply(_blindBox, itemSo);
            return true;
        }

        public void OnTapBox()
        {
            if (IsOpen || _waitOpen) return;
            _waitOpen = true;
            _spring.PlaySpring();
            _countTap++;
            if (_countTap >= _numTapToOpen)
            {
                OpenBox();
                return;
            }
            DOVirtual.DelayedCall(_delayTap, () => _waitOpen = false).SetLink(gameObject);
        }

        private void OpenBox()
        {
            if (_lightEffect != null) _lightEffect.SetActive(true);
            // The food pops in while the ball is still opening; waiting for the whole animation feels sluggish.
            DOVirtual.DelayedCall(_revealDelay, () =>
            {
                _itemSlot.RevealIcon();
                if (ItemSlotAnimator != null) ItemSlotAnimator.Play("open-blind-box");
            }).SetLink(gameObject);
            _blindBox.AnimationState.SetAnimation(0, _animOnOpen, false).Complete += _ =>
            {
                _blindBox.gameObject.SetActive(false);
                if (_lightEffect != null) _lightEffect.SetActive(false);
                _waitOpen = false;
                IsOpen = true;
                _itemSlot.NotifyBlindBoxOpened();
            };
        }

        public void CloseBox()
        {
            IsOpen = false;
            _blindBox.gameObject.SetActive(true);
            if (_lightEffect != null) _lightEffect.SetActive(false);
            _itemSlot.IconParent.gameObject.SetActive(false);
            if (Application.isPlaying) _blindBox.AnimationState.SetAnimation(0, _defaultAnim, true);
        }
    }
}
