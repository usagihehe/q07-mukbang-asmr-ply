using UnityEngine;

namespace Usaki
{
    public class SelfieState : CharBaseState
    {

        [SerializeField]
        private string[] _animList;

        private bool _onDone;

        private int _lastIndex;

        private bool _isClick;

        protected override void Awake()
        {
        }

        public override void EnterState()
        {
        }

        public override string Execute(float dt)
        {
            return null;
        }

        public override void ExitState()
        {
        }

        public override bool IsSuitable()
        {
            return true;
        }

        private string GetRandAnim()
        {
            return null;
        }
    }
}
