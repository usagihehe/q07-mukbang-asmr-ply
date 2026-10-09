using DG.Tweening;
using Spine.Unity;
using UnityEngine;
using Usaki;

namespace PLY33.Blindbox
{
    /// <summary>PLY33 shop screen: balls fly into the basket and Done goes straight to the live table (no pay panel).</summary>
    public class BlindboxSupermarketPanel : SupermarketPanel
    {
        private const string IdleAnim = "idle";

        [Header("Blindbox")]
        // Kept outside the machine mask: Luna does not render masked graphics reliably.
        [SerializeField] private SkeletonGraphic _flyBall;
        [SerializeField] private float _flyTime = 0.6f;
        [SerializeField] private Ease _flyEase = Ease.InBack;

        private int _totalPrice;

        protected override void OnEnable()
        {
            base.OnEnable();
            _totalPrice = 0;
            _flyBall.gameObject.SetActive(false);
            UpdateBasketTexts();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _flyBall.transform.DOKill();
        }

        protected override void OnPickItem(object data)
        {
            if (!(data is BlindboxSlotShelf ball)) return;
            SlotBasket basketSlot = _basketCtrl.BasketSlots.Find(slot => !slot.IsOccupied);
            // Basket full: the ball stays in the machine, still pickable.
            if (basketSlot == null) return;

            basketSlot.SetItem(ball.Item);
            _totalPrice += ball.Item.Price;
            FlyBall(ball, basketSlot);
        }

        // Taking a ball back out would fly the food icon and spoil the surprise.
        protected override void OnTakeOut(object data)
        {
        }

        protected override void OnPressDoneBtn()
        {
            UIManager.Instance.ShowSupermarketLivePanel(_basketCtrl.Items);
            gameObject.SetActive(false);
        }

        private void FlyBall(BlindboxSlotShelf ball, SlotBasket basketSlot)
        {
            Transform flyTransform = _flyBall.transform;
            flyTransform.DOKill(true);
            flyTransform.position = ball.GetFirstView().transform.position;
            _flyBall.gameObject.SetActive(true);
            BlindboxSkin.Apply(_flyBall, ball.Item);
            _flyBall.AnimationState.SetAnimation(0, IdleAnim, true);
            ball.gameObject.SetActive(false);

            flyTransform.DOMove(basketSlot.IconWorldPosition, _flyTime)
                .SetEase(_flyEase)
                .SetLink(gameObject)
                .OnComplete(() =>
                {
                    // Clearing CanPick tells the machine the ball has arrived, so it returns to idle.
                    ball.CanPick = false;
                    _flyBall.gameObject.SetActive(false);
                    basketSlot.ShowIcon();
                    _basketCtrl.PlaySpring();
                    UpdateBasketTexts();
                });
        }

        private void UpdateBasketTexts()
        {
            _goodsNumTxt.text = $"{_basketCtrl.PickNum}/{_basketCtrl.BasketSlots.Count}";
            _totalTxt.text = $"{_totalPrice}";
            _doneBtn.interactable = _basketCtrl.PickNum >= _requireAmount;
        }
    }
}
