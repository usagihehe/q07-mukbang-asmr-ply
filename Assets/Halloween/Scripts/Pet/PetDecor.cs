using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace Pet
{
    public class PetDecor : MonoBehaviour
    {
        [SerializeField] private GameObject petObj;
        [SerializeField] private SkeletonGraphic skeleton;
        [SerializeField] private PetCtrl petCtrl;
        [SerializeField] private RectTransform[] skinObjects;
        // The playable has no skin collection save: the pet shown is fixed per prefab/scene.
        [SerializeField] private int _petId;
        [SerializeField] private string _petSkinName = "1";
        private Dictionary<int, RectTransform> _skinDict;
        private Skin petSkin;

        private void Start()
        {
            _skinDict = new Dictionary<int, RectTransform>();
            for (int i = 0; i < skinObjects.Length; i++)
            {
                if (skinObjects[i] != null)
                    _skinDict[i] = skinObjects[i];
            }

            if (petObj != null) petObj.SetActive(_petId != -1);
            if (_petId != -1) ChangeSkin();
        }

        private void ChangeSkin()
        {
            foreach (var kvp in _skinDict)
            {
                kvp.Value.gameObject.SetActive(kvp.Key == _petId);
            }

            if (petCtrl != null && _skinDict.TryGetValue(_petId, out RectTransform activeObj))
                petCtrl.mPoint = activeObj;

            if (skeleton != null)
            {
                petSkin = new Skin("petskin");
                if (skeleton.SkeletonData.FindSkin(_petSkinName) != null) petSkin.AddSkin(skeleton.SkeletonData.FindSkin(_petSkinName));
                skeleton.Skeleton.SetSkin(petSkin);
                skeleton.Skeleton.SetSlotsToSetupPose();
            }
        }
    }
}
