using Capyboba.Mukbang;
using Pet;
using UnityEngine;

namespace Pet
{
    public class PetEatState : PetBaseState
    {
        private bool _isPlay;

        public override void EnterState()
        {
            // Drinks never reach here: PetStateMachine does not hold them for the pet.
            IEdible item = _stateMachine.Held;
            if (item != null)
            {
                PlayEatAnimation();
                item.Consume();
                _stateMachine.Tracker.OnBitten(item);
            }
        }

        public override string Execute(float dt)
        {
            //OTHER FOOD
            if (_isPlay) return PetStateMachine.EatingState;
            AudioManager.Instance.StopSoundEffect();
            return PetStateMachine.AfterEatState;
        }

        public override void ExitState()
        {
        }

        public override bool IsSuitable()
        {
            return true;
        }

        private void PlayEatAnimation()
        {
            _isPlay = true;
            _character.PlaySound(PetCtrl.CHEW);
            PlayEatAnimationTwice(PetCtrl.CHEW);
        }

        private void PlayEatAnimationTwice(string animName)
        {
            _character.PlayAnimByName(animName, false, delegate
            {
                _character.PlayAnimByName(animName, false, delegate { _isPlay = false; });
            });
        }
    }
}
