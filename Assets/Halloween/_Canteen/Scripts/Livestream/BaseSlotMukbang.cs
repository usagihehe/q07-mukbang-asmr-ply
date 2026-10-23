using Capyboba.Mukbang;
using DG.Tweening;
using SLunch.Tray;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SLunch.Livestream
{
    public abstract class BaseSlotMukbang : ItemMukbang
    {
        [SerializeField] protected Transform _iconParent;
        public Action<ItemMukbang> onConsume;
        public FoodItemData Item { get; protected set; }
        public override EaterGroup Group => EaterGroup.Canteen;
        public override SoundType EatSound => Item.EatSound;
        protected virtual List<Sprite> Steps => Item.SmallPhases;
        protected virtual Sprite EndPhaseIcon => Item.EndSmallPhase;
        protected int maxPhase => Steps.Count;
        public virtual bool Init(FoodItemData item, int phase)
        {
            if (item == null) return false;
            Item = item;

            _isDone = false;
            _curStep = phase - 1;
            InitIcon(item, phase);
            //Setup tool
            _tool.transform.localScale = Vector3.one;
            _tool.sprite = Item.ToolFull;
            _tool.SetNativeSize();
            Vector2 spritePivot = _tool.sprite.pivot;
            Vector2 texSize = _tool.rectTransform.rect.size;
            _tool.rectTransform.pivot = new Vector2(spritePivot.x / texSize.x, spritePivot.y / texSize.y);
            return true;
        }

        protected virtual void InitIcon(FoodItemData item, int phase)
        {
            _icon.sprite = Steps[phase - 1];
            _icon.SetNativeSize();
            _icon.rectTransform.anchoredPosition = Vector2.zero;
            _iconParent.gameObject.SetActive(true);
        }

        public override EmotionType GetEmotion()
        {
            return Item.Emotion;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (!CanPick()) return;
            if (_isDone) return;
            base.OnPointerDown(eventData);
            Observer.Instance.NotifyWithData(ObserverTopic.OnSelectItem, this);
            HandleIconAt(_curStep - 1, true);
            HandleTool();
            ShowTool();
        }

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
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
            if (_curStep == -1)
            {
                HandleIconAt(-1, true);
                _isDone = true;
                onDone?.Invoke(this);
            }
            HideTool();
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
            _tool.SetNativeSize();
        }

        public override void ShowTool()
        {
            if (_tool != null)
            {
                _tool.gameObject.SetActive(true);
                _tool.transform.localScale = Vector3.one;
                _tool.sprite = Item.ToolFull;
                _tool.SetNativeSize();
                Vector2 spritePivot = _tool.sprite.pivot;
                Vector2 texSize = _tool.rectTransform.rect.size;
                _tool.rectTransform.pivot = new Vector2(spritePivot.x / texSize.x, spritePivot.y / texSize.y);
                _tool.transform.DOScale(Vector3.one, _scaleTime).From(Vector3.zero);
            }
        }

        public override void HideTool()
        {
            Observer.Instance.NotifyWithData(ObserverTopic.OnDropItem, this);
            base.HideTool();
        }
    }
}
