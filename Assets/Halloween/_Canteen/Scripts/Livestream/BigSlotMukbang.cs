using SLunch.Tray;
using System.Collections.Generic;
using UnityEngine;

namespace SLunch.Livestream
{
    public class BigSlotMukbang : BaseSlotMukbang
    {
        protected override List<Sprite> Steps => Item.BigPhases;
        protected override Sprite EndPhaseIcon => Item.EndBigPhase;

        protected override void InitIcon(FoodItemData item, int phase)
        {
            base.InitIcon(item, phase);
            if (item.BigPhasesPos.Count > 0)
                _icon.rectTransform.anchoredPosition = item.BigPhasesPos[0];
        }
    }
}
