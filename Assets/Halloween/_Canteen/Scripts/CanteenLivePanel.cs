using DG.Tweening;
using SLunch.Tray;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Usaki;
using static SLunch.CanteenManager;

namespace SLunch.Livestream
{
    public class CanteenLivePanel : MonoBehaviour, IPanel
    {
        #region === UI Components ===
        [Header("General UI References")]
        [SerializeField] private Button _phoneBtn;
        //[SerializeField] private TextMeshProUGUI _viewerTxt;
        //[SerializeField] private TextMeshProUGUI _liveTxt;
        [SerializeField] private Image _flash;
        [SerializeField] private GameObject _lockImage;

        [Header("Character & Coin")]
        [SerializeField] private CharacterCtrl _character;
        [SerializeField] private Transform _showCoinPos;
        [SerializeField] private CoinBarCtrl _coinBarCtrl;

        [Header("Mukbang Items")]
        [SerializeField] private List<BaseSlotMukbang> _itemMukbangs;
        [SerializeField] private CanteenDrinkMukbang _drinkMukbang;

        [Header("Settings")]
        [SerializeField] private AudioClip _bgm;
        [SerializeField] private int _foodBitesToComplete = 3;
        #endregion

        #region === Private Fields ===
        private CanteenManager canteenManager => CanteenManager.Instance;
        private Dictionary<int, TrayFoodItemDTO> _items;
        private int _coutItemComplete;
        protected int _countItem;
        private bool _isFinished;
        #endregion

        private void Start()
        {

            _phoneBtn.onClick.AddListener(PlayFlash);
        }

        private void OnEnable()
        {
            AudioManager.Instance.PlayBGM(_bgm);
            _countItem = 0;
            _isFinished = false;
            for (int i = 0; i < _itemMukbangs.Count; i++)
            {
                _itemMukbangs[i].gameObject.SetActive(false);
                _itemMukbangs[i].onDone = null;
                _itemMukbangs[i].onConsume = null;
            }
            _drinkMukbang.gameObject.SetActive(false);
            _drinkMukbang.onDone = null;
            Observer.Instance.AddObserver(ObserverTopic.OnEndLiveGamePlay, OnEndLiveGamePlay);
        }

        private void OnDisable()
        {
            Observer.Instance.RemoveObserver(ObserverTopic.OnEndLiveGamePlay, OnEndLiveGamePlay);
        }

        private void OnEndLiveGamePlay(object data)
        {
            if (_lockImage != null) _lockImage.SetActive(true);
        }

        public void ShowPanel(Dictionary<int, TrayFoodItemDTO> itemDataDict, FoodItemData drinkData)
        {
            ItemMukbang.BlockPick = true;
            ShowIntro(() =>
            {
                ItemMukbang.BlockPick = false;
            });

            _items = itemDataDict;

            var itemList = _items.ToList();
            for (int i = 0; i < _items.Count; i++)
            {
                int index = i;
                var idTray = itemList[index].Key;
                var dto = itemList[index].Value;
                _itemMukbangs[idTray].gameObject.SetActive(true);
                _itemMukbangs[idTray].Init(dto.data, dto.phase);
                _itemMukbangs[idTray].onDone = OnDone;
                _itemMukbangs[idTray].onConsume = OnConsume;
            }

            // No drink chosen: the drink stays hidden (OnEnable) and is not counted below.
            if (drinkData != null)
            {
                _drinkMukbang.gameObject.SetActive(true);
                _drinkMukbang.Init(drinkData, drinkData.BigPhases.Count);
                _drinkMukbang.onDone = OnDone;
            }

            _coutItemComplete = itemList.Count + (drinkData != null ? 1 : 0);

            GameManager.Instance.SetCountComplete(_foodBitesToComplete);
            Observer.Instance.Notify(ObserverTopic.OnCanteenStartLive);
        }

        private void OnConsume(ItemMukbang item)
        {
            Observer.Instance.NotifyWithData(ObserverTopic.OnCanteenConsumeItem, item);
            GameManager.Instance.UpdateCountComplete();
        }

        private void OnDone(ItemMukbang item)
        {
            _countItem++;
            ClaimCoin();
            OnLiveFinished();
        }


        private void OnLiveFinished()
        {
            if (_isFinished || _countItem != _coutItemComplete) return;
            _isFinished = true;
        }

        private void ClaimCoin(int coinClaim = 30)
        {
            if (_coinBarCtrl != null) _coinBarCtrl.AddCoin(coinClaim);
        }

        public void ShowIntro(Action onCompleted)
        {
            gameObject.SetActive(true);
            onCompleted?.Invoke();
        }

        public void ShowOuttro(Action onCompleted)
        {
            gameObject.SetActive(false);
            onCompleted?.Invoke();
        }

        #region PLAY FLASH
        private float flashInDuration = 0.05f;
        private float flashOutDuration = 0.2f;
        private bool isFlashing = false;
        public void PlayFlash()
        {
            if (isFlashing) return;
            isFlashing = true;

            _flash.color = new Color(1, 1, 1, 0);
            _flash.gameObject.SetActive(true);

            Sequence flashSequence = DOTween.Sequence();
            flashSequence.Append(_flash.DOFade(1f, flashInDuration))
                         .Append(_flash.DOFade(0f, flashOutDuration))
                         .OnComplete(() =>
                         {
                             _flash.gameObject.SetActive(false);
                             isFlashing = false; // Allow next flash
                         });

            _character.PlayAnimFlash(false, delegate
            {
                _character.PlayAnimByName(CharacterCtrl.IDLE, true);
            });
        }
        #endregion

    }
}
