using UnityEngine;
using UnityEngine.UI;

public class SoundClick : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => AudioManager.Instance.PlayAudioClick());
    }
}
