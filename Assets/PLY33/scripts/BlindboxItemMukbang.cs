using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PLY33.Blindbox
{
    /// <summary>Table slot whose food stays inside a ball until the player taps it open.</summary>
    public class BlindboxItemMukbang : SupermarketItemMukbang
    {
        [SerializeField] private BlindBoxItemCtrl _ball;
        [SerializeField] private float _revealTime = 0.35f;
        [SerializeField] private Ease _revealEase = Ease.OutBack;

        public override bool Init(SupermarketItemSO item)
        {
            if (!base.Init(item)) return false;
            _iconParent.DOKill();
            _iconParent.gameObject.SetActive(false);
            _ball.Init(item);
            return true;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (!_ball.IsOpen)
            {
                if (CanPick()) _ball.OnTapBox(RevealFood);
                return;
            }
            base.OnPointerDown(eventData);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (!_ball.IsOpen) return;
            base.OnDrag(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if (!_ball.IsOpen) return;
            base.OnPointerUp(eventData);
        }

        private void RevealFood()
        {
            _iconParent.gameObject.SetActive(true);
            _iconParent.localScale = Vector3.zero;
            _iconParent.DOScale(Item.OffsetScaleOnTable, _revealTime).SetEase(_revealEase).SetLink(gameObject);
        }
    }
}
