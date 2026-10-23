using UnityEngine;
using UnityEngine.UI;

namespace SLunch
{
    public class BowlFood : MonoBehaviour, IBowlFood
    {
        [SerializeField] private Image _foodImage;
        [SerializeField] private GameObject _ads;
        private BowlFoodData _data;

        public void SetData(BowlFoodData data)
        {
            _data = data;
            ShowFull();
        }

        public void ShowFull()
        {
            _foodImage.sprite = _data.FullSprite;
            _foodImage.SetNativeSize();
        }

        public void ShowUsed()
        {
            _foodImage.sprite = _data.UsedSprite;
            _foodImage.SetNativeSize();
        }

        public BowlFoodData GetData()
        {
            return _data;
        }

        public Vector3 GetLocalScale()
        {
            return transform.localScale;
        }
    }
}
