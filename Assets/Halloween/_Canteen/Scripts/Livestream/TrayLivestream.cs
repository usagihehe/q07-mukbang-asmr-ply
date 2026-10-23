using System.Collections.Generic;
using SLunch.Tray;
using UnityEngine;

namespace SLunch.Livestream
{
    public class TrayLivestream : MonoBehaviour, ITrayLivestream
    {
        private List<ITraySlot> _slots;

		public void Initialize()
        {
            _slots = new List<ITraySlot>(GetComponentsInChildren<ITraySlot>());
        }

        public void CreateFoods()
        {
        }

        public ITraySlot GetSlotByID(int id)
        {
            return null;
        }

        public bool IsCompleted()
        {
            return false;
        }

        public void RetrieveFoods()
        {
        }
    }
}
