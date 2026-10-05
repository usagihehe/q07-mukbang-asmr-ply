using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;
using Usaki;

namespace PLY33.Blindbox
{
    /// <summary>The ball dispensed by the machine; tapping it sends it to the basket.</summary>
    [RequireComponent(typeof(Button))]
    public class BlindboxSlotShelf : SlotShelfCtrl
    {
        private const string IdleAnim = "idle";

        [SerializeField] private SkeletonGraphic _blindboxModel;

        public bool IsPicked { get; private set; }
        public RectTransform BallTransform => (RectTransform)transform;
        public SkeletonGraphic Model => _blindboxModel;

        // Replaces the base Awake: the ball has no item until the machine drops one, and must ignore taps until then.
        protected override void Awake()
        {
            _canPick = false;
            GetComponent<Button>().onClick.AddListener(OnTapBall);
        }

        public override bool Init(SupermarketItemSO itemSo)
        {
            if (itemSo == null) return false;
            _itemSO = itemSo;
            IsPicked = false;
            _canPick = false;
            BlindboxSkin.Apply(_blindboxModel, itemSo);
            _blindboxModel.AnimationState.SetAnimation(0, IdleAnim, true);
            return true;
        }

        public override GameObject GetFirstView()
        {
            return _blindboxModel.gameObject;
        }

        private void OnTapBall()
        {
            if (!_canPick) return;
            _canPick = false;
            IsPicked = true;
            AudioManager.Instance.PlayAudioClick();
            Observer.Instance.NotifyWithData(ObserverTopic.OnPickItem, this);
        }
    }
}
