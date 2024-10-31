using UnityEngine;

public class LaserEnemyDeadState : EnemyState<LaserEnemyStateEnum> {
    public LaserEnemyDeadState(Enemy enemy, EnemyStateMachine<LaserEnemyStateEnum> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
}