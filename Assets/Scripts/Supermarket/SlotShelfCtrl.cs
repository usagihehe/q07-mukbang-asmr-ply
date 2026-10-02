using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Usaki;

public class SlotShelfCtrl : MonoBehaviour
{
    [SerializeField] protected List<Image> _viewItems;
    [SerializeField] protected TextMeshProUGUI _priceTxt;
    [SerializeField] protected SupermarketItemSO _itemSO;
    protected bool _canPick;
    public SupermarketItemSO Item => _itemSO;
    public virtual bool CanPick
    {
        get { return _canPick; }
        set
        {
            _canPick = value;
        }
    }

    protected virtual void Awake()
    {
        Init(_itemSO);
        GetComponent<Button>().onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayAudioClick();
            Observer.Instance.NotifyWithData(ObserverTopic.OnPickItem, this);
        });
    }

    //public virtual void OnValidate()
    //{
    //    _viewItems = transform.GetChild(0).GetComponentsInChildren<Image>().ToList();
    //    if (_viewItems.Count > 0)
    //    {
    //        foreach (var item in _viewItems)
    //        {
    //            item.sprite = _itemSO.GetIcon();
    //            //item.SetNativeSize();
    //            item.transform.localScale = Vector3.one * _itemSO.OffsetScaleOnTable;
    //        }
    //    }

    //    if (transform.childCount <= 1) return;
    //    else if (transform.GetChild(1).childCount <= 0) return;
    //    else
    //    {
    //        _priceTxt = transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
    //    }
    //    if (_priceTxt != null) _priceTxt.text = "" + _itemSO.Price;
    //    if (_itemSO != null) gameObject.name = _itemSO.name;

    //}

    public virtual bool Init(SupermarketItemSO itemSO)
    {
        if (itemSO == null)
        {
            _canPick = false;
            return _canPick;
        }
        _itemSO = itemSO;
        foreach (var item in _viewItems)
        {
            item.sprite = itemSO.GetIcon();
            item.SetNativeSize();
        }
        if (_priceTxt != null) _priceTxt.text = "" + itemSO.Price;
        _canPick = true;
        return _canPick;
    }

    public virtual GameObject GetFirstView()
    {
        return _viewItems[_viewItems.Count - 1].gameObject;
    }
}
