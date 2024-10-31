using System.Collections;
using UnityEngine;

public class BossPattern1State : EnemyState<BossStateEnum> {
    private Boss boss;

    public BossPattern1State(Enemy enemy, EnemyStateMachine<BossStateEnum> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        boss = enemy as Boss;
    }

    private Transform playerTrm;

    public override void Enter() {
        base.Enter();
        playerTrm = PlayerManager.Instance.Player.transform;

        enemy.StartCoroutine(PatternRoutine());
    }

    public override void Exit() {
        boss.lastPattenTime = Time.time;

        base.Exit();
    }

    private IEnumerator PatternRoutine() {
        float elapseTime = 0;
        float targetTime = 0.5f;

        int rand = Random.Range(1, 3); // 1이면 왼손 2면 오른손

        Debug.Log(rand);

        Transform hand = rand == 1 ? boss.leftHandTrm : boss.rightHandTrm;
        Transform targetTrm = rand == 1 ? boss.leftHandAttackTrm : boss.rightHandAttackTrm;

        Vector2 originalPos = hand.position;

        Transform parentTrm = hand.parent;

        hand.SetParent(null);
        
        Vector2 startPos = hand.position;
        Vector2 endPos = targetTrm.position;

        while (elapseTime < targetTime) {
            float t = elapseTime / targetTime;
            hand.transform.position = Vector2.Lerp(startPos, endPos, t);

            elapseTime += Time.deltaTime;
            yield return null;
        }

        elapseTime = 0;
        targetTime = 2f;

         while (elapseTime < targetTime) {
            Vector3 targetPos;

            if (rand == 1) {
                targetPos = new Vector3(playerTrm.position.x, targetTrm.position.y, hand.position.z);
            } else {
                targetPos = new Vector3(targetTrm.position.x, playerTrm.position.y, hand.position.z);
            }
            hand.position = Vector3.Lerp(hand.position, targetPos, Time.deltaTime * 10f);

            elapseTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        elapseTime = 0;
        targetTime = 0.1f;

        if (rand == 1) {
            startPos = hand.position;
            endPos = new Vector2(hand.position.x, -6);
        }
        else if (rand == 2) {
            startPos = hand.position;
            endPos = new Vector2(-10, hand.position.y);
        }

        while (elapseTime < targetTime) {
            float t = elapseTime / targetTime;
            hand.transform.position = Vector2.Lerp(startPos, endPos, t);

            elapseTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        elapseTime = 0;
        targetTime = 0.5f;

        startPos = hand.position;
        endPos = originalPos;

        while (elapseTime < targetTime) {
            float t = elapseTime / targetTime;
            hand.transform.position = Vector2.Lerp(startPos, endPos, t);

            elapseTime += Time.deltaTime;
            yield return null;
        }


        hand.SetParent(parentTrm);

        stateMachine.ChangeState(BossStateEnum.WaitPattern);
    }
}