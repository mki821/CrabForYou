using UnityEngine;

public class BossPatten3State : EnemyState<BossStateEnum> {
    public BossPatten3State(Enemy enemy, EnemyStateMachine<BossStateEnum> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
}