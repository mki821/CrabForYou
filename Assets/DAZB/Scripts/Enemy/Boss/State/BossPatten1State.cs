using UnityEngine;

public class BossPatten1State : EnemyState<BossStateEnum> {
    public BossPatten1State(Enemy enemy, EnemyStateMachine<BossStateEnum> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
}