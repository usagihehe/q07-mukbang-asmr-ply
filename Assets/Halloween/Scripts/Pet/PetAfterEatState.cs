using Pet;
using UnityEngine;
using Usaki;

namespace Pet
{
    public class PetAfterEatState : PetBaseState
    {
        private bool _isPlay;
        public override void EnterState()
        {
            _isPlay = true;
            //PlaySound
            PlayHappyAnimation();
        }

        public override string Execute(float dt)
        {
            if (_isPlay)
            {
                return StateMachine.AfterEatState;
            }
            else return StateMachine.IdleState;
        }

        public override void ExitState()
        {
        }

        public override bool IsSuitable()
        {
            return true;
        }

        private void PlayHappyAnimation()
        {
            _isPlay = true;
            _character.PlaySound(PetCtrl.HAPPY);
            PlayHappyAnimationTwice(PetCtrl.HAPPY);
        }

        private void PlayHappyAnimationTwice(string animName)
        {
            _character.PlayAnimByName(animName, false, delegate
            {
                _character.PlayAnimByName(animName, false, delegate { _isPlay = false; AudioManager.Instance.StopSoundEffect(); });
            });
        }
    }
}
