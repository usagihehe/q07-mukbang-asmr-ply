using SLunch.Water;
using System;
using UnityEngine;
namespace SLunch
{
    public class CanteenPanel : MonoBehaviour, IPanel
    {
        [SerializeField] private CanteenGameplay canteenGameplay;
        [SerializeField] private CanteenWaterChoose canteenWaterChoose;

        private void Start()
        {
            canteenGameplay.Setup(this);
            canteenWaterChoose.Setup(this);
        }

        public virtual void ShowIntro(Action onCompleted)
        {
            SetActive(true);
            onCompleted?.Invoke();
        }

        public virtual void ShowOuttro(Action onCompleted)
        {
            onCompleted?.Invoke();
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}
