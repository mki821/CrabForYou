using UnityEngine;

public class RangeEnemyDeadState : EnemyState<RangeEnemyStateEnum> {
    public RangeEnemyDeadState(Enemy enemy, EnemyStateMachine<RangeEnemyStateEnum> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
}