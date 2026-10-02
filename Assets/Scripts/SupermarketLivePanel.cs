using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Usaki;

namespace Usaki
{
    public class SupermarketLivePanel : MonoBehaviour
    {
        #region === UI Components ===
        [Header("General UI References")]
        [SerializeField] private GameObject _tapToMukbang;

        [Header("Character & Coin")]
        [SerializeField] private CharacterCtrl _character;

        [Header("Mukbang Items")]
        [SerializeField] private List<SupermarketItemMukbang> _itemMukbangs;

        [Header("Settings")]
        [SerializeField] private float _delayToNext;
        [SerializeField] private AudioClip _bgm;
        [SerializeField] private int _countToComplete = 3;

        [Header("Food Set")]
        [SerializeField] private List<SupermarketItemSO> _itemSets;
        #endregion

        #region === Private Fields ===
        private List<SupermarketItemSO> _items;
        protected int _countItem;
        protected bool _isFirstConsume;
        private bool _canScoreThisHold;
        #endregion

        private void OnEnable()
        {
            AudioManager.Instance.PlayBGM(_bgm);
            _countItem = 0;
            _isFirstConsume = false;
            _canScoreThisHold = false;
            for (int i = 0; i < _itemMukbangs.Count; i++)
            {
                _itemMukbangs[i].gameObject.SetActive(false);
                _itemMukbangs[i].onDone = null;
                _itemMukbangs[i].onConsume = null;
            }
            if (_tapToMukbang != null) _tapToMukbang.SetActive(true);
            Observer.Instance.AddObserver(ObserverTopic.OnSelectItem, OnSelectItem);
            Observer.Instance.AddObserver(ObserverTopic.OnDropItem, OnDropItem);
            Observer.Instance.AddObserver(ObserverTopic.OnEndLiveGamePlay, OnEndLiveGamePlay);
        }

        protected void OnDisable()
        {
            Observer.Instance.RemoveObserver(ObserverTopic.OnSelectItem, OnSelectItem);
            Observer.Instance.RemoveObserver(ObserverTopic.OnDropItem, OnDropItem);
            Observer.Instance.RemoveObserver(ObserverTopic.OnEndLiveGamePlay, OnEndLiveGamePlay);
        }

        private void OnValidate()
        {
            for (int i = 0; i < _itemSets.Count; i++)
            {
                int index = i;
                _itemMukbangs[index].gameObject.SetActive(true);
                _itemMukbangs[index].Init(_itemSets[index]);
            }
        }

        private void OnSelectItem(object data)
        {
            _canScoreThisHold = true;
            UIManager.Instance.OnEatSlotPress();
        }

        private void OnDropItem(object data)
        {
            UIManager.Instance.OnEatSlotRelease();
        }

        public void ShowPanel(List<SupermarketItemSO> items)
        {
            ItemMukbang.BlockPick = true;
            _items = items;
            for (int i = 0; i < items.Count; i++)
            {
                int index = i;
                _itemMukbangs[index].gameObject.SetActive(true);
                _itemMukbangs[index].Init(items[index]);
                _itemMukbangs[index].onDone = OnDone;
                _itemMukbangs[index].onConsume = OnConsume;
            }
            ItemMukbang.BlockPick = false;

            GameManager.Instance.SetCountComplete(_countToComplete);
            UIManager.Instance.StartEatTutorial(_itemMukbangs[0].transform, _character.mPoint);
        }

        private void OnDone(ItemMukbang item)
        {
            _countItem++;
            ClaimCoin(item);
        }

        private void OnConsume(ItemMukbang item)
        {
            if (_tapToMukbang != null) _tapToMukbang.SetActive(false);
            UIManager.Instance.CompleteEatTutorial();

            // One press-and-hold scores once, however many bites it produces.
            if (!_canScoreThisHold) return;
            _canScoreThisHold = false;
            GameManager.Instance.UpdateCountComplete();
        }

        private void ClaimCoin(object data)
        {
        }

        private void OnEndLiveGamePlay(object data)
        {
            UIManager.Instance.StopHand();
            UIManager.Instance.ActiceLockImage();
        }
    }
}
