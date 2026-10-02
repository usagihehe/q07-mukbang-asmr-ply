using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleToScreen : MonoBehaviour
{
    [Header("Scale Settings")]
    [SerializeField] private bool _scaleOnStart = true;
    [SerializeField] private bool _fillParent = true; // true = fill (cover), false = fit (contain)

    private Canvas _canvas;
    private RectTransform _rectTransform;
    private RectTransform _parentRect;
    private Vector2 _originalSize;

    private IEnumerator Start()
    {
        // Wait 1 frame for Canvas to be fully initialized
        yield return null;

        // Store original size before scaling
        _rectTransform = GetComponent<RectTransform>();
        _originalSize = _rectTransform.sizeDelta;

        // Automatically scale on start if enabled
        if (_scaleOnStart)
        {
            Scale();
        }
    }

    public void Scale()
    {
        // Find parent Canvas if not already cached
        if (_canvas == null)
        {
            _canvas = GetComponentInParent<Canvas>();
        }

        // Call Scale with the found Canvas
        if (_canvas != null)
        {
            Scale(_canvas);
        }
    }

    public void Scale(Canvas canvas)
    {
        if (canvas == null) return;

        // Get RectTransform of this object
        if (_rectTransform == null)
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        // Get parent RectTransform
        if (_parentRect == null)
        {
            _parentRect = _rectTransform.parent as RectTransform;
            if (_parentRect == null)
            {
                // If no parent RectTransform, use Canvas
                _parentRect = canvas.GetComponent<RectTransform>();
            }
        }

        if (_parentRect == null) return;

        // Store original size if not already stored
        if (_originalSize == Vector2.zero)
        {
            _originalSize = _rectTransform.sizeDelta;
        }

        // Get parent size
        Vector2 parentSize = _parentRect.sizeDelta;

        // If parent has zero size, use Canvas size or screen dimensions
        if (parentSize.x == 0 || parentSize.y == 0)
        {
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            parentSize = canvasRect.sizeDelta;

            if (parentSize.x == 0 || parentSize.y == 0)
            {
                parentSize = new Vector2(Screen.width, Screen.height);
            }
        }

        // Get current object size (use original size for calculation)
        Vector2 currentSize = _originalSize;

        // If original size is zero, try to get from Image sprite or use RectTransform
        if (currentSize == Vector2.zero)
        {
            Image image = GetComponent<Image>();
            if (image != null && image.sprite != null)
            {
                currentSize = new Vector2(image.sprite.texture.width, image.sprite.texture.height);
            }
            else
            {
                currentSize = _rectTransform.sizeDelta;
            }
        }

        // If still zero, use default size
        if (currentSize.x == 0 || currentSize.y == 0)
        {
            currentSize = new Vector2(100, 100);
        }

        // Calculate aspect ratios for scaling
        float parentRatio = parentSize.x / parentSize.y;
        float currentRatio = currentSize.x / currentSize.y;

        Vector3 newScale;

        if (_fillParent)
        {
            // Fill parent (cover behavior) - may crop content
            if (currentRatio > parentRatio)
            {
                // Current object is wider than parent
                // Scale to fill height (crop sides if needed)
                float scaleY = parentSize.y / currentSize.y;
                newScale = new Vector3(scaleY, scaleY, 1f);
            }
            else
            {
                // Current object is taller than parent
                // Scale to fill width (crop top/bottom if needed)
                float scaleX = parentSize.x / currentSize.x;
                newScale = new Vector3(scaleX, scaleX, 1f);
            }
        }
        else
        {
            // Fit in parent (contain behavior) - show all content
            if (currentRatio > parentRatio)
            {
                // Current object is wider than parent
                // Scale to fit width
                float scaleX = parentSize.x / currentSize.x;
                newScale = new Vector3(scaleX, scaleX, 1f);
            }
            else
            {
                // Current object is taller than parent
                // Scale to fit height
                float scaleY = parentSize.y / currentSize.y;
                newScale = new Vector3(scaleY, scaleY, 1f);
            }
        }

        // Apply new scale
        _rectTransform.localScale = newScale;
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
