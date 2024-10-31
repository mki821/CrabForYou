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

        _rigidbody = _player.RigidbodyCompo;
        _originGravityScale = _rigidbody.gravityScale;
        _rigidbody.gravityScale = 0f;

        _anchorPosition = _player.Rope.anchorPosition;
        _currentRopeVelocity = _rigidbody.linearVelocityY;

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
        
        _currentAngle += _facingDirection * _currentRopeVelocity * 18f * Time.deltaTime;
        _currentAngle %= 360f;

        delta = _currentAngle * Mathf.Deg2Rad;
        Vector2 nextPosition = new Vector2(Mathf.Cos(delta), Mathf.Sin(delta)) * _anchorDistance;

        Vector2 velocity = nextPosition - currentPosition;
        _currentRopeVelocity += -Mathf.Sign(velocity.y) * _anchorDistance * 2.5f * Time.deltaTime;
        _currentRopeVelocity = Mathf.Clamp(_currentRopeVelocity, -10f, 10f);

        _player.transform.position = _anchorPosition + nextPosition;
    }

    public override void Exit() {

        float delta = _currentAngle * Mathf.Deg2Rad;
        Vector2 currentPosition = new Vector2(Mathf.Cos(delta), Mathf.Sin(delta)) * _anchorDistance;
        
        _currentAngle += _facingDirection * Time.deltaTime * 180f;
        _currentAngle %= 360;

        delta = _currentAngle * Mathf.Deg2Rad;
        Vector2 nextPosition = new Vector2(Mathf.Cos(delta), Mathf.Sin(delta)) * _anchorDistance;

        Vector2 velocity = nextPosition - currentPosition;
        velocity.Normalize();

        _currentRopeVelocity += -Mathf.Sign(velocity.y) * _anchorDistance * 2.5f * Time.deltaTime;
        _currentRopeVelocity = Mathf.Clamp(_currentRopeVelocity, -10f, 10f);

        velocity *= _currentRopeVelocity;

        _player.SetVelocity(velocity.x, velocity.y, true);
        
        _player.Input.RopeCancelEvent -= RopeCancel;

        _rigidbody.gravityScale = _originGravityScale;

        base.Exit();
    }

    private void RopeCancel() => _player.StateMachine.ChangeState(PlayerStateEnum.Fall);
}
