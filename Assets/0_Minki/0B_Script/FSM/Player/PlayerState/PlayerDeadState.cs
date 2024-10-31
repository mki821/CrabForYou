using UnityEngine;

public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName) { }

    public override void Enter() {
        base.Enter();

        _player.isDead = true;
        _player.StopImmediately(false);
    }
}
    
