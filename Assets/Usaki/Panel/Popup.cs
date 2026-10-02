using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    private static List<Popup> _popups = new List<Popup>();
    [SerializeField] protected Button _hideBtn;
    [SerializeField] protected Button _overlayBtn;
    [SerializeField] protected CanvasGroup _overlay;
    [SerializeField] protected UIAnim _popup;
    [SerializeField] protected AudioClip _popupSound;
    [SerializeField] protected Ease _fadeEase = Ease.OutQuad;

    public UIAnim PopupAnim => _popup;
    public CanvasGroup Overlay => _overlay;
    public Button HideBtn => _hideBtn;
    public Button OverlayBtn => _overlayBtn;

    public event Action onDone;

    public static void AddActivePopup(Popup popup)
    {
        if (!_popups.Contains(popup))
        {
            _popups.Add(popup);
        }
    }

    public static void RemoveActivePopup(Popup popup)
    {
        if (_popups.Contains(popup))
        {
            _popups.Remove(popup);
        }
    }

    public static void HideAllPopups()
    {
        foreach (var popup in new List<Popup>(_popups))
        {
            popup.HidePopup();
        }
    }

    public static Popup GetLastActivePopup()
    {
        if (_popups.Count > 0)
        {
            return _popups[_popups.Count - 1];
        }
        return null;
    }

    protected virtual void Awake()
    {
        LoadReferences();
    }

    [ContextMenu("LoadReferences")]
    protected virtual void LoadReferences()
    {
        _hideBtn = _hideBtn == null ? GetComponentInChildren<Button>() : _hideBtn;
        _overlayBtn = _overlayBtn == null ? GetComponentInChildren<Button>() : _overlayBtn;
        _overlay = _overlay == null ? GetComponentInChildren<CanvasGroup>() : _overlay;
        _popup = _popup == null ? GetComponentInChildren<UIAnim>() : _popup;
    }

    public virtual void ShowPopup(Action callbackOnComplete = null)
    {
        gameObject.SetActive(true);
        AddActivePopup(this);
        if (_popupSound != null) AudioManager.Instance?.PlaySoundEffect(_popupSound);
        if (_popup != null)
        {
            _popup.ShowIntro(() =>
            {
                callbackOnComplete?.Invoke();
            });
        }
        else
        {
            callbackOnComplete?.Invoke();
        }
    }

    public virtual void HidePopup(Action callbackOnComplete = null)
    {
        RemoveActivePopup(this);

        if (_popupSound != null) AudioManager.Instance?.PlaySoundEffect(_popupSound);
        if (_popup != null)
        {
            _popup?.ShowOuttro(() =>
            {
                gameObject.SetActive(false);
                callbackOnComplete?.Invoke();
                onDone?.Invoke();
            });
        }
        else
        {
            gameObject.SetActive(false);
            callbackOnComplete?.Invoke();
            onDone?.Invoke();
        }

    }

    protected virtual void SetInteracableHideButton(bool active)
    {
        if (_hideBtn != null)
            _hideBtn.interactable = active;
        if (_overlayBtn != null)
            _overlayBtn.interactable = active;
    }
}