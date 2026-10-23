using UnityEngine;
using UnityEngine.UI;

namespace SLunch.Tray
{
    public class BigSlot : BaseSlot
    {
        protected override bool IsMaxPhase(int foodItemID)
        {
            FoodItemData foodData = _recipeController.GetFoodItemByID(foodItemID);

            if (foodData == null)
                return true;

            return _currentPhase >= foodData.BigPhases.Count;
        }

        protected override int GetMaxPhase(int foodItemID)
        {
            FoodItemData foodData = _recipeController.GetFoodItemByID(foodItemID);
            return foodData == null ? 0 : foodData.BigPhases.Count;
        }

        protected override void ChangeCurrentPhase(int foodItemID)
        {
            FoodItemData foodData = _recipeController.GetFoodItemByID(foodItemID);

            if (foodData == null)
                return;

            int phaseIndex = _currentPhase - 1;

            Sprite sprite;

            if (phaseIndex < foodData.BigPhases.Count)
                sprite = foodData.BigPhases[phaseIndex];
            else
                sprite = foodData.EndBigPhase;

            foodSlot.SetFoodSprite(sprite);
            if (foodData.BigPhasesPos.Count > 0) foodSlot.SetPosition(foodData.BigPhasesPos[0]);
            foodSlot.SetActiveVisual(true);
        }
    }
}
