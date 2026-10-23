using SLunch.Tray;
using Spine;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SLunch.Water
{
    public class DrinkSlot : MonoBehaviour
    {
        [SerializeField] protected List<Image> _viewItems;
        [SerializeField] protected FoodItemData _itemSO;
        [SerializeField] protected Image _lock;
        protected bool _canPick;
        public FoodItemData Item => _itemSO;
        public virtual bool CanPick
        {
            get { return _canPick; }
            set
            {
                _canPick = value;
            }
        }

        protected virtual void Awake()
        {
            Init(_itemSO);
            GetComponent<Button>().onClick.AddListener(() =>
            {
                AudioManager.Instance.PlaySoundEffect(GameManager.Instance.GameSound.GetSound("click"));
                Observer.Instance.NotifyWithData(ObserverTopic.OnPickItem, this);
            });
        }

        private void OnEnable()
        {
            if (_lock == null || Item == null) return;
            _lock.gameObject.SetActive(false);
        }

        public virtual bool Init(FoodItemData itemSO)
        {
            if (itemSO == null)
            {
                _canPick = false;
                return _canPick;
            }
            _itemSO = itemSO;
            foreach (var item in _viewItems)
            {
                item.sprite = itemSO.GetIconDrink();
                item.SetNativeSize();
            }

            _canPick = true;
            return _canPick;
        }

        public virtual GameObject GetFirstView()
        {
            return _viewItems[_viewItems.Count - 1].gameObject;
        }
    }
}
