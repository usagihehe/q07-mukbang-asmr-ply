using UnityEngine;
using UnityEngine.UI;
using Usaki;
using Utils.Singletons;

namespace PLY34.UI
{
    /// <summary>UI hub for PLY34 scenes; replaces Usaki.UIManager so its Supermarket/PayPanel code stays out of the build.</summary>
    public class Ply34UIManager : ZMonoSingleton<Ply34UIManager>
    {
        [SerializeField] private Image _lockImage;
        [SerializeField] private HandHint _handHint;

        private Canvas _canvas;

        protected override void Awake()
        {
            base.Awake();
            _canvas = GetComponent<Canvas>();
        }

        public void ActiveLockImage() => _lockImage.gameObject.SetActive(true);

        public void ClickHandAt(Transform target) => _handHint.ShowHint(target, _canvas);

        public void MoveHand(Transform from, Transform to) => _handHint.MoveHint(from, to, _canvas);

        public void StopHand() => _handHint.StopHint();
    }
}
