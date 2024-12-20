using UnityEngine;

public class PlayerFallState : PlayerState
{
    public PlayerFallState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName) { }

    private bool _isInterference;
    private int _enterDir;

    private Rigidbody2D _rigidbody;

    public override void Enter() {
        base.Enter();
        
        _rigidbody = _player.RigidbodyCompo;
        _enterDir = (int)Mathf.Sign(_rigidbody.linearVelocityX);
        _isInterference = false;
    }

    public override void UpdateState() {
        base.UpdateState();
        
        float xInput = _player.Input.Movement.x;

        if(Mathf.Abs(xInput) > 0.05f) {
            if(!_isInterference && xInput != _enterDir) _isInterference = true;
        }

        if(_isInterference)
            _player.SetVelocity(xInput * _player.Stat.MoveSpeed * 0.7f, _rigidbody.linearVelocityY);

        if(_player.IsDetecteGround()) {
            if(Mathf.Abs(xInput) > 0.05f) _stateMachine.ChangeState(PlayerStateEnum.Move);
            else _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }
}
