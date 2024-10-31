using UnityEngine;

public abstract class PlayerState
{
    protected PlayerStateMachine _stateMachine;
    protected Player _player;

    protected int _animBoolHash;
    protected bool _animationEndTrigger;

    public PlayerState(Player player, PlayerStateMachine stateMachine, string animBoolName) {
        _player = player;
        _stateMachine = stateMachine;
        _animBoolHash = Animator.StringToHash(animBoolName);
    }

    public virtual void Enter() {
        _player.AnimatorCompo.SetBool(_animBoolHash, true);
        _animationEndTrigger = false;
    }

    public virtual void UpdateState() { }

    public virtual void Exit() {
        _player.AnimatorCompo.SetBool(_animBoolHash, false);
    }

    public virtual void AnimationFinishTrigger() {
        _animationEndTrigger = true;
    }
}
