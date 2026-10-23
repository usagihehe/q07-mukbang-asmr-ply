using SLunch.Tray;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SLunch.Livestream
{
    public class SmallSlotMukbang : BaseSlotMukbang
    {
        protected override List<Sprite> Steps => Item.SmallPhases;
        protected override Sprite EndPhaseIcon => Item.EndSmallPhase;

        protected override void InitIcon(FoodItemData item, int phase)
        {
            base.InitIcon(item, phase);
            if (item.SmallPhasesPos.Count > 0)
                _icon.rectTransform.anchoredPosition = item.SmallPhasesPos[0];
        }
    }
}
