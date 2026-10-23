using Spine.Unity;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PhoneLiveCtrl : MonoBehaviour
{
    [SerializeField]
    private ScrollRect _scrollRect;

    [SerializeField]
    private RectTransform _charm;

    [SerializeField]
    private SkeletonGraphic _skeletonGraphic;

    [SerializeField]
    private string[] _animList;

    [SerializeField]
    private float _convertAngle;

    [SerializeField]
    private float _minAngle;

    [SerializeField]
    private float _maxAngle;

    private bool _canPlayAnim;

    private int _lastIndex;

    private void Awake()
    {
        //_canPlayAnim = true;
    }

    private void Update()
    {
    }

    private string GetRandAnim()
    {
        return _animList[Random.Range(0, _animList.Length)];
    }
}
