using System.Collections.Generic;

namespace SLunch.Tray
{
	public interface IRecipeController
	{
		FoodItemData GetFoodItemByID(int id);

		RecipeData GetRecipeByID(int id);

		bool HasExactMatchRecipe(List<int> currentFoodIDs);

		bool HasPotentialRecipe(List<int> currentFoodIDs);

		RecipeData GetExactMatchRecipe(List<int> currentFoodIDs);

		RecipeData GetPotentialRecipe(List<int> currentFoodIDs);
	}
}
