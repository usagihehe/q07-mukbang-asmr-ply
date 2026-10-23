using Capyboba.Mukbang;
using DG.Tweening;
using SLunch.Tray;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SLunch.Livestream
{
    public class CanteenDrinkMukbang : ItemMukbang
    {
        [SerializeField] protected Transform _iconParent;
        [SerializeField] protected Transform _drinkPoint;
        public Action<ItemMukbang> onConsume;
        public Transform DrinkPoint => _drinkPoint;
        public FoodItemData Item { get; protected set; }
        public override EaterGroup Group => EaterGroup.Canteen;
        public override EatStyle Style => EatStyle.Sip;
        public override Vector3 EatPoint => _drinkPoint.position;
        public override SoundType EatSound => Item.EatSound;
        protected virtual List<Sprite> Steps => Item.BigPhases;
        protected virtual Sprite EndPhaseIcon => Item.EndBigPhase;
        protected int maxPhase => Steps.Count;
        protected bool _isOpen;

        public virtual bool Init(FoodItemData item, int phase)
        {
            if (item == null) return false;
            Item = item;
            _isDone = false;

            _curStep = Item.BigPhases.Count - 1;
            _isOpen = !HasBox();
            _icon.sprite = Steps[_curStep];
            _icon.SetNativeSize();
            _iconParent.gameObject.SetActive(true);

            //Setup tool
            _tool.sprite = _icon.sprite;
            _tool.SetNativeSize();
            Vector2 spritePivot = _tool.sprite.pivot;
            Vector2 texSize = _tool.rectTransform.rect.size;
            _tool.rectTransform.pivot = new Vector2(spritePivot.x / texSize.x, spritePivot.y / texSize.y);
            return true;
        }

        public override EmotionType GetEmotion()
        {
            return Item.Emotion;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (!CanPick()) return;
            if (_isDone) return;
            if (!_isOpen) { OpenItem(); return; }
            base.OnPointerDown(eventData);
            Observer.Instance.NotifyWithData(ObserverTopic.OnSelectItem, this);
            HandleIconAt(_curStep, false);
            HandleTool();
            ShowTool();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if (_isDone) return;
            base.OnPointerUp(eventData);
            Observer.Instance.NotifyWithData(ObserverTopic.OnDropItem, this);
            HandleIconAt(_curStep);
            HandleTool();
            HideTool();
        }

        public override void Consume()
        {
            _curStep--;
            onConsume?.Invoke(this);
            // Step 0 is the last drink frame before EndPhaseIcon: it must get its own sip.
            if (_curStep == -1)
            {
                HandleIconAt(-1, true);
                _isDone = true;
                onDone?.Invoke(this);
            }
            HandleTool();
        }

        protected virtual void OpenItem()
        {
            _isOpen = true;
            _curStep--;
            HandleIconAt(_curStep);
            AudioManager.Instance.PlaySoundEffect(GameManager.Instance.GameSound.GetSound("Take-Item"));
        }

        protected void HandleIconAt(int index, bool active = true)
        {
            if (index >= 0 && index < maxPhase)
            {
                _icon.sprite = Steps[index];
            }
            else if (index == -1)
            {
                _icon.sprite = EndPhaseIcon;
            }
            _icon.SetNativeSize();
            _icon.gameObject.SetActive(active);
        }

        private void HandleTool()
        {
            if (_isDone) { HideTool(); return; }
            _tool.sprite = Steps[_curStep];
            _tool.SetNativeSize();
        }

        public override void ShowTool()
        {
            if (_tool != null)
            {
                _tool.gameObject.SetActive(true);
                _tool.transform.DOScale(Vector3.one, _scaleTime).From(Vector3.zero);
            }
        }

        public override void HideTool()
        {
            Observer.Instance.NotifyWithData(ObserverTopic.OnDropItem, this);
            base.HideTool();
        }

        private bool HasBox()
        {
            return Steps != null && Steps[Steps.Count - 1].name.EndsWith("_0");
        }
    }
}
