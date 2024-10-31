using UnityEngine;

public class LaserEnemyCatchedState : EnemyState<LaserEnemyStateEnum> {
    public LaserEnemyCatchedState(Enemy enemy, EnemyStateMachine<LaserEnemyStateEnum> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
}