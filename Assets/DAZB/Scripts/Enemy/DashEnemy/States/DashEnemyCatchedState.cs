using UnityEngine;

public class DashEnemyCatchedState : EnemyState<DashEnemyStateEnum> {
    public DashEnemyCatchedState(Enemy enemy, EnemyStateMachine<DashEnemyStateEnum> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void UpdateState() {
        base.UpdateState();

        if (enemy.isCatchCanceled == true) {
            stateMachine.ChangeState(DashEnemyStateEnum.Attack);
        }
    }

    public override void Exit() {
        enemy.isCatchCanceled = false;
        base.Exit();
    }
}