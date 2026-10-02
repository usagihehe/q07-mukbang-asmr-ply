using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PayMachineCtrl : MonoBehaviour
{
    [SerializeField]
    private ImageState _scan;

    [SerializeField]
    private Image _scanEffect;

    [SerializeField]
    private Button _doneBtn;

    [SerializeField]
    private AudioClip _doneSound;

    [SerializeField]
    private GameObject _handObj;

    [SerializeField]
    private TransformTweenAnim _direct;

    [SerializeField]
    private BillItemCtrl[] _billItems;

    [SerializeField]
    private ScanItem[] _scanItems;

    [SerializeField]
    private TextMeshProUGUI _totalTxt;

    [SerializeField] private GameObject _scanScreen;
    [SerializeField] private GameObject _billScreen;
    [SerializeField] private GameObject _thankScreen;

    private Dictionary<PayScreen, GameObject> _screens;
    private Dictionary<SupermarketItemSO, int> _itemBill;
    private PayScreen _curScreen;
    private int _countScan;

    public Action onFirstScan;

    public ImageState Scan => _scan;
    public Transform FirstScanItem => _scanItems.Length > 0 ? _scanItems[0].transform : null;
    public Image ScaneEffect => _scanEffect;
    public TransformTweenAnim Direct => _direct;
    public Button DoneBtn => _doneBtn;
    public List<SupermarketItemSO> Items { get; private set; }

    public PayScreen CurScreen
    {
        get
        {
            return _curScreen;
        }
        set
        {
            _screens[_curScreen].SetActive(false);
            _curScreen = value;
            _screens[_curScreen].SetActive(true);
        }
    }

    private void Awake()
    {
        _screens = new Dictionary<PayScreen, GameObject>()
        {
            {PayScreen.Scan, _scanScreen },
            {PayScreen.Bill, _billScreen},
            {PayScreen.Thank, _thankScreen },
        };
        _doneBtn.onClick.AddListener(() =>
        {
            CurScreen = PayScreen.Thank;
            _doneBtn.gameObject.SetActive(false);
        });
    }

    private void OnEnable()
    {
        _itemBill = new Dictionary<SupermarketItemSO, int>();
        _scan.SetState(IMGState.OFF);
        ScaneEffect.gameObject.SetActive(false);
        _countScan = 0;
        for (int i = 0; i < _scanItems.Length; i++)
        {
            _scanItems[i].gameObject.SetActive(false);
        }
        foreach (var bill in _billItems)
        {
            bill.ResetBillItem();
        }
    }

    public void SetItems(List<SupermarketItemSO> items)
    {
        Items = new List<SupermarketItemSO>();
        Items = items;
        for (int i = 0; i < items.Count; i++)
        {
            int index = i;
            _scanItems[index].gameObject.SetActive(true);
            _scanItems[index].Init(items[index]);
        }
    }

    public bool IsScanAll()
    {
        return _countScan == Items.Count;
    }

    public void PrintItem(SupermarketItemSO item)
    {
        if (_countScan == 0)
        {
            onFirstScan?.Invoke();
        }
        _countScan++;
        if (_itemBill.ContainsKey(item))
        {
            _itemBill[item] += item.Price;
        }
        else
        {
            _itemBill.Add(item, item.Price);
        }
        BillItemCtrl bill = GetBillItem(item);
        if (bill != null)
        {
            bill.AddItem(item);
            bill.gameObject.SetActive(true);
        }
        _totalTxt.text = $"{_itemBill.Values.Sum()}";
    }

    private BillItemCtrl GetBillItem(SupermarketItemSO item)
    {
        foreach (var billItem in _billItems)
        {
            if (billItem.SO == null)
            {
                return billItem;
            }
            else if (billItem.SO == item)
            {
                return billItem;
            }
        }
        return null;
    }
}
