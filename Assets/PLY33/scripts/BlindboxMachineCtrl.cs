using DG.Tweening;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace PLY33.Blindbox
{
    /// <summary>Idle hint: while nobody presses the catcher, the machine plays "act" every interval to invite a tap.</summary>
    public class BlindboxMachineCtrl : ShelfCtrl
    {
        [SerializeField]
        private SkeletonGraphic _model;

        [SerializeField]
        private Button _playBtn;

        [SerializeField]
        private float _actInterval = 3f;

        private void Awake()
        {
            _playBtn.onClick.AddListener(OnPressPlayBtn);
        }

        private void OnEnable()
        {
            DOTween.Sequence()
                .AppendInterval(_actInterval)
                .AppendCallback(PlayGame)
                .SetLoops(-1)
                .SetId(this)
                .SetLink(gameObject, LinkBehaviour.KillOnDisable);
        }

        private void OnPressPlayBtn()
        {
            DOTween.Restart(this);
        }

        // A disabled button means a round owns the model; a queued animation means a one-shot (gachagacha) is still playing.
        private void PlayGame()
        {
            if (!_playBtn.interactable || _model.AnimationState.GetCurrent(0)?.Next != null) return;
            _model.AnimationState.SetAnimation(0, "act", false);
            _model.AnimationState.AddAnimation(0, "idle", true, 0f);
        }
    }
}
