using DG.Tweening;
using UnityEngine;

namespace SLunch.Tray
{
    public class TrayVisual : MonoBehaviour
    {
        [SerializeField]
        private Transform _trayTransform;

        [SerializeField]
        [Header("SCALE SETTINGS")]
        private Vector3 _scaleUpFactor = Vector3.one * 1.1f;

        [SerializeField]
        private float _scaleDuration = 0.2f;

        private Vector3 _originalScale;

        private void Awake()
        {
            _originalScale = transform.localScale;
        }

        private void OnDisable()
        {
            transform.DOKill();
        }

        public void ScaleUp()
        {
            transform.DOScale(_scaleUpFactor, _scaleDuration).SetEase(Ease.OutBack);
        }

        public void ScaleDown()
        {
            transform.DOScale(_originalScale, _scaleDuration).SetEase(Ease.OutBack);
        }
    }
}
