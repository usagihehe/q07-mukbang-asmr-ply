using DG.Tweening;
using Spine;
using UnityEngine;
using UnityEngine.UI;

namespace PLY32
{
    /// <summary>Phone selfie: flashes the screen and makes an idle character strike a random pose.</summary>
    [RequireComponent(typeof(Button))]
    public class PhoneFlashButton : MonoBehaviour
    {
        [SerializeField] private Image _flashImage;
        [SerializeField] private CharacterCtrl _character;
        [SerializeField] private EmotionType[] _poses = { EmotionType.Happy, EmotionType.Licklips, EmotionType.Shock };
        [SerializeField] private float _flashInTime = 0.05f;
        [SerializeField] private float _flashOutTime = 0.35f;

        private Button _button;
        private int _lastPoseIndex = -1;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(TakePhoto);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(TakePhoto);
        }

        private void TakePhoto()
        {
            PlayFlash();
            if (IsCharacterIdle()) PlayRandomPose();
        }

        private void PlayFlash()
        {
            _flashImage.DOKill();
            _flashImage.gameObject.SetActive(true);
            Color transparent = _flashImage.color;
            transparent.a = 0f;
            _flashImage.color = transparent;
            DOTween.Sequence()
                .Append(_flashImage.DOFade(1f, _flashInTime))
                .Append(_flashImage.DOFade(0f, _flashOutTime))
                .OnComplete(() => _flashImage.gameObject.SetActive(false))
                .SetTarget(_flashImage)
                .SetLink(_flashImage.gameObject);
        }

        // Posing only from Idle keeps the eat/before-eat animations owned by the state machine.
        private bool IsCharacterIdle()
        {
            TrackEntry current = _character.Skeleton.AnimationState.GetCurrent(0);
            return current != null && current.Animation.Name == CharacterCtrl.IDLE;
        }

        private void PlayRandomPose()
        {
            string poseAnim = _character.GetAnimEmotion(_poses[PickPoseIndex()]);
            TrackEntry pose = _character.PlayAnimByName(poseAnim, false);
            pose.Complete += delegate
            {
                // The state machine may have taken over (e.g. food picked up) while the pose played.
                if (_character.Skeleton.AnimationState.GetCurrent(0) == pose)
                    _character.PlayAnimByName(CharacterCtrl.IDLE, true);
            };
        }

        private int PickPoseIndex()
        {
            int index;
            do
            {
                index = Random.Range(0, _poses.Length);
            } while (index == _lastPoseIndex && _poses.Length > 1);
            _lastPoseIndex = index;
            return index;
        }
    }
}
