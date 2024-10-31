using UnityEngine;

public class DashEnemyCatchedState : EnemyState<DashEnemyStateEnum> {
    public DashEnemyCatchedState(Enemy enemy, EnemyStateMachine<DashEnemyStateEnum> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void UpdateState() {
        base.UpdateState();

    /* if (조건) {
            stateMachine.ChangeState(DashEnemyStateEnum.Attack);
        } */
    }
}