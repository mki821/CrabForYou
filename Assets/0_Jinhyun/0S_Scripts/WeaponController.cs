using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
public class WeaponController : MonoBehaviour
{
    public Transform weaponParent;

    public Transform firePos;
    public List<Transform> dots = new(); 

    private Vector2 firstPos;
    private Vector2 targetPos;

    private Player player;
    private Coroutine curRoutine;


    private void Awake()
    {
        weaponParent = transform.parent;
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
        print("��������");
        if (curRoutine != null)
            return;
        curRoutine = StartCoroutine(AttackToMousePos());
        print("��������2");
    }

    private IEnumerator AttackToMousePos()
    {
        transform.parent = null;
        (player as Entity).CanFlipControl = false;
        float atkRange = player.Stat.attackRange.GetValue();
        float atkSpeed = player.Stat.attackSpeed.GetValue();
        Vector2 firstPos = transform.position;
        Vector2 targetPos = player.Input.MouseWorldPos;
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

            int idx = Mathf.Clamp((int)(easedT * 20), 0, dots.Count - 1);
            GameObject dot = dots[idx].gameObject;
            if (!dot.activeSelf)
            {
                dot.SetActive(true);
                dot.transform.position = Vector2.Lerp(transform.position, targetPos, easedT);
            }


            if (Vector2.Distance(transform.position, targetPos) <= 0.1f)
            {
                float delta = 0;
                float targetTime = 0.1f;

                for (int i = 0; i < 3; i++)
                {
                    //SoundManager.Instance.PlayOneShot("DRILLWIING.wav");
                    player.Weapon.Attack(player);
                    while (delta < targetTime)
                    {
                        delta += Time.deltaTime;
                        yield return null;
                    }
                    delta = 0;
                }

                curRoutine = StartCoroutine(ReturnOriginPos());
                yield break;
            }
            yield return null;
        }
    }

    private IEnumerator ReturnOriginPos()
    {
        transform.parent = weaponParent;
        float atkRange = player.Stat.attackRange.GetValue();
        float atkSpeed = player.Stat.attackSpeed.GetValue();

        float elapsedTime = 0f;
        float duration = 1f / atkRange;

        while(Vector2.Distance(transform.position, firePos.position) >= 0.1f)
        {
            float t = elapsedTime / duration;
            float easedT = EaseOutQuart(t);

            foreach (Transform dot in dots)
            {
                if (dot.gameObject.activeSelf)
                    dot.gameObject.SetActive(false);
                else continue;
            }

            transform.position = Vector2.Lerp(transform.position, firePos.position, easedT);
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        transform.position = firePos.position;
        (player as Entity).CanFlipControl = true;
        curRoutine = null;
    }

    

    private float EaseOutQuart(float t)
    {
        return 1 - Mathf.Pow(1 - t, 4);
    }
}
