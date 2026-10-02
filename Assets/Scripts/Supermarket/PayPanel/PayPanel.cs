using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Usaki
{
    public class PayPanel : MonoBehaviour
    {
        [SerializeField]
        protected PayMachineCtrl _payMachineCtrl;

        [SerializeField]
        protected Button _doneBtn;


        [SerializeField]
        protected float _timeToNext;

        [SerializeField]
        protected CoinBarCtrl _coinBarCtrl;

        protected List<SupermarketItemSO> _items;
        [SerializeField] private AudioClip _scanComplete;

        public List<SupermarketItemSO> Items => _items;

        private void Awake()
        {
            _doneBtn.onClick.AddListener(OnPressDoneBtn);
            _payMachineCtrl.onFirstScan = OnFirstScan;
        }

        private void OnEnable()
        {
            _doneBtn.gameObject.SetActive(false);
        }

        public void ShowPanel(List<SupermarketItemSO> items)
        {
            _items = items;
            _payMachineCtrl.SetItems(items);
            UIManager.Instance.MoveHand(_payMachineCtrl.FirstScanItem, _payMachineCtrl.Scan.transform);
        }

        private void OnFirstScan() => UIManager.Instance.StopHand();

        protected virtual void OnPressDoneBtn()
        {
            AudioManager.Instance.PlaySoundEffect(_scanComplete);
            int total = 0;
            foreach (var item in _items)
            {
                total += item.Price;
            }
            _coinBarCtrl.TakeCoin(total);

            DOVirtual.DelayedCall(_timeToNext, () =>
            {
                UIManager.Instance.ShowSupermarketLivePanel(_items);
                ShowOuttro(() => gameObject.SetActive(false));
            });

        }

        public void ShowIntro(Action onCompleted)
        {
            onCompleted?.Invoke();
        }

        public void ShowOuttro(Action onCompleted)
        {
            onCompleted?.Invoke();
        }

      
    }
}
