using System.Collections.Generic;
using Spine.Unity;

namespace PLY33.Blindbox
{
    /// <summary>Ball skin lookup shared by the machine, basket and table so one food always shows the same ball.</summary>
    public static class BlindboxSkin
    {
        // The table shows the balls in draw order, so this reads left to right whatever the player opens first.
        private static readonly string[] SkinsByDrawOrder = { "1", "2", "3", "3", "1" };

        // Static so every ball view finds the skin without a shared reference; the machine clears it each new game.
        private static readonly Dictionary<SupermarketItemSO, string> SkinByItem = new Dictionary<SupermarketItemSO, string>();

        public static void Clear()
        {
            SkinByItem.Clear();
        }

        public static void Assign(SupermarketItemSO item, int drawIndex)
        {
            SkinByItem[item] = SkinsByDrawOrder[drawIndex % SkinsByDrawOrder.Length];
        }

        public static string ForItem(SupermarketItemSO item)
        {
            return SkinByItem.TryGetValue(item, out string skin) ? skin : SkinsByDrawOrder[0];
        }

        public static void Apply(SkeletonGraphic ball, SupermarketItemSO item)
        {
            ball.Skeleton.SetSkin(ForItem(item));
            ball.Skeleton.SetSlotsToSetupPose();
            ball.AnimationState.Apply(ball.Skeleton);
        }
    }
}
