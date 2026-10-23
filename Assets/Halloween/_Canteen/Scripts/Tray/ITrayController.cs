using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace SLunch.Tray
{
    public interface ITrayController
    {
        bool AddFoodToSlot(int slotID, int foodID);

        bool HasAnyEmptySlot();

        void Clear();

        void ClearSlot(int slotID);

        void SetActiveHighlight(int slotID, bool isActive, int foodID = -1);

        void ActiveEffectBling(int slotID);

        void ActiveEffectFull(int slotID);

        void ScaleUp();

        void ScaleDown();

        int GetCurrentFoodIDInSlot(int slotID);

        void MoveUp(float positionY, float duration, int loopTime = 0, LoopType loopType = LoopType.Yoyo, Action onCompleted = null);


    }
}
