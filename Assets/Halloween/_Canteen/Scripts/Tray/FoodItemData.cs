using System;
using System.Collections.Generic;
using UnityEngine;

namespace SLunch.Tray
{
    [CreateAssetMenu(fileName = "FoodItemData", menuName = "SLunch/FoodItemData")]
    public class FoodItemData : ScriptableObject
    {
        public int ID;
        public string FoodName;
        public Sprite ToolFull;
        public Sprite EndSmallPhase;
        public Sprite EndBigPhase;
        public List<Sprite> SmallPhases;
        public List<Sprite> BigPhases;
        public List<Vector2> SmallPhasesPos;
        public List<Vector2> BigPhasesPos;
        public EmotionType Emotion;
        public SoundType EatSound;

        public int LevelUnlock;
        public int UnlockValue;

        public int BonusValue;

        public Sprite GetIconDrink()
        {
            return BigPhases[BigPhases.Count - 1];
        }

    }
}
