using System.Collections;
using UnityEngine;

public class BossPattern3State : EnemyState<BossStateEnum> {
    private Boss boss;

    public BossPattern3State(Enemy enemy, EnemyStateMachine<BossStateEnum> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        boss = enemy as Boss;
    }

    Transform playerTrm;

    public override void Enter() {
        base.Enter();

        playerTrm = PlayerManager.Instance.Player.transform;

        boss.lineRendererCompo.enabled = true;

        enemy.StartCoroutine(AttackRoutine());
    }

    public override void Exit() {
        boss.lineRendererCompo.enabled = false;

        boss.lastPattenTime = Time.time;
        base.Exit();
    }

    private IEnumerator AttackRoutine() {
        float elapseTime = 0;
        float targetTime = 2f;

        Vector2 direction = (playerTrm.position - boss.transform.position).normalized;
        Vector2 rotatedDirection1 = Vector2.zero, rotatedDirection2 = Vector2.zero;

        // 초기 LineRenderer 설정
        boss.lineRendererCompo.startWidth = 0.2f;
        boss.lineRendererCompo.endWidth = 0.2f;
        boss.lineRendererCompo.positionCount = 4;

        // 눈에서 플레이어 위치로 LineRenderer 초기화
        boss.lineRendererCompo.SetPosition(0, boss.leftEyeTrm.position);
        boss.lineRendererCompo.SetPosition(1, playerTrm.position);
        boss.lineRendererCompo.SetPosition(2, boss.rightEyeTrm.position);
        boss.lineRendererCompo.SetPosition(3, playerTrm.position);

        yield return new WaitForSeconds(2f);

        // 회전 각도 설정 루프
        while (elapseTime < targetTime) {
            float t = easeOutCirc(elapseTime / targetTime);
            float angle = Mathf.Lerp(0, 45, t);

            // 플레이어 방향을 기준으로 회전 각도를 적용
            rotatedDirection1 = (Vector2)playerTrm.position + Rotate(direction, angle) * 1000f;
            rotatedDirection2 = (Vector2)playerTrm.position + Rotate(direction, -angle) * 1000f;

            boss.lineRendererCompo.SetPosition(0, boss.leftEyeTrm.position);
            boss.lineRendererCompo.SetPosition(1, rotatedDirection1);
            boss.lineRendererCompo.SetPosition(2, boss.rightEyeTrm.position);
            boss.lineRendererCompo.SetPosition(3, rotatedDirection2);

            elapseTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1f);


        // 빔 너비 감소 루프
        elapseTime = 0;
        targetTime = 0.2f;

        while (elapseTime < targetTime) {
            float t = easeInCirc(elapseTime / targetTime);
            float width = Mathf.Lerp(0.2f, 0f, t);
            boss.lineRendererCompo.startWidth = width;
            boss.lineRendererCompo.endWidth = width;

            elapseTime += Time.deltaTime;
            yield return null;
        }

        // 상태 변경
        stateMachine.ChangeState(BossStateEnum.WaitPattern);
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