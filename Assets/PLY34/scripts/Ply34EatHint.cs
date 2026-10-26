using System.Collections.Generic;
using PLY34.UI;
using SLunch.Livestream;
using UnityEngine;

namespace PLY34.Hint
{
    /// <summary>Eat tutorial: taps the first big tray slot, drags it to the mouth while held, until the first bite.</summary>
    public class Ply34EatHint : MonoBehaviour
    {
        [SerializeField] private List<BigSlotMukbang> _bigSlots;
        [SerializeField] private CharacterCtrl _character;

        private BigSlotMukbang _target;

        private void OnEnable()
        {
            Observer.Instance.AddObserver(ObserverTopic.OnCanteenStartLive, OnStartLive);
            Observer.Instance.AddObserver(ObserverTopic.OnSelectItem, OnSelectItem);
            Observer.Instance.AddObserver(ObserverTopic.OnDropItem, OnDropItem);
            Observer.Instance.AddObserver(ObserverTopic.OnCanteenConsumeItem, OnConsumeItem);
            Observer.Instance.AddObserver(ObserverTopic.OnEndLiveGamePlay, OnConsumeItem);
        }

        private void OnDisable()
        {
            Observer.Instance.RemoveObserver(ObserverTopic.OnCanteenStartLive, OnStartLive);
            Observer.Instance.RemoveObserver(ObserverTopic.OnSelectItem, OnSelectItem);
            Observer.Instance.RemoveObserver(ObserverTopic.OnDropItem, OnDropItem);
            Observer.Instance.RemoveObserver(ObserverTopic.OnCanteenConsumeItem, OnConsumeItem);
            Observer.Instance.RemoveObserver(ObserverTopic.OnEndLiveGamePlay, OnConsumeItem);
        }

        private void OnStartLive(object data)
        {
            _target = _bigSlots.Find(slot => slot.gameObject.activeSelf);
            if (_target != null) Ply34UIManager.Instance.ClickHandAt(_target.transform);
        }

        private void OnSelectItem(object data)
        {
            if (_target != null && ReferenceEquals(data, _target))
                Ply34UIManager.Instance.MoveHand(_target.transform, _character.mPoint);
        }

        // Consume() fires onConsume before its own OnDropItem, so the hint is already off by then.
        private void OnDropItem(object data)
        {
            if (_target != null && ReferenceEquals(data, _target))
                Ply34UIManager.Instance.ClickHandAt(_target.transform);
        }

        private void OnConsumeItem(object data)
        {
            if (_target == null) return;
            _target = null;
            Ply34UIManager.Instance.StopHand();
        }
    }
}
