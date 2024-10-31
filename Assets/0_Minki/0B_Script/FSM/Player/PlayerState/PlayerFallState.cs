using UnityEngine;

public class PlayerFallState : PlayerState
{
    public PlayerFallState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName) { }

    private bool _isInterference;

    private Rigidbody2D _rigidbody;

    public override void Enter() {
        base.Enter();
        
        _rigidbody = _player.RigidbodyCompo;
        _isInterference = false;
    }

    public override void UpdateState() {
        base.UpdateState();
        
        float xInput = _player.Input.Movement.x;

        if(Mathf.Abs(xInput) > 0.05f)
            if(!_isInterference) _isInterference = true;

        if(_isInterference)
            _player.SetVelocity(xInput * _player.moveSpeed, _rigidbody.linearVelocityY);

        if(_player.IsDetecteGround()) {
            if(Mathf.Abs(xInput) > 0.05f) _stateMachine.ChangeState(PlayerStateEnum.Move);
            else _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }
}
