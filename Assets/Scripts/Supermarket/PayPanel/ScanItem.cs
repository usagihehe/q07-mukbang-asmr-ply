using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScanItem : DragDropItem
{
    [SerializeField]
    private SupermarketItemSO _itemSO;
    [SerializeField]
    private ItemSpmkIconCtrl _iconCtrl;
    public bool IsScan;
    public bool BlockControl;
    private Vector3 _origionPos;
    private Vector3 _originRotate;
    public SupermarketItemSO SO => _itemSO;


    private void OnEnable()
    {
        IsScan = false;
        BlockControl = false;
        _origionPos = transform.position;
        _originRotate = transform.eulerAngles;
    }

    private void OnDisable()
    {
        transform.position = _origionPos;
        transform.eulerAngles = _originRotate;
    }

    public bool Init(SupermarketItemSO item)
    {
        if (item == null) { return false; }
        _itemSO = item;
        _iconCtrl.ActiveIcon(item);
        return true;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!BlockControl)
        {
            base.OnPointerDown(eventData);
            Observer.Instance.NotifyWithData(ObserverTopic.OnSelectItem, this);
            transform.eulerAngles = Vector3.zero;
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (!BlockControl)
        {
            base.OnDrag(eventData);
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (!BlockControl)
        {
            base.OnPointerUp(eventData);
            Observer.Instance.NotifyWithData(ObserverTopic.OnDropItem, this);
            transform.position = _origionPos;
            transform.eulerAngles = _originRotate;
        }
    }

    private void OnValidate()
    {
        _iconCtrl = transform.GetChild(0).GetComponent<ItemSpmkIconCtrl>();
        _offset = new Vector2(0, 150f);

    }
}
