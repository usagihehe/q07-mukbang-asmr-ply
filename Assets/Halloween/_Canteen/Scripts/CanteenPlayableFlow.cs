using UnityEngine;

namespace SLunch
{
    /// <summary>Playable entry: opens straight on the canteen gameplay, skipping the door and intro.</summary>
    public class CanteenPlayableFlow : MonoBehaviour
    {
        // The panel's AlphaTweenAnim starts it at alpha 0; ShowIntro fades it in.
        [SerializeField] private CanteenPanel _canteenPanel;

        private void Start()
        {
            _canteenPanel.ShowIntro(null);
            CanteenManager.Instance.ShowCanteenGameplay();
        }
    }
}
