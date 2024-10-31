using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName) { }

    public override void Enter() {
        base.Enter();

        _player.StopImmediately(false);
    }

    public override void UpdateState() {
        float xInput = _player.Input.Movement.x;

        if(Mathf.Abs(xInput) > 0.05f) {
            _stateMachine.ChangeState(PlayerStateEnum.Move);
        }
    }
}
