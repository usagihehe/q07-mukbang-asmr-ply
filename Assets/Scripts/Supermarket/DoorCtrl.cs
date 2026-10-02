using DG.Tweening;
using System;
using UnityEngine;

public class DoorCtrl : MonoBehaviour
{
    [SerializeField]
    private RectTransform _leftDoor;

    [SerializeField]
    private RectTransform _rightDoor;

    [SerializeField]
    private float _time;

    [SerializeField]
    private Ease _openEase;

    [SerializeField]
    private Ease _closeEase;

    private float _xLeftOrigin;

    private float _xRightOrigin;

    private void Awake()
    {
        // Store the original X positions of both doors
        if (_leftDoor != null)
        {
            _xLeftOrigin = _leftDoor.anchoredPosition.x;
        }

        if (_rightDoor != null)
        {
            _xRightOrigin = _rightDoor.anchoredPosition.x;
        }
    }

    /// <summary>
    /// Opens the door. Animates doors moving outward.
    /// </summary>
    /// <param name="playAnim">If true, play the animation. If false, set position instantly.</param>
    public void OpenDoor(bool playAnim, Action callback = null)
    {
        if (playAnim)
        {
            // Move the left door to the left and the right door to the right
            _leftDoor?.DOAnchorPosX(_xLeftOrigin - 600, _time).SetEase(_openEase); // Adjust "200" as the offset for the left door
            _rightDoor?.DOAnchorPosX(_xRightOrigin + 600, _time).SetEase(_openEase).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }
        else
        {
            _leftDoor.anchoredPosition = new Vector2(_xLeftOrigin - 600, _leftDoor.anchoredPosition.y);
            _rightDoor.anchoredPosition = new Vector2(_xRightOrigin + 600, _rightDoor.anchoredPosition.y);
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Closes the door. Animates doors moving back to their original positions.
    /// </summary>
    /// <param name="playAnim">If true, play the animation. If false, set position instantly.</param>
    public void CloseDoor(bool playAnim, Action callback = null)
    {
        gameObject.SetActive(true);
        if (playAnim)
        {
            // Move the left and right doors back to their original positions
            _leftDoor?.DOAnchorPosX(_xLeftOrigin, _time).SetEase(_closeEase);
            _rightDoor?.DOAnchorPosX(_xRightOrigin, _time).SetEase(_closeEase);
        }
        else
        {
            _leftDoor.anchoredPosition = new Vector2(_xLeftOrigin, _leftDoor.anchoredPosition.y);
            _rightDoor.anchoredPosition = new Vector2(_xRightOrigin, _rightDoor.anchoredPosition.y);
        }
    }

}