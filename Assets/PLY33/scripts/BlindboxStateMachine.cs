using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;
using Usaki;

namespace PLY33.Blindbox
{
    /// <summary>Gacha machine sequence: idle → act → gacha → open (ball waits to be picked) → idle.</summary>
    public class BlindboxStateMachine : BaseStateMachine
    {
        // State names double as the machine's Spine animation names.
        public readonly string Idle = "idle";

        public readonly string Act = "act";

        public readonly string Gacha = "gacha";

        public readonly string Open = "open";

        [SerializeField]
        private SupermarketItemListSO _gachaItemListSo;

        [SerializeField]
        private BasketCtrl _basket;

        public Button CatcherBtn;

        public SkeletonGraphic Model;

        public BlindboxSlotShelf Blindbox;

        [Tooltip("Ball anchored Y it drops from; keep it hidden behind the machine mask.")]
        public float StartPosY;

        // Drawing from a shuffled bag instead of a fresh random pick keeps the basket free of duplicate foods.
        private readonly List<SupermarketItemSO> _drawBag = new List<SupermarketItemSO>();

        private int _drawCount;

        private bool _isCatcherHintShown;

        public SupermarketItemListSO GachaItemList => _gachaItemListSo;

        public bool IsBasketFull => _basket.IsFull;

        protected override string InitialState => Idle;

        protected override void Awake()
        {
            base.Awake();
            CatcherBtn.onClick.AddListener(OnPressCatcherBtn);
            BlindboxSkin.Clear();
            ResetBlindboxPos();
        }

        private void Start()
        {
            _isCatcherHintShown = true;
            UIManager.Instance.ClickHandAt(CatcherBtn.transform);
        }

        /// <summary>Next food of the shuffled bag; also fixes its ball skin by draw order.</summary>
        public SupermarketItemSO DrawNextItem()
        {
            if (_drawBag.Count == 0) RefillDrawBag();
            int lastIndex = _drawBag.Count - 1;
            SupermarketItemSO item = _drawBag[lastIndex];
            _drawBag.RemoveAt(lastIndex);
            BlindboxSkin.Assign(item, _drawCount);
            _drawCount++;
            return item;
        }

        private void RefillDrawBag()
        {
            _drawBag.AddRange(_gachaItemListSo.Items);
            for (int i = _drawBag.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (_drawBag[i], _drawBag[j]) = (_drawBag[j], _drawBag[i]);
            }
        }

        /// <summary>Snaps the ball to its landing spot and hides it until the next drop.</summary>
        public void ResetBlindboxPos()
        {
            Blindbox.transform.DOKill(true);
            Blindbox.CanPick = false;
            Blindbox.gameObject.SetActive(false);
        }

        private void OnPressCatcherBtn()
        {
            AudioManager.Instance.PlayAudioClick();
            HideCatcherHint();
            if (IsBasketFull)
            {
                UIManager.Instance.Notify.Show("Basket is full");
                return;
            }
            CatcherBtn.interactable = false;
        }

        private void HideCatcherHint()
        {
            if (!_isCatcherHintShown) return;
            _isCatcherHintShown = false;
            UIManager.Instance.StopHand();
        }
    }
}
