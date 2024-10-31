using UnityEngine;

public class DashEnemyDeadState : EnemyState<DashEnemyStateEnum> {
    public DashEnemyDeadState(Enemy enemy, EnemyStateMachine<DashEnemyStateEnum> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
}