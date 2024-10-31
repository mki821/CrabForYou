using UnityEngine;

public class BossPatten2State : EnemyState<BossStateEnum> {
    public BossPatten2State(Enemy enemy, EnemyStateMachine<BossStateEnum> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
}