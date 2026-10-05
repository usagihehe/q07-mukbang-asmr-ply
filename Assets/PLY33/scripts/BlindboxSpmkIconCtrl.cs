using Spine.Unity;
using UnityEngine;

namespace PLY33.Blindbox
{
    /// <summary>Basket slot that shows the ball instead of the food icon, so the food stays a surprise.</summary>
    public class BlindboxSpmkIconCtrl : ItemSpmkIconCtrl
    {
        private const string IdleAnim = "idle";

        [SerializeField] private SkeletonGraphic _ballModel;

        public override bool ActiveIcon(SupermarketItemSO itemSo)
        {
            if (!base.ActiveIcon(itemSo)) return false;
            _IconImg.gameObject.SetActive(false);
            _ballModel.gameObject.SetActive(true);
            BlindboxSkin.Apply(_ballModel, itemSo);
            _ballModel.AnimationState.SetAnimation(0, IdleAnim, true);
            return true;
        }
    }
}
