using System.Collections;
using UnityEngine;

public class LaserEnemyAttackState : EnemyState<LaserEnemyStateEnum> {
    LaserEnemy enemy;

    public LaserEnemyAttackState(Enemy enemy, EnemyStateMachine<LaserEnemyStateEnum> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        this.enemy = enemy as LaserEnemy;
    }

    Transform playerTrm;

    public override void Enter() {
        base.Enter();

        playerTrm = PlayerManager.Instance.Player.transform;

        enemy.lineRendererCompo.enabled = true;

        enemy.StartCoroutine(AttackRoutine());
    }

    public override void Exit() {
        enemy.lineRendererCompo.enabled = false;
        enemy.lastAttackTime = Time.time;

        base.Exit();
    }

    public override void UpdateState() {
        base.UpdateState();
    }

    private IEnumerator AttackRoutine() {
        float elapseTime = 0;
        float targetTime = 1;

        Vector2 direction = playerTrm.position - enemy.transform.position;

        Vector2 rotatedDirection1;
        Vector2 rotatedDirection2;

        enemy.lineRendererCompo.startWidth = 0.05f;
        enemy.lineRendererCompo.endWidth = 0.05f;

        float angle;

        while (elapseTime < targetTime) {
            float t = easeOutCirc(elapseTime / targetTime);
            angle = Mathf.Lerp(60 , 0, t);

            rotatedDirection1 = enemy.transform.position + (Vector3)Rotate(direction.normalized * 1000f, angle);
            rotatedDirection2 = enemy.transform.position + (Vector3)Rotate(direction.normalized * 1000f, -angle);

            // LineRenderer에 4개의 포인트 설정
            enemy.lineRendererCompo.positionCount = 4;
            enemy.lineRendererCompo.SetPosition(0, enemy.transform.position);
            enemy.lineRendererCompo.SetPosition(1, rotatedDirection1);
            enemy.lineRendererCompo.SetPosition(2, enemy.transform.position);
            enemy.lineRendererCompo.SetPosition(3, rotatedDirection2);

            elapseTime += Time.deltaTime;

            yield return null;
        }

        elapseTime = 0;
        targetTime = 0.1f;

        enemy.lineRendererCompo.positionCount = 2;
        enemy.lineRendererCompo.SetPosition(0, enemy.transform.position);
        enemy.lineRendererCompo.SetPosition(1, direction * 1000f);

        float width;

        while (elapseTime < targetTime) {
            float t = easeOutCirc(elapseTime / targetTime);
            width = Mathf.Lerp(0.1f, 0.5f, t);
            enemy.lineRendererCompo.startWidth = width;
            enemy.lineRendererCompo.endWidth = width;

            elapseTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(3f);

        
        elapseTime = 0;
        targetTime = 0.2f;

        while (elapseTime < targetTime) {
            float t = easeInCirc(elapseTime / targetTime);
            width = Mathf.Lerp(0.5f, 0.1f, t);
            enemy.lineRendererCompo.startWidth = width;
            enemy.lineRendererCompo.endWidth = width;

            elapseTime += Time.deltaTime;
            yield return null;
        }

        stateMachine.ChangeState(LaserEnemyStateEnum.Battle);
    }

    // Vector2에 각도를 추가하는 Rotate 함수
    private Vector2 Rotate(Vector2 vector, float angle) {
        float radian = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radian);
        float sin = Mathf.Sin(radian);

        return new Vector2(
            cos * vector.x - sin * vector.y,
            sin * vector.x + cos * vector.y
        );
    }

    private float easeInCirc(float x) {
        return 1 - Mathf.Sqrt(1 - Mathf.Pow(x, 2));
    }

    private float easeOutCirc(float x) {
        return Mathf.Sqrt(1 - Mathf.Pow(x - 1, 2));
    }
}