using System.Collections.Generic;

namespace SLunch.Tray
{
	public interface ITraySlot
	{
		int ID { get; }

		List<int> FoodItemIDList { get; }

		void Initialize(int id);

		bool TryAddFood(int foodItemID);

		bool IsEmpty();

		bool IsFull();

		void ClearSlot();
	}
}
