using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Supermarket;
using UnityEngine;
using UnityEngine.UI;
using Usaki;

public class LuckyDessertCtrl : MonoBehaviour
{
    [SerializeField]
    private LuckyControlTween _control;
    [SerializeField]
    private Button _playBtn;
    [SerializeField]
    private Transform _attachmentPoint;
    [SerializeField]
    private SlotShelfCtrl _item;
    [SerializeField]
    private List<SlotShelfCtrl> _items;
    [SerializeField]
    private float _from;
    [SerializeField]
    private float _to;
    [SerializeField]
    private float _time;
    [SerializeField]
    private BasketCtrl _basketCtrl;

    private void Awake()
    {
        _playBtn.onClick.AddListener(Pick);
    }

    private void OnEnable()
    {
        _control.PlayIdle();
        foreach (var item in _items)
        {
            item.CanPick = false;
        }
    }

    private void Pick()
    {
        if (!_basketCtrl.IsFull)
        {
            _control.PlayPick(SpawnPickedItem);
        }
    }
    private void SpawnPickedItem()
    {
        _item.transform.position = _attachmentPoint.position;
        _item.gameObject.SetActive(true);
        _item.Init(_items[Random.Range(0, _items.Count)].Item);
        _item.CanPick = false;

        DOVirtual.DelayedCall(0.25f, () =>
        {
            Observer.Instance.NotifyWithData(ObserverTopic.OnPickItem, _item);
            _item.gameObject.SetActive(false);
        });
    }
}
