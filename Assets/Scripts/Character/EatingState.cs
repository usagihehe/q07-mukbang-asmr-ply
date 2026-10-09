using DG.Tweening.Core.Easing;
using Usaki;
using Spine;
using Spine.Unity;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class EatingState : CharBaseState
{
    private TrackEntry _biteEntry;
    private SupermarketItemMukbang _itemSmk;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void EnterState()
    {
        _itemSmk = _stateMachine.MukbangItem as SupermarketItemMukbang;
        _biteEntry = null;
        // IS SUPERMARKET
        if (_itemSmk == null) return;
        _stateMachine.EatenItem = _itemSmk;
        EatOneBite();
    }

    public override string Execute(float dt)
    {
        //OTHER FOOD
        if (_character.IsPlaying(_biteEntry)) return StateMachine.EatingState;
        AudioManager.Instance.StopSoundEffect();
        // Going back through Idle for one frame cuts the Spine mix and makes the next bite snap.
        if (IsStillAtMouth())
        {
            EatOneBite();
            return StateMachine.EatingState;
        }
        return _stateMachine.IsSelectItem ? StateMachine.BeforeEatState : StateMachine.AfterEatState;
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

    // The player may drop this food and pick another mid-bite; only keep biting the one still held.
    private bool IsStillAtMouth()
    {
        return _itemSmk != null && !_itemSmk.IsDone && _stateMachine.IsSelectItem
               && _stateMachine.MukbangItem == _itemSmk
               && _stateMachine.Distance() < _stateMachine.DistanceEating;
    }

    private void PlayEatAnimation(MukbangItemSO item)
    {
        string animName = GetAnimName(item);
        if (GameManager.Instance.CharSound.GetEatSound(item.EatSound) != null)
            AudioManager.Instance.PlaySoundEffect(GameManager.Instance.CharSound.GetEatSound(item.EatSound));
        _biteEntry = _character.PlayAnimByName(animName, false);
    }

    private string GetAnimName(MukbangItemSO item)
    {
        return _character.GetAnimName(item.EatSound);
    }
}

