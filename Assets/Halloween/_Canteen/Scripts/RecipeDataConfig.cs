using System.Collections.Generic;
using UnityEngine;

namespace SLunch
{
    [CreateAssetMenu(fileName = "RecipeDataConfig", menuName = "SLunch/RecipeDataConfig")]
    public class RecipeDataConfig : ScriptableObject
    {
        [SerializeField] public List<RecipeData> _recipes;
        public List<RecipeData> Recipes => _recipes;

        public RecipeData GetRecipeByID(int id)
        {
            foreach (var recipe in _recipes)
            {
                if (recipe.ResultId == id) return recipe;
            }
            return null;
        }

        private List<int> ConvertStringToListInt(string hintString)
        {
            List<int> result = new List<int>();

            if (string.IsNullOrEmpty(hintString))
                return result;

            var parts = hintString.Split(';');

            foreach (var p in parts)
            {
                if (int.TryParse(p, out int value))
                {
                    result.Add(value);
                }
            }

            return result;
        }

        private string ConvertListIntToString(List<int> list)
        {
            if (list == null || list.Count == 0)
                return string.Empty;

            return string.Join(";", list);
        }

        [ContextMenu("SetValue")]
        public void SetPara()
        {
            foreach (var recipe in _recipes)
            {
                string repeString = ConvertListIntToString(recipe.IngredientIds);
                recipe.Ingredients = repeString;
            }

        }
    }
}
