using SLunch.Tray;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


namespace SLunch.Water
{
    public class WaterBasketCtrl : MonoBehaviour
    {
        [SerializeField]
        private Button _button;
        [SerializeField]
        private List<WaterSlotBasket> _basketSlots;
        private bool _canTakeOut;
        public List<WaterSlotBasket> BasketSlots => _basketSlots;
        public List<FoodItemData> Items => _basketSlots.FindAll(slot => slot.Item != null).ConvertAll(slot => slot.Item);
        public bool IsFull => PickNum == _basketSlots.Count;
        public int PickNum => _basketSlots.FindAll(slot => slot.Item != null).Count;

        private void Awake()
        {
            _button.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlaySoundEffect(GameManager.Instance.GameSound.GetSound("click"));
                //Observer.Instance.NotifyWithData(ObserverTopic.OnTakeOut, this);
            });
        }

        private void OnEnable()
        {
            _canTakeOut = false;
            ClearBasket();
        }

        public void ClearBasket()
        {
            foreach (var basket in _basketSlots)
            {
                basket.ClearSlot();
            }
        }

        public bool PutInItem(DrinkSlot fromSlot, Action onPutDone)
        {
            WaterSlotBasket slot = GetSlot();
            if (slot == null)
                return false;
            else
            {
                _canTakeOut = false;
                fromSlot.GetFirstView().SetActive(false);
                slot.SetItem(fromSlot.Item, fromSlot.GetFirstView().transform, () =>
                {
                    fromSlot.GetFirstView().SetActive(true);
                    onPutDone?.Invoke();
                    _canTakeOut = PickNum > 0;
                });
                return true;
            }
        }

        public bool TakeOutItem()
        {
            if (_canTakeOut)
            {
                if (GetLastPutIn() == null) return false;
                else
                {
                    GetLastPutIn().TakeOut(null);
                    _canTakeOut = PickNum > 0;
                }
                return true;
            }
            return false;

        }

        private WaterSlotBasket GetSlot()
        {
            foreach (var slot in BasketSlots)
            {
                if (slot.Item == null)
                {
                    return slot;
                }
            }
            return null;
        }

        private WaterSlotBasket GetLastPutIn()
        {
            return _basketSlots[PickNum - 1];
        }
    }
}
