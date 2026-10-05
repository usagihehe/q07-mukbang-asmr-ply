using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Usaki;
using Utils.Singletons;

namespace Usaki
{
    public class UIManager : ZMonoSingleton<UIManager>
    {
        [SerializeField] private SupermarketLivePanel _supermarketLivePanel;
        [SerializeField] public PayPanel _payPanel;
        [SerializeField] public NotifyCtrl _notifyCtrl;

        [SerializeField] private Image lockImage;
        [SerializeField] private HandHint handHint;

        private bool _isEatTutorialActive;
        private Transform _eatTutorialSlot;
        private Transform _eatTutorialTarget;
        private Canvas canvas;

        public SupermarketLivePanel SupermarketLivePanel => _supermarketLivePanel;
        public PayPanel PayPanel => _payPanel;
        public NotifyCtrl Notify => _notifyCtrl;
        public HandHint HandHint => handHint;

        protected override void Awake()
        {
            base.Awake();
            canvas = GetComponent<Canvas>();
        }

        public void ShowSupermarketLivePanel(List<SupermarketItemSO> items)
        {
            SupermarketLivePanel.gameObject.SetActive(true);
            SupermarketLivePanel.ShowPanel(items);
        }

        public void ShowPayPanel(List<SupermarketItemSO> items)
        {
            PayPanel.gameObject.SetActive(true);
            PayPanel.ShowPanel(items);
        }

        public void ActiceLockImage()
        {
            lockImage.gameObject.SetActive(true);
        }

        public void ClickHandAt(Transform target) => HandHint.ShowHint(target, canvas);

        public void MoveHand(Transform from, Transform to) => handHint.MoveHint(from, to, canvas);

        public void StopHand() => HandHint.StopHint();

        public void StartEatTutorial(Transform slot, Transform target)
        {
            _isEatTutorialActive = true;
            _eatTutorialSlot = slot;
            _eatTutorialTarget = target;
            handHint.ShowHint(slot, canvas);
        }

        public void OnEatSlotPress()
        {
            if (!_isEatTutorialActive) return;
            handHint.MoveHint(_eatTutorialSlot, _eatTutorialTarget, canvas);
        }

        public void OnEatSlotRelease()
        {
            if (!_isEatTutorialActive) return;
            handHint.ShowHint(_eatTutorialSlot, canvas);
        }

        public void CompleteEatTutorial()
        {
            if (!_isEatTutorialActive) return;
            _isEatTutorialActive = false;
            handHint.StopHint();
        }
    }
}
