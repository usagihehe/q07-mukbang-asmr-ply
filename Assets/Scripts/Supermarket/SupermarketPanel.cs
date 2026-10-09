using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Usaki
{
    public class SupermarketPanel : MonoBehaviour
    {
        [SerializeField] protected int _requireAmount;
        [SerializeField] protected Button _doneBtn;
        [SerializeField] protected Button _leftArrowBtn;
        [SerializeField] protected Button _rightArrowBtn;
        [SerializeField] protected List<RectTransform> _shelfs;
        [SerializeField] protected ScrollRect _scrollRect;
        [SerializeField] protected TextMeshProUGUI _totalTxt;
        [SerializeField] protected TextMeshProUGUI _goodsNumTxt;
        [SerializeField] protected BasketCtrl _basketCtrl;
        [SerializeField] protected float _timeIncrPrice;
        [SerializeField] protected AudioClip _bgm;
        [SerializeField] protected float _moveTime;
        [SerializeField] protected Ease _ease;

        private int _curTotalPrice;
        private const int MAX_BASKET = 5;
        public BasketCtrl Basket => _basketCtrl;

        protected virtual void Awake()
        {
            _doneBtn.onClick.AddListener(OnPressDoneBtn);
            _doneBtn.interactable = false;
            AudioManager.Instance.PlayBGM(_bgm);
        }

        protected virtual void OnEnable()
        {
            Observer.Instance.AddObserver(ObserverTopic.OnPickItem, OnPickItem);
            Observer.Instance.AddObserver(ObserverTopic.OnTakeOut, OnTakeOut);
            ShowPanel();
            _curTotalPrice = 0;
            _goodsNumTxt.text = $"{_basketCtrl.PickNum}/{MAX_BASKET}";
            _totalTxt.text = _curTotalPrice + "";
            _doneBtn.interactable = false;
            // Single-shelf variants have no scroll view.
            if (_scrollRect != null) _scrollRect.horizontalNormalizedPosition = 0f;
        }

        protected virtual void OnPressDoneBtn()
        {
            UIManager.Instance.ShowPayPanel(_basketCtrl.Items);
            ShowOuttro(() => gameObject.SetActive(false));
        }

        protected virtual void OnPressXBtn()
        {
            ShowOuttro(() => gameObject.SetActive(false));
        }

        protected virtual void OnDisable()
        {
            Observer.Instance.RemoveObserver(ObserverTopic.OnPickItem, OnPickItem);
            Observer.Instance.RemoveObserver(ObserverTopic.OnTakeOut, OnTakeOut);
        }

        protected virtual void OnPickItem(object data)
        {
            SlotShelfCtrl slot = data as SlotShelfCtrl;
            if (_basketCtrl.PickNum >= MAX_BASKET) { UIManager.Instance.Notify.Show("Basket is full"); return; }
            //No need check coin 
            _curTotalPrice += slot.Item.Price;
            _basketCtrl.PutInItem(slot, () =>
            {
                _goodsNumTxt.text = $"{_basketCtrl.PickNum}/{MAX_BASKET}";
                _totalTxt.text = $"{_curTotalPrice}";
                _doneBtn.interactable = _basketCtrl.PickNum >= _requireAmount;
            });
        }

        protected virtual void OnTakeOut(object data)
        {
            if (_basketCtrl.PickNum == 0) { UIManager.Instance.Notify.Show("Basket is empty"); }
            _basketCtrl.PutOutItem();
            int total = 0;
            foreach (var item in _basketCtrl.Items)
            {
                total += item.Price;
            }
            _curTotalPrice = total;
            _totalTxt.text = $"{_curTotalPrice}";
            _doneBtn.interactable = _basketCtrl.PickNum >= _requireAmount;
            _goodsNumTxt.text = $"{_basketCtrl.PickNum}/{MAX_BASKET}";
        }

        public virtual void ShowPanel()
        {
            ShowIntro(null);
        }

        public virtual void ShowIntro(Action onCompleted)
        {
            onCompleted?.Invoke();
        }

        public virtual void ShowOuttro(Action onCompleted)
        {
            onCompleted?.Invoke();
        }

        public virtual void ScrollToLeftChild(ScrollRect scrollRect, List<RectTransform> children)
        {

        }

        public virtual void ScrollToRightChild(ScrollRect scrollRect, List<RectTransform> children)
        {

        }



    }
}
