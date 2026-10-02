using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FitToScreen : MonoBehaviour
{
    private Canvas _canvas;
    private RectTransform _rectTransform;
    private Vector2 _lastScreenSize;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _lastScreenSize = new Vector2(Screen.width, Screen.height);
    }

    private IEnumerator Start()
    {
        ResetScale();
        yield return new WaitForEndOfFrame();
        Fit();
    }

    private void Update()
    {
        Vector2 current = new Vector2(Screen.width, Screen.height);
        if (current == _lastScreenSize) return;

        _lastScreenSize = current;
        StartCoroutine(AutoFit());
    }

    private IEnumerator AutoFit()
    {
        ResetScale();
        yield return new WaitForSeconds(0.2f);
        Fit();
    }

    public void Fit()
    {
        if (_canvas == null)
            _canvas = GetComponentInParent<Canvas>();

        if (_canvas != null)
            Fit(_canvas);
    }

    public void Fit(Canvas canvas)
    {
        if (canvas == null) return;

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        if (canvasRect == null) return;

        Vector2 canvasSize = canvasRect.sizeDelta;
        if (canvasSize.x == 0 || canvasSize.y == 0) canvasSize = new Vector2(Screen.width, Screen.height);
        //Debug.Log("canvasRect canvasSize: " + canvasSize);
        Vector2 imageSize = _rectTransform.sizeDelta;
        //Debug.Log("Before imageSize: " + imageSize);

        bool screenIsPortrait = canvasSize.y >= canvasSize.x;


        //if (screenIsPortrait)
        //{
        // fit height first; if scaled width overflows → fall back to fit width (uniform)
        // Scale theo Y
        float scaleY = canvasSize.y / imageSize.y;
        _rectTransform.localScale = Vector3.one * scaleY;

        //float scaledWidth = imageSize.x * scaleY;
        //Debug.Log("After imageSize: " + imageSize);
        //if (scaledWidth < canvasSize.x)
        //{
        //    float scaleX = canvasSize.x / scaledWidth;
        //    _rectTransform.localScale *= scaleX;
        //}

    }

    public void ResetScale()
    {
        if (_rectTransform != null)
            _rectTransform.localScale = Vector3.one;
    }
}
