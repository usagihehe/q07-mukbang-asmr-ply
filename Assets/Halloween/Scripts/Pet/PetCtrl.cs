using Spine;
using Spine.Unity;
using UnityEngine;

namespace Pet
{
    public class PetCtrl : MonoBehaviour
    {
        [SerializeField]
        private SkeletonGraphic _skeleton;
        [SerializeField]
        private RectTransform _mPoint;
        [SerializeField]
        private CharacterSoundSO _petSound;
        public SkeletonGraphic Skeleton => _skeleton;
        public RectTransform mPoint { get => _mPoint; set => _mPoint = value; }

        public TrackEntry PlayAnimByName(string animName, bool loop, Spine.AnimationState.TrackEntryDelegate callbackOnComplete = null)
        {
            TrackEntry trackfalse = _skeleton.AnimationState.SetAnimation(0, animName, loop);
            trackfalse.Complete += callbackOnComplete;
            return trackfalse;
        }

        public void PlaySound(string state)
        {
            AudioClip audio = null;
            switch (state)
            {
                case IDLE:

                    break;
                case CHEW:
                    audio = _petSound.GetEatSound(SoundType.Eat);
                    AudioManager.Instance.PlaySoundEffect(audio);
                    break;
                case TOUCH:
                    audio = _petSound.GetEmotionSound(EmotionType.Cute);
                    AudioManager.Instance.PlaySoundEffect(audio);
                    break;
                case HAPPY:
                    audio = _petSound.GetEmotionSound(EmotionType.Happy);
                    AudioManager.Instance.PlaySoundEffect(audio);
                    break;
            }
        }

        private void OnEnable()
        {
            PlayAnimByName(IDLE, true, null);
        }

        public const string IDLE = "idle";
        public const string CHEW = "chew";
        public const string TOUCH = "touch";
        public const string HAPPY = "happy";
    }
}
