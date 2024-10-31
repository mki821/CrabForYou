using UnityEngine;

public class PlayerRopeState : PlayerState
{
    public PlayerRopeState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName) { }

    private int _facingDirection;
    private float _currentAngle;
    private float _anchorDistance;
    private float _currentRopeVelocity;
    private Vector2 _anchorPosition;

    public override void Enter() {
        base.Enter();
        
        _player.Input.RopeCancelEvent += RopeCancel;

        _anchorPosition = _player.Rope.anchorPosition;
        _currentRopeVelocity = 10f;

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
        
        _currentAngle += _facingDirection * Time.deltaTime * 180f;
        _currentAngle %= 360;

        delta = _currentAngle * Mathf.Deg2Rad;
        Vector2 nextPosition = new Vector2(Mathf.Cos(delta), Mathf.Sin(delta)) * _anchorDistance;

        float yDifference = nextPosition.y - _anchorPosition.y;

        Vector2 velocity = nextPosition - currentPosition;
        velocity.Normalize();
        velocity *= _currentRopeVelocity;

        _player.SetVelocity(velocity.x, velocity.y, true);
    }

    public override void Exit() {
        _player.Input.RopeCancelEvent -= RopeCancel;

        base.Exit();
    }

    private void RopeCancel() => _player.StateMachine.ChangeState(PlayerStateEnum.Fall);
}
