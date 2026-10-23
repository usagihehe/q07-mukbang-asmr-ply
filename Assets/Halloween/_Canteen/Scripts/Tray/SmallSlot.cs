using UnityEngine;
using UnityEngine.UI;

namespace SLunch.Tray
{
	public class SmallSlot : BaseSlot
	{
		protected override bool IsMaxPhase(int foodItemID)
		{
            FoodItemData foodData = _recipeController.GetFoodItemByID(foodItemID);

            if (foodData == null)
                return true;

            return _currentPhase >= foodData.SmallPhases.Count;
        }

        protected override int GetMaxPhase(int foodItemID)
        {
            FoodItemData foodData = _recipeController.GetFoodItemByID(foodItemID);
            return foodData == null ? 0 : foodData.SmallPhases.Count;
        }

		protected override void ChangeCurrentPhase(int foodItemID)
		{
            FoodItemData foodData = _recipeController.GetFoodItemByID(foodItemID);

            if (foodData == null)
                return;

            int phaseIndex = _currentPhase - 1;

            Sprite sprite;

            if (phaseIndex < foodData.SmallPhases.Count)
                sprite = foodData.SmallPhases[phaseIndex];
            else
                sprite = foodData.EndSmallPhase;

            foodSlot.SetFoodSprite(sprite);
            if (foodData.SmallPhasesPos.Count > 0) foodSlot.SetPosition(foodData.SmallPhasesPos[0]);
            foodSlot.SetActiveVisual(true);
        }
    }
}
