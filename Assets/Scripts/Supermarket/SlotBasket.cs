using UnityEngine;
using UnityEngine.UIElements;

public class SlotBasket : MonoBehaviour
{
    [SerializeField]
    private ItemSpmkIconCtrl _spmkIconCtrl;
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

    public void ClearSlot()
    {
        Item = null;
        _spmkIconCtrl.gameObject.SetActive(false);
    }

    private void OnValidate()
    {
        _spmkIconCtrl = transform.GetChild(0).GetComponent<ItemSpmkIconCtrl>();
    }
}
