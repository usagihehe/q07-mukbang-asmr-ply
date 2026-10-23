using SLunch.Tray;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SLunch
{
    [CreateAssetMenu(fileName = "CanteenDataConfig", menuName = "SLunch/CanteenDataConfig")]
    public class CanteenDataConfig : ScriptableObject
    {
        [SerializeField] List<BowlFoodData> _bowlFoodDatas;
        public List<BowlFoodData> BowlFoodDatas => _bowlFoodDatas;

        private Dictionary<int, BowlFoodData> _dictBowls;

        private void OnEnable()
        {
            if (_bowlFoodDatas == null || _bowlFoodDatas.Count <= 1) return;
            _bowlFoodDatas.Sort((a, b) => a.Id.CompareTo(b.Id));
            _dictBowls = new Dictionary<int, BowlFoodData>();
            foreach (var data in _bowlFoodDatas)
            {
                //Debug.Log($"{data.FoodName} - {data.Id}");
                _dictBowls[data.Id] = data;
            }
        }

        public void SetCanteenDataConfigCollection(List<BowlFoodData> items)
        {
            _bowlFoodDatas = new List<BowlFoodData>();
            _bowlFoodDatas = items;
        }
    }
}
