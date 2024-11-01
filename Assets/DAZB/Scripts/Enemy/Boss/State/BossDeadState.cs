using UnityEngine;

public class BossDeadState : EnemyState<BossStateEnum> {
    private Boss boss;

    public BossDeadState(Enemy enemy, EnemyStateMachine<BossStateEnum> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
         boss = enemy as Boss;
    }

    public override void Enter() {
        base.Enter();

        Particle go = enemy.CreateObject(boss.deadParticle);
        go.transform.position = enemy.transform.position;
        go.Destroy(1);

        boss.gameObject.SetActive(false);
    }
}