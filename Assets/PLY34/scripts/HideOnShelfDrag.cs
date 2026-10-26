using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PLY34.UI
{
    /// <summary>Hides the given objects (e.g. the swipe tutorial arrow) for good once the player drags the shelf.</summary>
    /// <remarks>Sits on the ScrollRect object: onValueChanged also fires on layout settle in Luna, hiding the arrow before any swipe.</remarks>
    public class HideOnShelfDrag : MonoBehaviour, IBeginDragHandler
    {
        [SerializeField] private List<GameObject> _targets;

        public void OnBeginDrag(PointerEventData eventData)
        {
            foreach (var target in _targets) target.SetActive(false);
        }
    }
}
