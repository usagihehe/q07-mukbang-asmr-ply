using SLunch.Tray;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Usaki;
using static SLunch.CanteenManager;


namespace SLunch.Water
{
    public class CanteenWaterChoose : MonoBehaviour
    {
        [SerializeField] protected Button _doneBtn;
        [SerializeField] protected ScrollRect _scrollRect;
        [SerializeField] protected WaterBasketCtrl _basketCtrl;
        [SerializeField] protected TrayViewCtrl _trayViewCtrl;

        private const int MAX_BASKET = 1;
        protected CanteenPanel _canteenPanel;
        public WaterBasketCtrl BasketCtrl => _basketCtrl;

        protected virtual void Awake()
        {
            _doneBtn.onClick.AddListener(OnPressDoneBtn);
            _doneBtn.gameObject.SetActive(false);
        }

        public void Setup(CanteenPanel panel)
        {
            _canteenPanel = panel;
        }

        protected virtual void OnEnable()
        {
            Observer.Instance.AddObserver(ObserverTopic.OnPickItem, OnPickItem);

            _doneBtn.gameObject.SetActive(false);
            _scrollRect.horizontalNormalizedPosition = 0.2f;
        }

        protected virtual void OnDisable()
        {
            Observer.Instance.RemoveObserver(ObserverTopic.OnPickItem, OnPickItem);
        }

        protected virtual void OnPressDoneBtn()
        {
            CanteenManager.Instance.ShowCanteenLivePanel();
            _canteenPanel.ShowOuttro(() => _canteenPanel.SetActive(false));
        }
        protected virtual void OnPickItem(object data)
        {
            DrinkSlot slot = data as DrinkSlot;
            
            if (_basketCtrl.PickNum > 0)
            {
                _basketCtrl.TakeOutItem();
            }

            _basketCtrl.PutInItem(slot, () =>
            {
                _doneBtn.gameObject.SetActive(_basketCtrl.PickNum >= MAX_BASKET);
            });
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public void ShowCanteenWaterChoose(Dictionary<int, TrayFoodItemDTO> dicts)
        {
            SetActive(true);
            _trayViewCtrl.SetViewTray(dicts);
        }
    }
}
