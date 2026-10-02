using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class ItemSpmkIconCtrl : MonoBehaviour
{
    [SerializeField]
    protected Image _IconImg;

    public virtual bool ActiveIcon(SupermarketItemSO itemSo)
    {
        if (itemSo == null) return false;
        GetComponent<CanvasGroup>().alpha = 1;
        gameObject.SetActive(true);
        _IconImg.sprite = itemSo.GetIcon();
        _IconImg.SetNativeSize();
        _IconImg.transform.localScale = Vector3.one * itemSo.OffsetScaleOnTable;
        return true;
    }

    public void DisableIcon()
    {
        _IconImg.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public bool IsActive()
    {
        return _IconImg.gameObject.activeSelf;
    }

    private void OnValidate()
    {
        _IconImg = GetComponentInChildren<Image>();
    }
}
