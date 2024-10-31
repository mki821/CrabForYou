using UnityEngine;

public class DroneEnemyDeadState : EnemyState<DroneEnemyStateEnum> {
    public DroneEnemyDeadState(Enemy enemy, EnemyStateMachine<DroneEnemyStateEnum> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
}