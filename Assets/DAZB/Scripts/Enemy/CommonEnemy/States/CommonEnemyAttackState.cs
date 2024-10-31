using UnityEngine;

public class CommonEnemyAttackState : EnemyState<CommonEnemyStateEnum> {
    public CommonEnemyAttackState(Enemy enemy, EnemyStateMachine<CommonEnemyStateEnum> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
}