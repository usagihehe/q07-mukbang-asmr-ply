using UnityEngine;
using UnityEngine.UI;

public class SpringCtrl : MonoBehaviour
{
    public float _defaultScale;
    private Button _button;
    private SpringUtils.tDampedSpringMotionParams springParams;
    private float frequency = 10.0f;
    private float dampingRatio = 0.3f;
    private float offsetSpringFeel = 0.15f;
    private float currentScale;
    private float velScale = 1.2f;
    private const float REST_EPSILON = 1e-4f;
    private const float MIN_DEFORM = 0.1f;

    private void Start()
    {
        springParams = new SpringUtils.tDampedSpringMotionParams();
        currentScale = _defaultScale;
        velScale = 0f;
        SpringUtils.CalcDampedSpringMotionParams(ref springParams, Time.deltaTime, frequency * Mathf.PI, dampingRatio);
        _button = GetComponent<Button>();
        ApplyScale();
    }

    public void PlaySpring()
    {
        currentScale -= offsetSpringFeel;
        velScale = -2.0f;
    }

    private void Update()
    {
        // At rest: skip, so the canvas is not dirtied every frame.
        if (currentScale == _defaultScale && velScale == 0f) return;
        SpringUtils.CalcDampedSpringMotionParams(ref springParams, Time.deltaTime, frequency * Mathf.PI, dampingRatio);
        SpringUtils.UpdateDampedSpringMotion(ref currentScale, ref velScale, _defaultScale, springParams);
        if (Mathf.Abs(currentScale - _defaultScale) < REST_EPSILON && Mathf.Abs(velScale) < REST_EPSILON)
        {
            currentScale = _defaultScale;
            velScale = 0f;
        }
        ApplyScale();
    }

    // Squash & stretch with constant area: height follows the spring, width is its inverse.
    private void ApplyScale()
    {
        float deform = Mathf.Max(currentScale / _defaultScale, MIN_DEFORM);
        transform.localScale = new Vector3(_defaultScale / deform, _defaultScale * deform, 1f);
    }
}
