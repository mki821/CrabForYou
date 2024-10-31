using System.Collections;
using UnityEngine;

public class BossWaitPattenState : EnemyState<BossStateEnum> {
    private Boss boss;

    public BossWaitPattenState(Enemy enemy, EnemyStateMachine<BossStateEnum> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        boss = enemy as Boss;
    }

    private Vector3 movePosition;

    public override void Enter() {
        base.Enter();

        enemy.StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine() {
        while (true) {

            if (Vector2.Distance(boss.transform.position, movePosition) < 0.1f) {
                movePosition = new Vector3(Random.Range(-8, 9), 0);
            }
            else {
                Move();
            }

            if (boss.CanPattenStart()) {
                float rand = Random.Range(0, 100);
                if (rand <= 50) {
                    stateMachine.ChangeState(BossStateEnum.Patten1);
                }
                else if (rand > 50 && rand < 70) {
                    stateMachine.ChangeState(BossStateEnum.Patten2);
                }
                else if (rand >= 70) {
                    stateMachine.ChangeState(BossStateEnum.Patten3);
                }
            }

            yield return null;
        }
    }

    private void Move() {
        Vector2 dir = (movePosition - boss.transform.position).normalized;
        boss.SetVelocity(boss.moveSpeed * dir.x, boss.RigidbodyCompo.linearVelocityY);
    }

}