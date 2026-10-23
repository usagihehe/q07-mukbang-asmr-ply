using System;
using UnityEngine;

namespace SLunch
{
    public interface IBowlFood
    {
        void ShowFull();

        void ShowUsed();

        void SetData(BowlFoodData data);

        BowlFoodData GetData();

        Vector3 GetLocalScale();
    }
}
