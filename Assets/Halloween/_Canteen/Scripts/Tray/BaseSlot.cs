using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SLunch.Tray
{
    public abstract class BaseSlot : MonoBehaviour, ITraySlot
    {
        [SerializeField]
        protected int _id;

        [Header("VISUALS")]
        [SerializeField]
        protected GameObject normalHighlight;

        [SerializeField]
        protected GameObject mixedHighlight;

        [SerializeField]
        protected ParticleSystem blingEffect;

        [SerializeField]
        protected ParticleSystem fullEffect;

        [Header("DATA")]
        [SerializeField] protected List<int> foodItemIDList;
        [SerializeField] protected FoodSlot foodSlot;

        protected IRecipeController _recipeController;

        //DEBUG
        [SerializeField] protected int _currentResultFoodID;

        //DEBUG
        [SerializeField] protected int _currentPhase;

        public int ID => _id;

        public List<int> FoodItemIDList => foodItemIDList;

        public int CurrentResultFoodID => _currentResultFoodID;

        public int CurrentPhase => _currentPhase;

        public FoodSlot FoodSlot => foodSlot;



        private void Awake()
        {
            _recipeController = FindObjectOfType<RecipeController>();
        }

        public virtual void Initialize(int id)
        {
            _id = id;
        }

        public virtual bool TryAddFood(int foodItemID)
        {
            if (IsFull()) return false;

            if (IsEmpty())
            {
                AddFirstFood(foodItemID);
                ActiveEffectFull();
                return true;
            }

            if (_currentResultFoodID == foodItemID)
            {
                _currentResultFoodID = foodItemID;
                _currentPhase++;
                AddNextPhase(foodItemID);
                ActiveEffectFull();
                return true;
            }

            return false;
        }

        public bool CanMixFood(int foodItemID)
        {
            return GetMixRecipe(foodItemID) != null;
        }

        public virtual bool TryMixFood(int foodItemID)
        {
            RecipeData exactRecipe = GetMixRecipe(foodItemID);
            if (exactRecipe == null) return false;

            foodItemIDList.Add(foodItemID);
            _currentResultFoodID = exactRecipe.ResultId;
            _currentPhase = GetMaxPhase(_currentResultFoodID);
            ChangeCurrentPhase(_currentResultFoodID);
            SetActiveMixedHighlight(false);
            return true;
        }

        private RecipeData GetMixRecipe(int foodItemID)
        {
            if (IsEmpty()) return null;

            List<int> tempFoodList = new List<int>()
            {
                _currentResultFoodID,
                foodItemID,
            };
            return _recipeController.GetExactMatchRecipe(tempFoodList);
        }

        protected virtual void AddFirstFood(int foodItemID)
        {
            foodItemIDList.Add(foodItemID);

            _currentResultFoodID = foodItemID;
            _currentPhase = GetMaxPhase(foodItemID);

            ChangeCurrentPhase(foodItemID);
        }

        protected virtual void AddNextPhase(int foodItemID)
        {
            foodItemIDList.Add(foodItemID);
            ChangeCurrentPhase(foodItemID);
        }

        protected abstract bool IsMaxPhase(int foodItemID);

        protected abstract int GetMaxPhase(int foodItemID);

        protected abstract void ChangeCurrentPhase(int foodItemID);

        public virtual bool IsEmpty()
        {
            return foodItemIDList == null || foodItemIDList.Count == 0;
        }

        public virtual bool IsFull()
        {
            if (IsEmpty()) return false;
            return IsMaxPhase(CurrentResultFoodID);
        }

        public void ClearSlot()
        {
            // Reset DATA
            foodItemIDList = new List<int>();

            _currentResultFoodID = -1;
            _currentPhase = 0;

            foodSlot.SetActiveVisual(false);
            // Reset VISUAL
            normalHighlight.gameObject.SetActive(false);
            mixedHighlight.gameObject.SetActive(false);
        }

        public virtual void SetActiveNormalHighlight(bool isActive)
        {
            normalHighlight.SetActive(isActive);
        }

        public virtual void SetActiveMixedHighlight(bool isActive)
        {
            mixedHighlight.SetActive(isActive);
        }

        public virtual void ActiveEffectBling()
        {
            if (blingEffect != null) blingEffect.Play();
        }

        public virtual void ActiveEffectFull()
        {
            if (fullEffect != null) fullEffect.Play();
        }
    }
}
