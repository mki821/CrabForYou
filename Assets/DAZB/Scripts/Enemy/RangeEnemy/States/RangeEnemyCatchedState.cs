using UnityEngine;

public class RangeEnemyCatchedState : EnemyState<RangeEnemyStateEnum> {
    public RangeEnemyCatchedState(Enemy enemy, EnemyStateMachine<RangeEnemyStateEnum> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void UpdateState() {
        base.UpdateState();

        if (enemy.isCatchCanceled == true) {
            stateMachine.ChangeState(RangeEnemyStateEnum.Battle);
        }
    }

    public override void Exit() {
        enemy.isCatchCanceled = false;
        base.Exit();
    }
}