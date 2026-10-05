using Spine.Unity;

namespace PLY33.Blindbox
{
    /// <summary>Ball skin lookup shared by the machine, basket and table so one food always shows the same ball.</summary>
    public static class BlindboxSkin
    {
        private static readonly string[] SkinNames = { "1", "2", "3" };

        public static string ForItem(SupermarketItemSO item)
        {
            int nameSum = 0;
            foreach (char letter in item.name) nameSum += letter;
            return SkinNames[nameSum % SkinNames.Length];
        }

        public static void Apply(SkeletonGraphic ball, SupermarketItemSO item)
        {
            ball.Skeleton.SetSkin(ForItem(item));
            ball.Skeleton.SetSlotsToSetupPose();
            ball.AnimationState.Apply(ball.Skeleton);
        }
    }
}
