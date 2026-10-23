using SLunch.Tray;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SLunch.CanteenManager;

namespace SLunch.Water
{
    public class TrayViewCtrl : MonoBehaviour
    {
        [SerializeField] private FoodSlot[] _imageFoods;
        private Dictionary<int, FoodSlot> dictFoods;

        private void OnEnable()
        {
            dictFoods = new Dictionary<int, FoodSlot>();
            for (int i = 0; i < _imageFoods.Length; i++)
            {
                int index = i;
                dictFoods[index] = _imageFoods[index];
                _imageFoods[index].SetActiveVisual(false);
            }
        }

        public void SetViewTray(Dictionary<int, TrayFoodItemDTO> dicts)
        {
            if (dicts == null || dictFoods == null)
                return;

            foreach (var pair in dictFoods)
            {
                int index = pair.Key;
                FoodSlot img = pair.Value;

                // Check if data exists for this slot
                if (dicts.TryGetValue(index, out TrayFoodItemDTO dto))
                {
                    // Enable image
                    img.SetActiveVisual(true);

                    // Assign sprite (assuming DTO has Sprite)
                    if (index == 0 || index == 1 || index == 2)
                    {
                        img.SetFoodSprite(dto.data.SmallPhases[dto.phase - 1]);
                        if (dto.data.SmallPhasesPos.Count > 0) img.SetPosition(dto.data.SmallPhasesPos[0]);
                    }
                    else if (index == 3 || index == 4)
                    {
                        img.SetFoodSprite(dto.data.BigPhases[dto.phase - 1]);
                        if (dto.data.BigPhasesPos.Count > 0) img.SetPosition(dto.data.BigPhasesPos[0]);
                    }
                }
                else
                {
                    // Hide if no data
                    img.SetActiveVisual(false);
                }
            }
        }
    }
}
