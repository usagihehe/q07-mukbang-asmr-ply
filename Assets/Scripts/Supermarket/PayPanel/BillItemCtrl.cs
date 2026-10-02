using DG.Tweening;
using TMPro;
using UnityEngine;

public class BillItemCtrl : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _itemName;

    [SerializeField]
    private TextMeshProUGUI _itemPrice;

    [SerializeField]
    private float _timeScale;

    [SerializeField]
    private float _scaleVal;

    private int _quantity;

    public SupermarketItemSO SO { get; private set; }

    public int Quantity => _quantity;

    public void AddItem(SupermarketItemSO so)
    {
        SO = so;
        _quantity++;
        _itemName.text = so.name + " x" + _quantity;
        _itemPrice.text = "" + (so.Price * _quantity);
        transform.DOScale(_scaleVal, _timeScale).OnComplete(() =>
        {
            transform.localScale = Vector3.one;
        });
    }

    public void ResetBillItem()
    {
        _quantity = 0;
        _itemName.text = "";
        _itemPrice.text = "";
        gameObject.SetActive(false);
        SO = null;
    }
}
