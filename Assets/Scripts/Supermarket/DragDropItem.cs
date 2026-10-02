using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropItem : MonoBehaviour, IDragHandler, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    protected Vector2 _offset;
    public Vector2 Offset => _offset;

    public virtual void OnDrag(PointerEventData eventData)
    {
        // Get the parent RectTransform
        RectTransform parentRect = transform.parent as RectTransform;

        // Convert the screen point to local point in the parent RectTransform
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint);

        // Update the anchored position of the RectTransform (add the fixed offset)
        (transform as RectTransform).anchoredPosition = localPoint + _offset;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        RectTransform parentRect = transform.parent as RectTransform;

        // Convert the screen point to local point in the parent RectTransform
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint);

        // Update the anchored position of the RectTransform (add the fixed offset)
        (transform as RectTransform).anchoredPosition = localPoint + _offset;
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
    }
}
