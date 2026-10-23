namespace SLunch.Livestream
{
	public interface ITrayLivestream
	{
		void Initialize();

		void CreateFoods();

		ITraySlot GetSlotByID(int id);

		bool IsCompleted();

		void RetrieveFoods();
	}
}
