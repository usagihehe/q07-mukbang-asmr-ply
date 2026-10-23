namespace SLunch.Livestream
{
	public interface ITraySlot
	{
		int ID { get; }

		int FoodID { get; }

		void Initialize(int id, ITrayLivestream tray);

		void AddFood(int foodID, int currentPhase);

		void ChangePhase(bool isIncrease = false);

		bool IsEatCompleted();

		void RetrieveFood();
	}
}
