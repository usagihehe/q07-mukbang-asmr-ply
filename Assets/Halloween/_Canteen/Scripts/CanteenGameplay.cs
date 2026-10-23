using SLunch;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Usaki;

public class CanteenGameplay : MonoBehaviour
{
    [SerializeField] private Button _buttonDone;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private Image _lockImage;

    protected CanteenPanel _canteenPanel;
    [SerializeField] private AudioClip _bgm;
    private float _idleTimer;

    private void Awake()
    {
        _buttonDone.onClick.AddListener(Show);
        _scrollRect.onValueChanged.AddListener(OnScrollValueChanged);
    }

    public void Setup(CanteenPanel panel)
    {
        _canteenPanel = panel;
    }

    private void OnEnable()
    {
        AudioManager.Instance.PlayBGM(_bgm);
        _buttonDone.gameObject.SetActive(false);
        StartCoroutine(IEShowLockImage());
    }

    private void OnScrollValueChanged(Vector2 pos)
    {
        _lockImage.gameObject.SetActive(false);
    }


    private void EnableLockImage()
    {
        _lockImage.gameObject.SetActive(true);
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void Show()
    {
        CanteenManager.Instance.ShowCateenChooseWater();
    }

    public void ShowButtonDone()
    {
        _buttonDone.gameObject.SetActive(true);
    }

    private IEnumerator IEShowLockImage()
    {
        yield return new WaitForSeconds(0.75f);
        EnableLockImage();
    }
}
