using UnityEngine;

public class DroneEnemyCatchedState : EnemyState<DroneEnemyStateEnum> {
    public DroneEnemyCatchedState(Enemy enemy, EnemyStateMachine<DroneEnemyStateEnum> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter() {
        base.Enter();
    }

     public override void UpdateState() {
        base.UpdateState();

        if (enemy.isCatchCanceled == true) {
            stateMachine.ChangeState(DroneEnemyStateEnum.Asphyxia);
        }
    }

    public override void Exit() {
        enemy.isCatchCanceled = false;
        base.Exit();
    }
}