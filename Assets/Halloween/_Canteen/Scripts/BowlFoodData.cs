using System;
using UnityEngine;

namespace SLunch
{
    [CreateAssetMenu(fileName = "BowlFoodData", menuName = "SLunch/BowlFoodData")]
    public class BowlFoodData: ScriptableObject
	{
		public int Id;
		public string FoodName;
		public Sprite PieceSprite;
		public Sprite FullSprite;
		public Sprite UsedSprite;
		public Sprite LockedSprite;

		public AudioClip soundPick;
		public AudioClip soundDrop;
    }
}
