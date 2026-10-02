using System;
using UnityEngine;

namespace Usaki
{
    public class AnimEventReceiver : MonoBehaviour
    {
        public Action<string> OnTriggerEvents;

        public void OnTriggerEvent(string eventName)
        {
            OnTriggerEvents?.Invoke(eventName);
        }
    }
}
