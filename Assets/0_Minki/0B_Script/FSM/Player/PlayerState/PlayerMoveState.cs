using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName) { }
    
    public override void UpdateState() {
        float xInput = _player.Input.Movement.x;

        _player.SetVelocity(xInput * _player.Stat.MoveSpeed, _player.RigidbodyCompo.linearVelocityY);

        if(Mathf.Abs(xInput) < 0.05f) {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }
}
