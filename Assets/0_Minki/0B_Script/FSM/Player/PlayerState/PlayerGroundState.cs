public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName) { }

    public override void Enter() {
        base.Enter();

        _player.Input.JumpEvent += HandleJump;
    }

    public override void Exit() {
        _player.Input.JumpEvent -= HandleJump;

        base.Exit();
    }

    private void HandleJump() {
        if(_player.IsDetecteGround())
            _stateMachine.ChangeState(PlayerStateEnum.Jump);
    }
}
