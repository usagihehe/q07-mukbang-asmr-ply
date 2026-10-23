using System;
using System.Collections.Generic;
using UnityEngine;

namespace SLunch
{
    [Serializable]
    public class RecipeData
    {
        public int ResultId;
        public string ResultName;
        public List<int> IngredientIds;
        //[HideInInspector]
        public string Ingredients;
    }
}
