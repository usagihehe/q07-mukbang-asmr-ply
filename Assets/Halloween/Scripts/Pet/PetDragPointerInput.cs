using UnityEngine;
using UnityEngine.EventSystems;

namespace Pet
{
    /// <summary>
    /// Cùng pattern <see cref="ItemMukbang"/>: EventSystem + RectTransform (qua <see cref="PetDragCtrl"/>).
    /// Gắn trên GameObject có <see cref="UnityEngine.UI.Graphic"/> raycast (cùng pet hoặc child vùng chạm).
    /// </summary>
    [DisallowMultipleComponent]
    public class PetDragPointerInput : MonoBehaviour, IDragHandler, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
    {
        private PetDragCtrl _host;

        private void Awake()
        {
            _host = GetComponent<PetDragCtrl>();
            if (_host == null)
                _host = GetComponentInParent<PetDragCtrl>();
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            _host?.HandlePointerDown(eventData);
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            _host?.HandleDrag(eventData);
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            _host?.HandlePointerUp(eventData);
        }
    }
}
