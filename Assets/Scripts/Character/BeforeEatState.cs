using DG.Tweening.Core.Easing;
using Usaki;
using UnityEngine;
using UnityEngine.UI;

public class BeforeEatState : CharBaseState
{
    [SerializeField]
    private float _waitTime;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void EnterState()
    {
        //_wait = true;
        if (GameManager.Instance.CharSound.GetEatSound(SoundType.Open) != null) AudioManager.Instance.PlaySoundEffect(GameManager.Instance.CharSound.GetEatSound(SoundType.Open));
        _character.PlayAnimByName(CharacterCtrl.MOUTH_OPEN, true);
    }

    public override string Execute(float dt)
    {
        if (_stateMachine.MukbangItem != null)
        {
            if (_stateMachine.Distance() < _stateMachine.DistanceEating)
            {
                AudioManager.Instance.StopSoundEffect();
                return StateMachine.EatingState;
            }
            else
            {
                return StateMachine.BeforeEatState;
            }
        }
        else if (!_stateMachine.IsSelectItem)
        {
            AudioManager.Instance.StopSoundEffect();
            return StateMachine.IdleState;
        }
        return StateMachine.BeforeEatState;
    }

    public override void ExitState()
    {
        //_wait = false;
    }

    public override bool IsSuitable()
    {
        return true;
    }

    private static Vector3 GetTopCenterPosition(Image image)
    {
        if (image == null)
        {
            Debug.LogWarning("Image is null!");
            return Vector3.zero;
        }

        RectTransform rectTransform = image.rectTransform;

        Vector3[] worldCorners = new Vector3[4];
        rectTransform.GetWorldCorners(worldCorners);

        // Top Left (1) và Top Right (2)
        Vector3 topCenter = (worldCorners[1] + worldCorners[2]) / 2f;
        return topCenter;
    }
}
