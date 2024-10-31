using UnityEngine;

public class CommonEnemyDeadState : EnemyState<CommonEnemyStateEnum> {
    public CommonEnemyDeadState(Enemy enemy, EnemyStateMachine<CommonEnemyStateEnum> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
}