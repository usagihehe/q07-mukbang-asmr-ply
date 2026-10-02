using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FitToScreenXY : MonoBehaviour
{
    private Canvas _canvas;
    private RectTransform _rectTransform;

    private IEnumerator Start()
    {
        // Wait 1 frame for Canvas to be fully initialized
        yield return new WaitForEndOfFrame();

        // Automatically fit on start
        Fit();
    }

    public void Fit()
    {
        // Find parent Canvas if not already cached
        if (_canvas == null)
        {
            _canvas = GetComponentInParent<Canvas>();
        }

        // Call Fit with the found Canvas
        if (_canvas != null)
        {
            Fit(_canvas);
        }
    }

    public void Fit(Canvas canvas)
    {
        if (canvas == null) return;

        // Get RectTransform of this object
        if (_rectTransform == null)
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        if (canvasRect == null) return;
        Vector2 canvasSize = canvasRect.sizeDelta;
        Vector2 rectSize = _rectTransform.sizeDelta;

        // If Canvas has zero size, use screen dimensions
        if (canvasSize.x == 0 || canvasSize.y == 0)
        {
            canvasSize = new Vector2(Screen.width, Screen.height);
        }

        float canvasRatioY = canvasSize.y / rectSize.y;

        //Debug.Log($"canvasSize {canvasSize}");
        //Debug.Log($"rectSize {rectSize}");
        //Debug.Log($"canvasRatio {canvasRatioY}");
        //Debug.Log($"_rectTransform.localScale {_rectTransform.localScale}");

        Vector3 newScale = Vector3.one * canvasRatioY;
        _rectTransform.localScale = newScale;

        float canvasRatioX = canvasSize.x / rectSize.x;
        Vector3 newScaleX = _rectTransform.localScale * canvasRatioX;
        _rectTransform.localScale = newScaleX;

    }

    public void ResetScale()
    {
        // Reset to original scale
        if (_rectTransform != null)
        {
            _rectTransform.localScale = Vector3.one;
        }
    }
}
