using UnityEngine;
using UnityEngine.UI;

namespace SLunch.Tray
{
	public class FoodSlot : MonoBehaviour
	{
		[SerializeField]	private Image _foodImage;

		public void SetFoodSprite(Sprite sprite)
		{
			_foodImage.sprite = sprite;
			_foodImage.SetNativeSize();
        }

		public void SetPosition(Vector3 position)
		{
            _foodImage.rectTransform.anchoredPosition = position;
		}

		public void SetActiveVisual(bool isActive)
		{
            _foodImage.gameObject.SetActive(isActive);

        }
	}
}
