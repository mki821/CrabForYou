using UnityEngine;

public class PlayerRopeState : PlayerState
{
    public PlayerRopeState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName) { }

    private int _facingDirection;
    private float _currentAngle;
    private float _anchorDistance;
    private float _currentRopeVelocity;
    private Vector2 _anchorPosition;

    private float _originGravityScale;
    private Rigidbody2D _rigidbody;

    public override void Enter() {
        base.Enter();
        
        _player.Input.RopeCancelEvent += RopeCancel;
        _player.Input.JumpEvent += Acceleration;

        _rigidbody = _player.RigidbodyCompo;
        _originGravityScale = _rigidbody.gravityScale;
        _rigidbody.gravityScale = 0f;

        _anchorPosition = _player.Rope.anchorPosition;

        _currentRopeVelocity = 15f;

        Vector2 direction = (Vector2)_player.transform.position - _anchorPosition;
        _anchorDistance = direction.magnitude;
        _facingDirection = -(int)Mathf.Sign(direction.x);

        direction.Normalize();

        _currentAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    public override void UpdateState() {
        Vector2 direction = (Vector2)_player.transform.position - _anchorPosition;
        direction.Normalize();

        _currentAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        float delta = _currentAngle * Mathf.Deg2Rad;
        Vector2 currentPosition = new Vector2(Mathf.Cos(delta), Mathf.Sin(delta)) * _anchorDistance;

        _player.transform.position = _anchorPosition + currentPosition;
        
        _currentAngle += _facingDirection * _anchorDistance * Time.deltaTime * 60f;
        _currentAngle %= 360;

        delta = _currentAngle * Mathf.Deg2Rad;
        Vector2 nextPosition = new Vector2(Mathf.Cos(delta), Mathf.Sin(delta)) * _anchorDistance;
        
        Vector2 velocity = nextPosition - currentPosition;
        velocity.Normalize();

        _currentRopeVelocity += -Mathf.Sign(velocity.y) * _anchorDistance * 2.5f * Time.deltaTime;
        _currentRopeVelocity = Mathf.Clamp(_currentRopeVelocity, -25f, 25f);

        velocity *= _currentRopeVelocity;

        _player.SetVelocity(velocity.x, velocity.y, true);
    }

    public override void Exit() {
        _player.Input.RopeCancelEvent -= RopeCancel;
        _player.Input.JumpEvent -= Acceleration;

        _rigidbody.gravityScale = _originGravityScale;

        base.Exit();
    }

    private void RopeCancel() => _player.StateMachine.ChangeState(PlayerStateEnum.Fall);

    private void Acceleration() {
        _currentRopeVelocity += 5f;
        _currentRopeVelocity = Mathf.Clamp(_currentRopeVelocity, -25f, 25f);
    }
}
