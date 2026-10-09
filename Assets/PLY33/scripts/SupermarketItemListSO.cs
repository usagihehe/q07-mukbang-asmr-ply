using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/SupermarketItemList")]
public class SupermarketItemListSO : ScriptableObject
{
	public List<SupermarketItemSO> Items;
}
