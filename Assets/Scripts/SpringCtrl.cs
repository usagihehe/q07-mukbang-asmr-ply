using UnityEngine;
using UnityEngine.UI;

public class SpringCtrl : MonoBehaviour
{
    public float _defaultScale;
    private Button _button;
    private SpringUtils.tDampedSpringMotionParams springParams;
    private float frequency = 10.0f;
    private float dampingRatio = 0.5f;
    private float offsetSpringFeel = 0.15f;
    private float currentScale;
    private float velScale = 1.2f;

    private void Start()
    {
        springParams = new SpringUtils.tDampedSpringMotionParams();
        currentScale = _defaultScale;
        velScale = 0f;
        SpringUtils.CalcDampedSpringMotionParams(ref springParams, Time.deltaTime, frequency * Mathf.PI, dampingRatio);
        _button = GetComponent<Button>();
    }

    public void PlaySpring()
    {
        currentScale -= offsetSpringFeel;
        velScale = -2.0f;
    }

    private void Update()
    {
        SpringUtils.CalcDampedSpringMotionParams(ref springParams, Time.deltaTime, frequency * Mathf.PI, dampingRatio);
        SpringUtils.UpdateDampedSpringMotion(ref currentScale, ref velScale, _defaultScale, springParams);
        transform.localScale = Vector3.one * currentScale;
    }
}
