using System.Collections;
using UnityEngine;

namespace Usaki
{
    /// <summary>
    /// Scales to the canvas height only; width is free because the ScrollView content can extend horizontally.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class FitToScreenHeight : MonoBehaviour
    {
        private Canvas _canvas;
        private RectTransform _rectTransform;
        private float _fittedCanvasHeight;

        private IEnumerator Start()
        {
            // Canvas size is only final after its first layout pass
            yield return new WaitForEndOfFrame();
            Fit();
        }

        // Polls the canvas height because rotation resizes the canvas, not this rect, so no resize callback reaches us
        private void Update()
        {
            if (_canvas == null) return;

            float canvasHeight = ((RectTransform)_canvas.transform).rect.height;
            if (!Mathf.Approximately(canvasHeight, _fittedCanvasHeight))
            {
                Fit(_canvas);
            }
        }

        public void Fit()
        {
            if (_canvas == null)
            {
                _canvas = GetComponentInParent<Canvas>();
            }

            if (_canvas != null)
            {
                Fit(_canvas);
            }
        }

        public void Fit(Canvas canvas)
        {
            if (canvas == null) return;

            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }

            float rectHeight = _rectTransform.rect.height;
            if (rectHeight <= 0) return;

            float canvasHeight = ((RectTransform)canvas.transform).rect.height;
            _fittedCanvasHeight = canvasHeight;
            if (canvasHeight <= 0)
            {
                canvasHeight = Screen.height;
            }

            _rectTransform.localScale = Vector3.one * (canvasHeight / rectHeight);
        }

        public void ResetScale()
        {
            if (_rectTransform != null)
            {
                _rectTransform.localScale = Vector3.one;
            }
        }
    }
}
