using System.Collections.Generic;
using UnityEngine;
using Usaki;

namespace PLY32
{
    /// <summary>Skips shopping and checkout: the playable opens straight on the eating screen.</summary>
    // Runs after GameManager.Start, whose SetCountComplete(4) would otherwise overwrite the panel's count.
    [DefaultExecutionOrder(100)]
    public class Ply32DirectEatFlow : MonoBehaviour
    {
        [SerializeField] private SupermarketItemSO[] _foods = new SupermarketItemSO[5];

        private void Start()
        {
            UIManager.Instance.ShowSupermarketLivePanel(new List<SupermarketItemSO>(_foods));
        }
    }
}
