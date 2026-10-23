using SLunch.Canteen;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Usaki;
namespace SLunch
{
    public class BowlCtrl : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("Topping")]
        [SerializeField] private BowlFood _topping;
        private ScrollRect _toppingScrollRect;
        private bool _isDragging;

        public BowlFood BowlFood
        {
            get
            {
                return _topping;
            }
            set
            {
                _topping = value;
            }
        }

        private FoodController FoodController => CanteenManager.Instance.FoodController;

        private void Awake()
        {
            _topping = GetComponent<BowlFood>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_isDragging)
            {
                _isDragging = true;
                HandleToppingSelection(eventData);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_isDragging)
            {
                _isDragging = false;
                FoodController.StopDragFood();
                _topping.ShowFull();
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
        }

        public void OnDrag(PointerEventData eventData)
        {
            FoodController.OnDragFood(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        }

        private void HandleToppingSelection(PointerEventData eventData)
        {
            FoodController.StartDragFood(_topping, eventData);
            _topping.ShowUsed();
        }
    }

}
