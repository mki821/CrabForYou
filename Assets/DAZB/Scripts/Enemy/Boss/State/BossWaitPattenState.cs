using UnityEngine;

public class BossWaitPattenState : EnemyState<BossStateEnum> {
    public BossWaitPattenState(Enemy enemy, EnemyStateMachine<BossStateEnum> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter() {
        base.Enter();
    }
}