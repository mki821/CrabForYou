using UnityEngine;

public class LaserEnemyCatchedState : EnemyState<LaserEnemyStateEnum> {
    public LaserEnemyCatchedState(Enemy enemy, EnemyStateMachine<LaserEnemyStateEnum> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void UpdateState() {
        base.UpdateState();

        if (enemy.isCatchCanceled == true) {
            stateMachine.ChangeState(LaserEnemyStateEnum.Stealth);
        }
    }

    public override void Exit() {
        enemy.isCatchCanceled = false;
        base.Exit();
    }
}