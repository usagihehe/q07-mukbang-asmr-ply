using DG.Tweening;
using SLunch.Tray;
using System;
using UnityEngine;

namespace SLunch.Water
{
    public class WaterSlotBasket : MonoBehaviour
    {
        [SerializeField]
        private FoodSlot _foodSlot;
        [Header("Put in")]
        [SerializeField]
        private float _timeMove;
        [SerializeField]
        private Ease _moveEase;
        [Header("Take out")]
        [SerializeField]
        private float _timeFly;
        [SerializeField]
        private Vector3 _flyHigh;
        [SerializeField]
        private Ease _fadeEase;
        public FoodItemData Item;
        public bool IsOccupied => Item != null;

        public void SetItem(FoodItemData item, Transform from, Action onDone)
        {
            Item = item;
            _foodSlot.DOKill();
            CanvasGroup canvasGroup = _foodSlot.GetComponent<CanvasGroup>();
            canvasGroup.DOKill();
            canvasGroup.alpha = 1;
            _foodSlot.gameObject.SetActive(true);

            _foodSlot.SetFoodSprite(item.GetIconDrink());
            _foodSlot.transform.localPosition = Vector3.zero;
            Vector3 position = _foodSlot.transform.position;
            _foodSlot.transform.localScale = Vector3.one;
            _foodSlot.transform.DOMove(position, _timeMove).SetEase(_moveEase).From(from.position)
                .OnComplete(() =>
                {
                    onDone?.Invoke();
                    _foodSlot.transform.localPosition = Vector3.zero;
                });
        }

        public void ClearSlot()
        {
            Item = null;
            _foodSlot.gameObject.SetActive(false);
        }

        public void TakeOut(Action onDone)
        {
            Item = null;
            //Vector3 position = _foodSlot.transform.position;
            //CanvasGroup canvasGroup = _foodSlot.GetComponent<CanvasGroup>();
            //_foodSlot.DOKill();
            //canvasGroup.DOKill();

            //_foodSlot.transform.DOMove(position + _flyHigh, _timeFly).OnComplete(() =>
            //{
            //    onDone?.Invoke();
            //    if (!IsOccupied)
            //    {
            //        _foodSlot.transform.position = position;
            //        _foodSlot.gameObject.SetActive(false);
            //    }
            //});
            //canvasGroup.DOFade(0, _timeFly).SetEase(_fadeEase);
        }
    }
}
