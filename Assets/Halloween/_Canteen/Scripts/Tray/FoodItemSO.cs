using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SLunch.Tray
{
    [CreateAssetMenu(fileName = "TrayFoodItemSO", menuName = "SLunch/TrayFoodItemSO")]
    public class FoodItemSO : ScriptableObject
    {
        [SerializeField] private List<FoodItemData> _foodItems;
        [SerializeField] private CanteenDataConfig _canteenDataConfig;
        [SerializeField] private RecipeDataConfig _recipeDataConfig;
        public List<FoodItemData> FoodItems => _foodItems;

        public FoodItemData GetFoodItemByID(int id)
        {
            foreach (FoodItemData item in _foodItems)
            {
                if (item.ID == id)
                    return item;
            }
            return null;
        }

        private void OnEnable()
        {
            //if (_canteenDataConfig == null || _recipeDataConfig == null)
            //{
            //    foreach (FoodItemData item in _foodItems)
            //    {
            //        Debug.Log($"FoundID: {item.ID} with {item.FoodName} - Level Unlock {item.LevelUnlock}");
            //    }
            //    return;
            //}

            //foreach (FoodItemData item in _foodItems)
            //{
            //    BowlFoodData bowlFoodData = _canteenDataConfig.BowlFoodDatas.FirstOrDefault(x => x.FoodName == item.FoodName);
            //    if (bowlFoodData != null)
            //    {
            //        item.ID = bowlFoodData.Id;
            //        Debug.Log($"FoundID Bowl Food: {item.ID} with {item.FoodName} - Level Unlock {item.LevelUnlock}");
            //    }
            //    else
            //    {
            //        RecipeData recipeData = _recipeDataConfig.GetRecipeByID(item.ID);
            //        if (recipeData != null)
            //        {
            //            item.ID = recipeData.ResultId;
            //            Debug.Log($"FoundID: {item.ID} with {item.FoodName} - Level Unlock {item.LevelUnlock}");
            //        }
            //        else
            //        {
            //            Debug.Log($"Not foundID: {item.ID} with {item.FoodName} - Level Unlock {item.LevelUnlock}");
            //        }
            //    }
            //    item.EatSound = SoundType.Eat;
            //    item.Emotion = EmotionType.Happy;
            //}
            //_foodItems.Sort((a, b) => a.ID.CompareTo(b.ID));
        }

        public void SetFoodItemDataCollection(List<FoodItemData> items)
        {
            _foodItems = new List<FoodItemData>();
            _foodItems = items;
        }

        public void AddFoodItemDataCollection(List<FoodItemData> items)
        {
            if (_foodItems != null)
            {
                _foodItems.AddRange(items);
            }
        }
    }
}
