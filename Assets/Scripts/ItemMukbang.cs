using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemMukbang : MonoBehaviour, IDragHandler, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
{
    public static bool BlockPick;

    public Action<ItemMukbang> onDone;
    public Action<ItemMukbang> onConsume;

    [SerializeField]
    protected Image _icon;

    [SerializeField]
    protected Image _tool;

    [SerializeField]
    protected float _heightOffset;

    [SerializeField]
    protected float _scaleTime;

    protected int _curStep;

    protected bool _isDone;

    public Image Icon => _icon;

    public Image Tool => _tool;

    public Vector2 Offset => new Vector2(0, _heightOffset);

    public int CurStep => _curStep;

    public virtual bool IsDone
    {
        get
        {
            return _isDone;
        }
        protected set
        {
            _isDone = value;
        }
    }

    /// <summary>
    /// Gets the emotion type associated with this item.
    /// </summary>
    /// <returns>EmotionType of the item.</returns>
    public virtual EmotionType GetEmotion()
    {
        return EmotionType.None;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (!CanPick()) return;

        RectTransform parentRect = _tool.rectTransform.parent as RectTransform;

        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect, eventData.position, eventData.pressEventCamera, out localPoint))
        {
            _tool.rectTransform.anchoredPosition = localPoint + Offset;
        }
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (!CanDrag()) return;
        RectTransform parentRect = _tool.rectTransform.parent as RectTransform;

        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect, eventData.position, eventData.pressEventCamera, out localPoint))
        {
            _tool.rectTransform.anchoredPosition = localPoint + Offset;
        }
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {

    }

    public virtual void ShowTool()
    {
        if (_tool != null)
        {
            _tool.gameObject.SetActive(true);
        }
        _tool.transform.DOScale(Vector3.one, _scaleTime).From(Vector3.zero);
    }

    public virtual void HideTool()
    {
        if (_tool != null)
        {
            Tool.rectTransform.anchoredPosition = Icon.rectTransform.anchoredPosition;
            _tool.gameObject.SetActive(false);
        }
    }

    public virtual void Consume()
    {
    }

    protected virtual bool CanPick()
    {
        return !BlockPick && !_isDone;
    }

    protected virtual bool CanDrag()
    {
        return !BlockPick && !_isDone && _tool.gameObject.activeSelf;
    }
}
