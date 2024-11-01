using System.Collections;
using UnityEngine;

public class DroneEnemyAttackState : EnemyState<DroneEnemyStateEnum> {
    private DroneEnemy enemy;

    public DroneEnemyAttackState(Enemy enemy, EnemyStateMachine<DroneEnemyStateEnum> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        this.enemy = enemy as DroneEnemy;
    }

    private Transform playerTrm;

    private Coroutine coroutine;

    public override void Enter() {
        base.Enter();

        playerTrm = PlayerManager.Instance.Player.transform;

        enemy.StopImmediately(true);

        enemy.isAttack = true;
    
        coroutine = enemy.StartCoroutine(DashRoutine());
    }

    public override void Exit() {
        enemy.lastAttackTime = Time.time;

        enemy.isAttack = false;

        enemy.StopCoroutine(coroutine);

        base.Exit();
    }

    public override void UpdateState() {
        base.UpdateState();
    }

    private IEnumerator DashRoutine() {
        yield return new WaitForSeconds(0.5f);

        float elapseTime = 0;
        float targetTime = 0.7f;

        float t;
        Vector2 startPos = enemy.transform.position;

        Vector2 endPos = startPos + (Vector2)(playerTrm.position - enemy.transform.position).normalized * 8;

        while (elapseTime < targetTime) {
            t = easeOutExpo(elapseTime / targetTime);
            enemy.transform.position = Vector2.Lerp(startPos, endPos, t);
            elapseTime += Time.deltaTime;

            yield return null;
        }
        stateMachine.ChangeState(DroneEnemyStateEnum.Battle);
    }

    private float easeOutExpo(float x)  {
        return x == 1 ? 1 : 1 - Mathf.Pow(2, -10 * x);
    }
}