using UnityEngine;

namespace MukbangAsmr.Ply31
{
    /// <summary>Single-screen playable: any tap ends the game and opens the store.</summary>
    public class Ply31PlayableFlow : MonoBehaviour
    {
        private bool _isGameEnded;

        private void Start()
        {
            Luna.Unity.LifeCycle.GameStarted();
            Luna.Unity.LifeCycle.GameLoaded();
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
