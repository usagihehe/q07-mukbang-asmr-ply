using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PayPanel : MonoBehaviour, IPanel
{
    [SerializeField]
    protected PayMachineCtrl _payMachineCtrl;

    [SerializeField]
    protected Button _doneBtn;

    [SerializeField]
    protected Button _xBtn;

    [SerializeField]
    protected float _timeToNext;

    [SerializeField]
    protected AlphaTweenAnim _alphaTweenAnim;

    [SerializeField]
    protected CoinBarCtrl _coinBarCtrl;

    protected List<SupermarketItemSO> _items;
    [SerializeField] private AudioClip _scanComplete;

    public List<SupermarketItemSO> Items => _items;

    private void Awake()
    {
        _doneBtn.onClick.AddListener(OnPressDoneBtn);
        _xBtn.onClick.AddListener(OnPressXBtn);
    }

    private void OnEnable()
    {
        _doneBtn.gameObject.SetActive(false);
    }

    public void ShowPanel(List<SupermarketItemSO> items)
    {
        _items = items;
        _payMachineCtrl.SetItems(items);
    }

    protected virtual void OnPressDoneBtn()
    {
        AudioManager.Instance.PlaySoundEffect(_scanComplete);
        int total = 0;
        foreach (var item in _items)
        {
            total += item.Price;
        }
        _coinBarCtrl.TakeCoin(total);

        DOVirtual.DelayedCall(_timeToNext, () =>
        {
            UIManager.Instance.ShowSupermarketLivePanel(_items);
            ShowOuttro(() => gameObject.SetActive(false));
        });

    }

    protected virtual void OnPressXBtn()
    {
    }

    public void ShowIntro(Action onCompleted)
    {
        _alphaTweenAnim.PlayAnim(onCompleted);
    }

    public void ShowOuttro(Action onCompleted)
    {
        _alphaTweenAnim.Revert(onCompleted);
    }
}
