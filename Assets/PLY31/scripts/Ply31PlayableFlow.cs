using UnityEngine;
using Usaki;

namespace PLY31
{
    /// <summary>Single-screen playable: any tap ends the game and opens the store.</summary>
    public class Ply31PlayableFlow : MonoBehaviour
    {
        [SerializeField] private HandHint _handHint;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Transform[] _modeImages;

        private bool _isGameEnded;

        private void Start()
        {
            Luna.Unity.LifeCycle.GameStarted();
            Luna.Unity.LifeCycle.GameLoaded();
            _handHint.PointSequentially(_modeImages, _canvas);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OpenStore();
            }
        }

        private void OpenStore()
        {
            if (!_isGameEnded)
            {
                _isGameEnded = true;
                Luna.Unity.LifeCycle.GameEnded();
            }

            Luna.Unity.Playable.InstallFullGame();
        }
    }
}
