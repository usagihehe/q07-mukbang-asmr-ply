using System.Collections.Generic;
using UnityEngine;
using Utils.Singletons;

public class SupermarketItemData : ZMonoSingleton<SupermarketItemData>
{
    [SerializeField] private List<SupermarketItemSO> _items;
    [SerializeField] private List<SupermarketItemSO> _drinkItems;
    [SerializeField] private List<SupermarketItemSO> _blindBoxItems;

    public SupermarketItemSO GetItem(string name)
    {
        return _items?.Find(item => item.name == name);
    }

    public bool IsDrinkItem(SupermarketItemSO item)
    {
        return _drinkItems?.Contains(item) ?? false;
    }
    public bool IsBlindBoxItem(SupermarketItemSO item)
    {
        return _blindBoxItems?.Contains(item) ?? false;
    }
}
