using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/SupermarketItem")]
public class SupermarketItemSO : MukbangItemSO
{
    public string ShelfName;
    public Sprite Tool;
    public int Price;
    public int BonusPercent;
    public float OffsetScaleOnTable = 1f;
    protected Dictionary<string, string> _Prams;

    public Sprite GetIcon()
    {
        return Steps != null && Steps.Count > 0 ? Steps[Steps.Count - 1] : null;
    }

    public bool HasLastFrame()
    {
        return Steps != null && Steps[0].name.EndsWith("_99");
    }

    public bool HasBox()
    {
        return Steps != null && Steps[Steps.Count - 1].name.EndsWith("_0");
    }

    [ContextMenu("Reverse")]
    public void Reverse()
    {
        Steps.Reverse();
    }

}
