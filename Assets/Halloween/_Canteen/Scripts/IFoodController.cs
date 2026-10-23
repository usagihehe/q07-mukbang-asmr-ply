using System;
using UnityEngine.EventSystems;

namespace SLunch.Canteen
{
	public interface IFoodController
	{
		void StartDragFood(IBowlFood food, PointerEventData evsentData);

		void OnDragFood(PointerEventData eventData);

        void StopDragFood();

		void Register(Action onCompleted);

		void Unregister();
	}
}
