using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SLunch.Tray
{
    public class TrayController : MonoBehaviour, ITrayController
    {
        [SerializeField] private TrayVisual _trayVisual;
        [Header("DEBUG SETTINGS")]
        [SerializeField] private int _slotID;
        [SerializeField] private int _itemID;
        [SerializeField] private List<BaseSlot> _baseSlots;

        public TrayVisual TrayVisual => _trayVisual;
        public List<BaseSlot> BaseSlots => _baseSlots;

        private Vector3 _defaultScale;

        private void Awake()
        {
            _defaultScale = transform.localScale;
        }

        private void OnEnable()
        {
            Clear();
            transform.localScale = _defaultScale;
        }

        public BaseSlot GetSlotByID(int id)
        {
            foreach (BaseSlot slot in _baseSlots)
            {
                if (slot.ID == id) return slot;
            }
            return null;
        }

        public int GetCurrentFoodIDInSlot(int slotID)
        {
            BaseSlot slot = GetSlotByID(slotID);
            return slot.CurrentResultFoodID;
        }

        public bool AddFoodToSlot(int slotID, int foodID)
        {
            ITraySlot slot = GetSlotByID(slotID);
            bool isComplete = slot.TryAddFood(foodID);
            return isComplete;
        }

        public bool HasAnyEmptySlot()
        {
            foreach (ITraySlot slot in _baseSlots)
            {
                if (slot.IsEmpty()) return true;
            }
            return false;
        }

        public void Clear()
        {
            foreach (BaseSlot slot in _baseSlots)
            {
                slot.ClearSlot();
            }
        }

        public void ClearSlot(int slotID)
        {
            GetSlotByID(slotID).ClearSlot();
        }

        public void SetActiveHighlight(int slotID, bool isActive, int foodID = -1)
        {
            BaseSlot slot = GetSlotByID(slotID);
            slot.SetActiveNormalHighlight(isActive);

        }

        public void ActiveEffectBling(int slotID)
        {
            BaseSlot slot = GetSlotByID(slotID);
            slot.ActiveEffectBling();
        }

        public void ActiveEffectFull(int slotID)
        {
            BaseSlot slot = GetSlotByID(slotID);
            slot.ActiveEffectFull();
        }

        public void ScaleUp()
        {
            transform.DOScale(_defaultScale * 1.05f, 0.2f);
        }

        public void ScaleDown()
        {
            transform.DOScale(_defaultScale, 0.2f);
        }

        public void MoveUp(float positionY, float duration, int loopTime = 0, LoopType loopType = LoopType.Yoyo, Action onCompleted = null)
        {
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }

        public virtual BaseSlot GetNearestSlot(RectTransform pickerRect, float snapDistance = 100f)
        {
            BaseSlot nearestSlot = null;

            Vector3 foodPos = pickerRect.position;

            float minSqrDist = float.MaxValue;
            float maxSqrDist = snapDistance * snapDistance;

            for (int i = 0; i < _baseSlots.Count; i++)
            {
                int index = i;
                Vector3 slotPos = _baseSlots[index].transform.position;


                // EARLY REJECT
                float dx = Mathf.Abs(slotPos.x - foodPos.x);
                if (dx > snapDistance) continue;

                float dy = Mathf.Abs(slotPos.y - foodPos.y);
                if (dy > snapDistance) continue;

                float sqrDist = (slotPos - foodPos).sqrMagnitude;

                if (sqrDist < maxSqrDist && sqrDist < minSqrDist)
                {
                    minSqrDist = sqrDist;
                    nearestSlot = _baseSlots[index];
                }
            }

            return nearestSlot;
        }
    }
}
