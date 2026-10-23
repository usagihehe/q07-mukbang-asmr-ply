using System.Collections.Generic;
using UnityEngine;

namespace SLunch.Tray
{
    public class RecipeController : MonoBehaviour, IRecipeController
    {
        private RecipeDataConfig _recipeData => CanteenManager.Instance.RecipeData;
        private FoodItemSO _foodItemData => CanteenManager.Instance.FoodItemSO;

        public FoodItemData GetFoodItemByID(int id)
        {
            FoodItemData foodItemData = _foodItemData.GetFoodItemByID(id);
            if (foodItemData != null) return foodItemData;
            else return null;
        }

        public RecipeData GetRecipeByID(int id)
        {
            return _recipeData.GetRecipeByID(id);
        }

        public bool HasExactMatchRecipe(List<int> currentFoodIDs)
        {
            foreach (var recipe in _recipeData.Recipes)
            {
                if (recipe.IngredientIds.Count != currentFoodIDs.Count) continue;

                bool match = true;
                foreach (var id in currentFoodIDs)
                {
                    if (!recipe.IngredientIds.Contains(id))
                    {
                        match = false;
                        break;
                    }
                }
                if (match) return true;
            }
            return false;
        }

        public bool HasPotentialRecipe(List<int> currentFoodIDs)
        {
            foreach (var recipe in _recipeData.Recipes)
            {
                if (recipe.IngredientIds.Count < currentFoodIDs.Count) continue;
                for (int i = 0; i < currentFoodIDs.Count; i++)
                {
                    int index = i;
                    if (!recipe.IngredientIds.Contains(currentFoodIDs[index]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public RecipeData GetExactMatchRecipe(List<int> currentFoodIDs)
        {
            foreach (var recipe in _recipeData.Recipes)
            {
                if (recipe.IngredientIds.Count != currentFoodIDs.Count) continue;

                List<int> temp = new List<int>(recipe.IngredientIds);

                bool match = true;

                foreach (var id in currentFoodIDs)
                {
                    if (!temp.Remove(id))
                    {
                        match = false;
                        break;
                    }
                }

                if (match) return recipe;
            }

            return null;
        }

        public RecipeData GetPotentialRecipe(List<int> currentFoodIDs)
        {
            foreach (var recipe in _recipeData.Recipes)
            {
                if (recipe.IngredientIds.Count < currentFoodIDs.Count) continue;

                bool potential = true;
                foreach (var id in currentFoodIDs)
                {
                    if (!recipe.IngredientIds.Contains(id))
                    {
                        potential = false;
                        break;
                    }
                }

                if (potential) return recipe;
            }

            return null;
        }
    }
}
