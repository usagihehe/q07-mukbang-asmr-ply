using Spine.Unity;
using UnityEngine;
using Usaki;

namespace PLY33.Blindbox
{
    /// <summary>The ball dispensed by the machine; it goes to the basket on its own once it lands.</summary>
    public class BlindboxSlotShelf : SlotShelfCtrl
    {
        [SerializeField]
        private SkeletonGraphic _blindboxModel;

        [SerializeField]
        private string[] _blindboxSkins;

        public override bool CanPick
        {
            get => _canPick;
            set => _canPick = value;
        }

        // Replaces the base Awake: the ball has no item until the machine drops one, and is never tapped (see Pick).
        protected override void Awake()
        {
        }

        /// <summary>Sends the landed ball to the basket; CanPick stays set until the basket clears it on arrival.</summary>
        public void Pick()
        {
            if (!_canPick) return;
            Observer.Instance.NotifyWithData(ObserverTopic.OnPickItem, this);
        }

        public override bool Init(SupermarketItemSO itemSo)
        {
            if (itemSo == null) return false;
            _itemSO = itemSo;
            _canPick = false;
            BlindboxSkin.Apply(_blindboxModel, itemSo);
            return true;
        }

        public override GameObject GetFirstView()
        {
            return _blindboxModel.gameObject;
        }
    }
}
