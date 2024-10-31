
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum CastingType
{
    Box,
    Circle,
    None
}

public class DamageCaster : MonoBehaviour
{
    [SerializeField] private LayerMask _whatIsTarget;

    public void Cast(int damage, Vector2 position, Vector2 size, float angle, float radius, CastingType type)
    {
        Debug.Log($"Casting by {type}");
        switch (type)
        {
            case CastingType.Circle:
                CastBox(damage, position, size, angle);
                break;
            case CastingType.Box:
                CastCircle(damage, position, radius);
                break;
        }
    }

    void CastBox(int damage, Vector2 position, Vector2 range, float angle)
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll(position, range, angle, _whatIsTarget);

        foreach (Collider2D col in cols)
        {
            //if (col.TryGetComponent(out IDamageable target))
            //{
            //    Debug.Log($"{col.gameObject.name} hit");
            //    target.ApplyDamage(damage, transform);
            //}
        }
    }
    void CastCircle(int damage, Vector2 position, float radius)
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(position, radius, _whatIsTarget);

        foreach (Collider2D col in cols)
        {
            //if (col.TryGetComponent(out IDamageable target))
            //{
            //    Debug.Log($"{col.gameObject.name} hit");
            //    target.ApplyDamage(damage, transform);
            //}
        }
    }
}
