using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class BossWaitPatternState : EnemyState<BossStateEnum> {
    private Boss boss;

    public BossWaitPatternState(Enemy enemy, EnemyStateMachine<BossStateEnum> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        boss = enemy as Boss;
    }

    private Vector3 movePosition;
    private int newXPos;
    public override void Enter() {
        base.Enter();
        newXPos = Mathf.Clamp(Random.Range(-8, 9), -8, 8);
        movePosition = new Vector3(newXPos, boss.transform.position.y);
        enemy.StartCoroutine(MoveRoutine());
    }

private IEnumerator MoveRoutine() {

    while (true) {
        if (Vector2.Distance(boss.transform.position, movePosition) <= 0.1f) {
            newXPos =Random.Range(-8, 9);
            movePosition = new Vector3(newXPos, boss.transform.position.y);
        }
        else {
            Move();
        }

        if (boss.CanPatternStart()) {
             boss.StopImmediately(false);
            float rand = Random.Range(0, 100);

            if (rand <= 50) {
                stateMachine.ChangeState(BossStateEnum.Pattern1);
                yield break;
            } else if (rand > 50 && rand < 70) {
                stateMachine.ChangeState(BossStateEnum.Pattern2);
                yield break;
            } else if (rand >= 70) {
                stateMachine.ChangeState(BossStateEnum.Pattern3);
                yield break;
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