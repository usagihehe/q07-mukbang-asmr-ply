using SLunch.Canteen;
using SLunch.Livestream;
using SLunch.Tray;
using SLunch.Water;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Usaki;
using Utils.Singletons;
namespace SLunch
{
    public class CanteenManager : ZMonoSingleton<CanteenManager>
    {
        [Header("Visual")]
        [SerializeField] private CanteenPanel canteenPanel;
        [SerializeField] private CanteenGameplay canteenGameplay;
        [SerializeField] private CanteenWaterChoose canteenWaterChoose;
        [SerializeField] private CanteenLivePanel canteenLivePanel;

        [Header("Control")]
        [SerializeField] private FoodController _foodController;
        [SerializeField] private TrayController _trayController;
        [SerializeField] private BowlFood[] _bowlFoods;
        [SerializeField] private WaterBasketCtrl _waterBasket;

        [Header("Config")]
        [SerializeField] private CanteenDataConfig _dataConfig;
        [SerializeField] private FoodItemSO _foodItemSO;
        [SerializeField] private FoodItemSO _drinkItemSOs;
        [SerializeField] private RecipeDataConfig _recipeData;


        private Dictionary<int, TrayFoodItemDTO> dictsFoodTrayItem;
        private FoodItemData _drinkItemBasket;
        public FoodController FoodController => _foodController;
        public TrayController TrayController => _trayController;
        public BowlFood[] BowlFoods => _bowlFoods;
        public CanteenDataConfig DataConfig => _dataConfig;
        public FoodItemSO FoodItemSO => _foodItemSO;
        public FoodItemSO DrinkItemSOs => _drinkItemSOs;
        public RecipeDataConfig RecipeData => _recipeData;
        public CanteenGameplay CanteenGameplay => canteenGameplay;

        [Serializable]
        public class TrayFoodItemDTO
        {
            public FoodItemData data;
            public int phase;
        }

        private void OnEnable()
        {
            canteenGameplay.SetActive(false);
            canteenWaterChoose.SetActive(false);

            Initialize();
            //DataManager.Instance.CheatAds();
        }

        private void Initialize()
        {
            for (int i = 0; i < _dataConfig.BowlFoodDatas.Count; i++)
            {
                int index = i;
                BowlFoodData data = _dataConfig.BowlFoodDatas[index];
                BowlFood food = _bowlFoods[index];
                if (food != null && data != null) food.SetData(data);
                food.gameObject.name = $"{data.FoodName} Box";
            }


        }

        public void ShowCanteenGameplay()
        {
            Initialize();

            canteenGameplay.SetActive(true);
            canteenWaterChoose.SetActive(false);
        }

        public void ShowCateenChooseWater()
        {
            canteenGameplay.SetActive(false);
            canteenWaterChoose.ShowCanteenWaterChoose(GetFoodItemDatasPick(_trayController));
        }

        public void ShowCanteenLivePanel()
        {
            canteenPanel.SetActive(false);
            canteenGameplay.SetActive(false);
            canteenWaterChoose.SetActive(false);
            if (_waterBasket.Items != null)
            {
                _drinkItemBasket = _waterBasket.Items[0];
                canteenLivePanel.ShowPanel(GetFoodItemDatasPick(_trayController), _drinkItemBasket);
            }
            else
            {
                canteenLivePanel.ShowPanel(GetFoodItemDatasPick(_trayController), _drinkItemSOs.FoodItems[0]);
            }
        }

        public Dictionary<int, TrayFoodItemDTO> GetFoodItemDatasPick(TrayController trayController)
        {
            dictsFoodTrayItem = new Dictionary<int, TrayFoodItemDTO>();
            foreach (BaseSlot slot in trayController.BaseSlots)
            {
                FoodItemData foodItemData = _foodItemSO.GetFoodItemByID(slot.CurrentResultFoodID);
                if (foodItemData != null)
                {
                    TrayFoodItemDTO dto = new TrayFoodItemDTO();
                    dto.data = foodItemData;
                    dto.phase = slot.CurrentPhase;
                    dictsFoodTrayItem[slot.ID] = dto;
                }
            }
            dtos = dictsFoodTrayItem.Values.ToList();
            id = dictsFoodTrayItem.Keys.ToList();
            return dictsFoodTrayItem;
        }


        public List<TrayFoodItemDTO> dtos;
        public List<int> id;
    }
}
