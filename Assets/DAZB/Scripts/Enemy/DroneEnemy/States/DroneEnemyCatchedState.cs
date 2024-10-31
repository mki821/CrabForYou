using UnityEngine;

public class DroneEnemyCatchedState : EnemyState<DroneEnemyStateEnum> {
    public DroneEnemyCatchedState(Enemy enemy, EnemyStateMachine<DroneEnemyStateEnum> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
}