using SLunch.Tray;
using Spine;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Usaki;

namespace SLunch.Canteen
{
    public class FoodController : MonoBehaviour, IFoodController
    {
        [SerializeField] private bool _isDragging;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _dragFoodImage;
        [SerializeField] private AudioClip _mixedFoodSound;

        private IBowlFood _currentFood;
        private BaseSlot _currentSlot;
        private TrayController _trayController;
        private Action _onCompleted;
        private const int FOOD_NUMBER_DONE = 3;
        public RectTransform RectTransform => _rectTransform;
        public RectTransform RectDragFood => _dragFoodImage.rectTransform;

        private void Awake()
        {
            _trayController = FindObjectOfType<TrayController>();
        }

        private void OnEnable()
        {
            _isDragging = false;
        }

        private void OnDisable()
        {

        }

        public void StartDragFood(IBowlFood food, PointerEventData eventData)
        {
            _isDragging = true;
            _currentFood = food;
            _dragFoodImage.gameObject.SetActive(true);
            _dragFoodImage.sprite = _currentFood.GetData().PieceSprite;
            _dragFoodImage.SetNativeSize();
            _dragFoodImage.transform.localScale = _currentFood.GetLocalScale();
            HandleDrag(eventData);
            _trayController.ScaleUp();
            AudioManager.Instance.PlaySoundEffect(_currentFood.GetData().soundPick);
        }

        private void HandleDrag(PointerEventData eventData)
        {
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(RectTransform, eventData.position, eventData.pressEventCamera, out localPoint))
            {
                RectDragFood.anchoredPosition = localPoint + Vector2.up * 200f;
            }
        }

        public void OnDragFood(PointerEventData eventData)
        {
            Vector2 localPoint = Vector2.zero;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(RectTransform, eventData.position, eventData.pressEventCamera, out localPoint))
            {
                RectDragFood.anchoredPosition = localPoint + Vector2.up * 200f;
            }
        }

        public void StopDragFood()
        {
            _isDragging = false;
            _dragFoodImage.gameObject.SetActive(false);
            CheckDropFood();
            _trayController.ScaleDown();
        }

        public void Register(Action onCompleted)
        {
            _onCompleted = onCompleted;
        }

        public void Unregister()
        {
            _onCompleted = null;
        }

        private void Update()
        {
            if (!_isDragging) return;
            BaseSlot slot = _trayController.GetNearestSlot(RectDragFood, 180);

            if (slot == null || (slot.IsFull() && !slot.CanMixFood(_currentFood.GetData().Id)))
            {
                if (_currentSlot != null)
                {
                    _currentSlot.SetActiveNormalHighlight(false);
                    _currentSlot.SetActiveMixedHighlight(false);
                }
                _currentSlot = null;
                return;
            }

            if (_currentSlot != slot)
            {
                _currentSlot?.SetActiveNormalHighlight(false);
                _currentSlot?.SetActiveMixedHighlight(false);
                _currentSlot = slot;

                if (slot.CurrentResultFoodID == -1 || _currentFood.GetData().Id == slot.CurrentResultFoodID)
                {
                    _currentSlot.SetActiveNormalHighlight(true);
                }
                else if (slot.CanMixFood(_currentFood.GetData().Id))
                {
                    _currentSlot.SetActiveMixedHighlight(true);
                }
            }
        }

        private void CheckDropFood()
        {
            if (_currentSlot != null)
            {
                BaseSlot slot = _currentSlot;
                if (slot.TryAddFood(_currentFood.GetData().Id))
                {
                    AudioManager.Instance.PlaySoundEffect(_currentFood.GetData().soundDrop);
                    _trayController.ActiveEffectFull(slot.ID);
                    Observer.Instance.NotifyWithData(ObserverTopic.OnCanteenFoodAdded, slot);
                }
                else if (slot.TryMixFood(_currentFood.GetData().Id))
                {
                    AudioManager.Instance.PlaySoundEffect(_mixedFoodSound);
                    _trayController.ActiveEffectBling(slot.ID);
                    Observer.Instance.NotifyWithData(ObserverTopic.OnCanteenFoodMixed, slot);
                }
                slot.SetActiveNormalHighlight(false);
                CheckFoodInTray();
            }
        }

        private void CheckCompleted()
        {
            _onCompleted?.Invoke();
        }

        private void CheckFoodInTray()
        {
            int doneCount = 0;
            foreach (BaseSlot slot in _trayController.BaseSlots)
            {
                if (!slot.IsEmpty()) doneCount++;
            }
            if (doneCount >= FOOD_NUMBER_DONE)
            {
                CanteenManager.Instance.CanteenGameplay.ShowButtonDone();
            }
        }
    }
}
