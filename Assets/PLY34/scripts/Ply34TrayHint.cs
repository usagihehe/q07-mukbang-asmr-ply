using PLY34.UI;
using SLunch;
using SLunch.Tray;
using UnityEngine;
using UnityEngine.UI;

namespace PLY34.Hint
{
    /// <summary>Tray tutorial: drag the first food into the tray, then drag a food that mixes with it, until the first mix.</summary>
    public class Ply34TrayHint : MonoBehaviour
    {
        [SerializeField] private BowlFood _firstFood;
        [SerializeField] private ScrollRect _scrollRect;

        private bool _isMixDone;
        private Transform _hintFrom;
        private Transform _hintTo;
        private bool _isPausedByScroll;
        private float _resumeTime;

        private const float RESUME_DELAY = 0.3f;

        private bool IsHinting => _hintFrom != null;

        private void OnEnable()
        {
            Observer.Instance.AddObserver(ObserverTopic.OnCanteenFoodAdded, OnFoodAdded);
            Observer.Instance.AddObserver(ObserverTopic.OnCanteenFoodMixed, OnFoodMixed);
            _scrollRect.onValueChanged.AddListener(OnScroll);
            if (!_isMixDone) ShowDragHint(_firstFood.transform, FindFirstEmptySlot());
        }

        private void OnDisable()
        {
            Observer.Instance.RemoveObserver(ObserverTopic.OnCanteenFoodAdded, OnFoodAdded);
            Observer.Instance.RemoveObserver(ObserverTopic.OnCanteenFoodMixed, OnFoodMixed);
            _scrollRect.onValueChanged.RemoveListener(OnScroll);
            Hide();
        }

        private void OnFoodAdded(object data)
        {
            if (_isMixDone) return;
            BaseSlot slot = (BaseSlot)data;
            BowlFood mixFood = FindMixFood(slot);
            if (mixFood == null) Hide();
            else ShowDragHint(mixFood.transform, slot.transform);
        }

        private void OnFoodMixed(object data)
        {
            _isMixDone = true;
            Hide();
        }

        // The hand position is captured once per tween, so it is hidden while the bowls scroll and re-aimed once they stop.
        private void OnScroll(Vector2 scrollPosition)
        {
            if (!IsHinting) return;
            _resumeTime = Time.time + RESUME_DELAY;
            if (_isPausedByScroll) return;
            _isPausedByScroll = true;
            Ply34UIManager.Instance.StopHand();
        }

        private void Update()
        {
            if (!_isPausedByScroll || Time.time < _resumeTime) return;
            _isPausedByScroll = false;
            if (IsHinting) Ply34UIManager.Instance.MoveHand(_hintFrom, _hintTo);
        }

        private void ShowDragHint(Transform from, Transform to)
        {
            if (to == null) { Hide(); return; }
            _hintFrom = from;
            _hintTo = to;
            _isPausedByScroll = false;
            Ply34UIManager.Instance.MoveHand(from, to);
        }

        private void Hide()
        {
            if (!IsHinting) return;
            _hintFrom = null;
            _hintTo = null;
            Ply34UIManager.Instance.StopHand();
        }

        private static Transform FindFirstEmptySlot()
        {
            foreach (BaseSlot slot in CanteenManager.Instance.TrayController.BaseSlots)
            {
                if (slot.IsEmpty()) return slot.transform;
            }
            return null;
        }

        private static BowlFood FindMixFood(BaseSlot slot)
        {
            foreach (BowlFood bowl in CanteenManager.Instance.BowlFoods)
            {
                if (bowl != null && slot.CanMixFood(bowl.GetData().Id)) return bowl;
            }
            return null;
        }
    }
}
