using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinCtrl : MonoBehaviour
{
    [Header("Skeleton")]
    [SerializeField] private SkeletonGraphic skeleton;
    private Skin characterSkin;

    private string baseHair;
    private string baseBody;
    private string baseHat;

    public void InitSkin()
    {
        //var hairList = ItemDecorManager.Instance.GetItemListByType(TypeItemDecor.hair);
        //var bodyList = ItemDecorManager.Instance.GetItemListByType(TypeItemDecor.body);
        //var hatList = ItemDecorManager.Instance.GetItemListByType(TypeItemDecor.hat);

        //if (hairList.Length == 0 || bodyList.Length == 0 || hatList.Length == 0)
        //{
        //    return;
        //}


        //baseHair = randomHair.name;
        //baseBody = randomBody.name;
        //baseHat = randomHat.name;

        ChangeSkin(null);
    }

    private void ChangeSkin(object data)
    {

        if (skeleton != null)
        {
            UpdateCharacterSkin();
            UpdateCombinedSkin();
        }
    }

    private void UpdateCharacterSkin()
    {
        SkeletonData skeletonData = skeleton.SkeletonData;
        characterSkin = new Skin("character-combined");
        // Note that the result Skin returned by calls to skeletonData.FindSkin()
        // could be cached once in Start() instead of searching for the same skin
        // every time. For demonstration purposes we keep it simple here.
        characterSkin.AddSkin(skeletonData.FindSkin(baseBody));
        characterSkin.AddSkin(skeletonData.FindSkin(baseHat));
        characterSkin.AddSkin(skeletonData.FindSkin(baseHair));
    }

    void AddEquipmentSkinsTo(Skin combinedSkin)
    {
        SkeletonData skeletonData = skeleton.SkeletonData; ;
        if (!string.IsNullOrEmpty(baseBody)) combinedSkin.AddSkin(skeletonData.FindSkin(baseBody));
        if (!string.IsNullOrEmpty(baseHat)) combinedSkin.AddSkin(skeletonData.FindSkin(baseHat));
        if (!string.IsNullOrEmpty(baseHair)) combinedSkin.AddSkin(skeletonData.FindSkin(baseHair));
    }

    void UpdateCombinedSkin()
    {
        Skin resultCombinedSkin = new Skin("character-combined");
        resultCombinedSkin.AddSkin(characterSkin);
        AddEquipmentSkinsTo(resultCombinedSkin);
        skeleton.Skeleton.SetSkin(resultCombinedSkin);
        skeleton.Skeleton.SetSlotsToSetupPose();

    }
}
