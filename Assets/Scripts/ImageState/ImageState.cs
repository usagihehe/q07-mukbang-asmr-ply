using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageState : MonoBehaviour
{
    [SerializeField]
    private Image _image;

    [SerializeField]
    private Sprite _on;

    [SerializeField]
    private Sprite _off;

    public void SetState(IMGState state)
    {
        if (_image == null) return;

        switch (state)
        {
            case IMGState.ON:
                _image.sprite = _on;
                break;
            case IMGState.OFF:
                _image.sprite = _off;
                break;
            default:
                break;
        }
    }
}