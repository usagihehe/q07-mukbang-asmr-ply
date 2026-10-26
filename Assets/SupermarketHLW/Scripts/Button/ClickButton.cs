using UnityEngine;
using UnityEngine.UI;

public class ClickButton : MonoBehaviour
{
    [SerializeField]
    private bool _loadDefaultScale;
    private float _defaultScale;
    private Button _button;
    private SpringUtils.tDampedSpringMotionParams springPram;
    private float frequency = 5.0f;
    private float dampingRatio = 0.3f;
    private float currentScale;
    private float velScale = 0.5f;

    private void Start()
    {
        _button = GetComponent<Button>();
        if (_button != null)
        {
            _button.onClick.AddListener(OnButtonClick);
        }
        Init();
    }

    public bool Init()
    {
        _defaultScale = transform.localScale.x;
        currentScale = _defaultScale;
        velScale = 0;
        if (_loadDefaultScale) transform.localScale = Vector3.one * _defaultScale;
        springPram = new SpringUtils.tDampedSpringMotionParams();
        SpringUtils.CalcDampedSpringMotionParams(ref springPram, Time.deltaTime, frequency, dampingRatio);
        return false;
    }

    private void Update()
    {
        if (_button.enabled)
        {
            SpringUtils.UpdateDampedSpringMotion(ref currentScale, ref velScale, _defaultScale, springPram);
            transform.localScale = new Vector3(currentScale, currentScale, currentScale);
        }
    }

    private void OnButtonClick()
    {
        currentScale = _defaultScale * 0.8f;
        velScale = 0;
    }

}
