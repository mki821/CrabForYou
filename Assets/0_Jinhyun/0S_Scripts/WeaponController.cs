using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    public Transform firePos;

    private Vector2 firstPos;
    private Vector2 targetPos;

    private Player player;
    private Coroutine curRoutine;

    private void Awake()
    {
        player = transform.parent.GetComponent<Player>();
    }

    private void OnEnable()
    {
        player.Input.AttackEvent += Move;
    }

    private void OnDisable()
    {
        player.Input.AttackEvent -= Move;
    }

    public void Move()
    {
        print("けいしぉ");
        if (curRoutine != null)
            return;
        curRoutine = StartCoroutine(AttackToMousePos());
        print("けいしぉ2");
    }

    private IEnumerator AttackToMousePos()
    {
        float atkRange = player.Stat.attackRange.GetValue();
        float atkSpeed = player.Stat.attackSpeed.GetValue();
        Vector2 firstPos = transform.position;
        Vector2 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 moveDir = (targetPos - (Vector2)transform.position).normalized;
        transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg - 90);

        if (Vector2.Distance(firstPos, targetPos) > atkRange)
        {
            targetPos = firstPos + (targetPos - firstPos).normalized * atkRange;
        }

        float elapsedTime = 0f;
        float duration = 1f / atkSpeed;

        while(elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float easedT = EaseOutQuart(t);

            transform.position = Vector2.Lerp(transform.position, targetPos, easedT);
            elapsedTime += Time.deltaTime;

            if (Vector2.Distance(transform.position, targetPos) <= 0.1f)
            {
                curRoutine = StartCoroutine(ReturnOriginPos());
                yield break;
            }
            yield return null;
        }
    }

    private IEnumerator ReturnOriginPos()
    {
        float atkRange = player.Stat.attackRange.GetValue();
        float atkSpeed = player.Stat.attackSpeed.GetValue();

        float elapsedTime = 0f;
        float duration = 1f / atkRange;

        while(Vector2.Distance(transform.position, firePos.position) >= 0.1f)
        {
            float t = elapsedTime / duration;
            float easedT = EaseOutQuart(t);

            transform.position = Vector2.Lerp(transform.position, firePos.position, easedT);
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        transform.position = firePos.position;
        curRoutine = null;
    }

    

    private float EaseOutQuart(float t)
    {
        return 1 - Mathf.Pow(1 - t, 4);
    }
}
