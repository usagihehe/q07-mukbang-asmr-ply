using System.Collections;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace SLunch.Character
{
    public class PhoneCtrl : MonoBehaviour
    {
        [SerializeField] private SkeletonGraphic _skeleton;

        [SpineAnimation(dataField = "_skeleton")]
        [SerializeField] private string _idleAnimation = "Idle";

        [SpineAnimation(dataField = "_skeleton")]
        [SerializeField] private string[] _randomAnimations;

        [SerializeField] private float _interval = 5f;

        private void OnEnable()
        {
            _skeleton.AnimationState.SetAnimation(0, _idleAnimation, true);
            StartCoroutine(PlayRandomLoop());
        }

        private void OnDisable() => StopAllCoroutines();

        private IEnumerator PlayRandomLoop()
        {
            WaitForSeconds wait = new WaitForSeconds(_interval);
            while (true)
            {
                yield return wait;

                TrackEntry entry = PlayRandomAnimation();
                if (entry != null)
                    yield return new WaitForSpineAnimationComplete(entry);
            }
        }

        private TrackEntry PlayRandomAnimation()
        {
            if (_randomAnimations == null || _randomAnimations.Length == 0) return null;

            string animName = _randomAnimations[Random.Range(0, _randomAnimations.Length)];
            TrackEntry entry = _skeleton.AnimationState.SetAnimation(0, animName, false);
            _skeleton.AnimationState.AddAnimation(0, _idleAnimation, true, 0f);
            return entry;
        }
    }
}
