using UnityEngine;
using UnityEngine.UI;
using Usaki;

namespace PLY34.Hint
{
    /// <summary>Shelf tutorial: taps the front item view (v1) of the target shelf slot until the first item is picked.</summary>
    public class ShelfTapHint : MonoBehaviour
    {
        [SerializeField] private SlotShelfCtrl _target;
        [SerializeField] private ScrollRect _scrollRect;

        private bool _isPickDone;
        private bool _isWaitingToShow;
        private float _showTime;

        private const float RESUME_DELAY = 0.3f;

        private void OnEnable()
        {
            Observer.Instance.AddObserver(ObserverTopic.OnPickItem, OnPickItem);
            if (_scrollRect != null) _scrollRect.onValueChanged.AddListener(OnScroll);
            // Delayed so the shelf layout has settled before the hand captures its position.
            if (!_isPickDone) ScheduleShow(RESUME_DELAY);
        }

        private void OnDisable()
        {
            Observer.Instance.RemoveObserver(ObserverTopic.OnPickItem, OnPickItem);
            if (_scrollRect != null) _scrollRect.onValueChanged.RemoveListener(OnScroll);
            _isWaitingToShow = false;
            if (!_isPickDone) UIManager.Instance.StopHand();
        }

        private void OnPickItem(object data)
        {
            if (_isPickDone) return;
            _isPickDone = true;
            _isWaitingToShow = false;
            UIManager.Instance.StopHand();
        }

        // The hand position is captured once per call, so it is hidden while the shelf scrolls and re-aimed once it stops.
        private void OnScroll(Vector2 scrollPosition)
        {
            if (_isPickDone) return;
            if (!_isWaitingToShow) UIManager.Instance.StopHand();
            ScheduleShow(RESUME_DELAY);
        }

        private void ScheduleShow(float delay)
        {
            _isWaitingToShow = true;
            _showTime = Time.time + delay;
        }

        private void Update()
        {
            if (!_isWaitingToShow || Time.time < _showTime) return;
            _isWaitingToShow = false;
            UIManager.Instance.ClickHandAt(_target.GetFirstView().transform);
        }
    }
}
