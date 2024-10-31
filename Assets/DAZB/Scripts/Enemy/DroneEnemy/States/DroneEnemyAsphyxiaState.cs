using UnityEngine;

public class DroneEnemyAsphyxiaState : EnemyState<DroneEnemyStateEnum> {
    public DroneEnemyAsphyxiaState(Enemy enemy, EnemyStateMachine<DroneEnemyStateEnum> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter() {
        base.Enter();

        enemy.StartDelayCallback(1, () => stateMachine.ChangeState(DroneEnemyStateEnum.Battle));
    }
}