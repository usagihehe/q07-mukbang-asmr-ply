using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class SlotBasket : MonoBehaviour
{
    [SerializeField]
    private ItemSpmkIconCtrl _spmkIconCtrl;
    [Header("Put in")]
    [SerializeField]
    private float _timeMove;
    [SerializeField]
    private Ease _moveEase;
    [Header("Take out")]
    [SerializeField]
    private float _timeFly;
    [SerializeField]
    private Vector3 _flyHigh;
    [SerializeField]
    private Ease _fadeEase;
    public SupermarketItemSO Item;
    public bool IsOccupied => Item != null;
    public Vector3 IconWorldPosition => _spmkIconCtrl.transform.position;

    /// Reserves the slot; the icon stays hidden until the fly anim lands on it.
    public void SetItem(SupermarketItemSO item)
    {
        Item = item;
        _spmkIconCtrl.gameObject.SetActive(false);
    }

    public void ShowIcon()
    {
        if (!_spmkIconCtrl.ActiveIcon(Item)) return;
        _spmkIconCtrl.transform.localPosition = Vector3.zero;
        _spmkIconCtrl.transform.localScale = Vector3.one * 0.75f;
    }

    public void SetItem(SupermarketItemSO item, Transform from, Action onDone)
    {
        Item = item;
        _spmkIconCtrl.DOKill();
        CanvasGroup canvasGroup = _spmkIconCtrl.GetComponent<CanvasGroup>();
        canvasGroup.DOKill();
        canvasGroup.alpha = 1;
        _spmkIconCtrl.gameObject.SetActive(true);

        _spmkIconCtrl.ActiveIcon(item);
        _spmkIconCtrl.transform.localPosition = Vector3.zero;
        Vector3 position = _spmkIconCtrl.transform.position;
        _spmkIconCtrl.transform.localScale = Vector3.one * 0.75f;
        _spmkIconCtrl.transform.DOMove(position, _timeMove).SetEase(_moveEase).From(from.position)
            .OnComplete(() =>
            {
                onDone?.Invoke();
                _spmkIconCtrl.transform.localPosition = Vector3.zero;
            });
    }

    public void ClearSlot()
    {
        Item = null;
        _spmkIconCtrl.gameObject.SetActive(false);
    }

    public void TakeOut(Action onDone)
    {
        Item = null;
        Vector3 position = _spmkIconCtrl.transform.position;
        CanvasGroup canvasGroup = _spmkIconCtrl.GetComponent<CanvasGroup>();
        _spmkIconCtrl.DOKill();
        canvasGroup.DOKill();

        _spmkIconCtrl.transform.DOMove(position + _flyHigh, _timeFly).OnComplete(() =>
        {
            onDone?.Invoke();
            if (!IsOccupied)
            {
                _spmkIconCtrl.transform.position = position;
                _spmkIconCtrl.gameObject.SetActive(false);
            }
        });
        canvasGroup.DOFade(0, _timeFly).SetEase(_fadeEase);

    }
}
