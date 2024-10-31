using UnityEngine;

public class CommonEnemyBattleState : EnemyState<CommonEnemyStateEnum> {
    private CommonEnemy enemy;

    public CommonEnemyBattleState(Enemy enemy, EnemyStateMachine<CommonEnemyStateEnum> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        this.enemy = enemy as CommonEnemy;
    }

    private Transform playerTrm;

    public override void Enter() {
        base.Enter();
    }

    public override void Exit() {
        base.Exit();
    }

    public override void UpdateState() {
        base.UpdateState();
    }
}