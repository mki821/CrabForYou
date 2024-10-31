using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName) { }

    private Rigidbody2D _rigidbody;

    public override void Enter() {
        base.Enter();

        _rigidbody = _player.RigidbodyCompo;

        _player.SetVelocity(_rigidbody.linearVelocityX, _player.jumpForce);
    }

    public override void UpdateState() {
        base.UpdateState();
        
        if(_rigidbody.linearVelocityY <= 0)
            _stateMachine.ChangeState(PlayerStateEnum.Fall);

        float xInput = _player.Input.Movement.x;

        _player.SetVelocity(xInput * _player.moveSpeed, _rigidbody.linearVelocityY);
    }
}
