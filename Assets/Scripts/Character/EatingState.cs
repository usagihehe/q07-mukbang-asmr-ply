using DG.Tweening.Core.Easing;
using Usaki;
using Spine;
using Spine.Unity;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class EatingState : CharBaseState
{
    private bool _isPlay;
    private SupermarketItemMukbang _itemSmk;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void EnterState()
    {
        _itemSmk = _stateMachine.MukbangItem as SupermarketItemMukbang;
        _isPlay = _itemSmk != null;
        // IS SUPERMARKET
        if (_itemSmk != null) EatOneBite();
    }

    public override string Execute(float dt)
    {
        //OTHER FOOD
        if (_isPlay) return StateMachine.EatingState;
        AudioManager.Instance.StopSoundEffect();
        // Going back through Idle for one frame cuts the Spine mix and makes the next bite snap.
        if (IsStillAtMouth())
        {
            EatOneBite();
            return StateMachine.EatingState;
        }
        return _stateMachine.IsSelectItem ? StateMachine.IdleState : StateMachine.AfterEatState;
    }

    public override void ExitState()
    {
    }

    public override bool IsSuitable()
    {
        return true;
    }

    private void EatOneBite()
    {
        PlayEatAnimation(_itemSmk.Item);
        _itemSmk.Consume();
    }

    private bool IsStillAtMouth()
    {
        return _itemSmk != null && !_itemSmk.IsDone && _stateMachine.IsSelectItem
               && _stateMachine.Distance() < _stateMachine.DistanceEating;
    }

    private void PlayEatAnimation(MukbangItemSO item)
    {
        _isPlay = true;
        string animName = GetAnimName(item);
        if (GameManager.Instance.CharSound.GetEatSound(item.EatSound) != null)
            AudioManager.Instance.PlaySoundEffect(GameManager.Instance.CharSound.GetEatSound(item.EatSound));
        _character.PlayAnimByName(animName, false, delegate { _isPlay = false; });
    }

    private string GetAnimName(MukbangItemSO item)
    {
        return _character.GetAnimName(item.EatSound);
    }
}

