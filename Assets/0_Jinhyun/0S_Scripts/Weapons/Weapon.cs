using UnityEngine;

public abstract class Weapon : MonoBehaviour
{

    #region Weapon Info
    [SerializeField]
    protected int _damage;
    [SerializeField, HideInInspector]
    private float _lastAttackTime;
    [SerializeField] 
    private float _attackDelay;
    #endregion

    #region Casting Infos
    [SerializeField]
    private Vector2 _castPos;
    [SerializeField]
    private Vector2 _castSize;
    [SerializeField]
    private float _castAngle;
    [SerializeField]
    private float _castRadius;
    [SerializeField]
    protected CastingType castType = CastingType.None;
    #endregion

    public DamageCaster damageCaster;

    public virtual void Attack(Entity owner)
    {
        if (_lastAttackTime < _attackDelay + Time.time)
        {
            _lastAttackTime = Time.time;
            Vector2 curPos = (Vector2)transform.position;
            damageCaster.Cast(_damage, curPos + _castPos, _castSize, _castAngle, _castRadius, castType);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if(castType == CastingType.Circle)
        {
            Gizmos.DrawWireSphere((Vector2)transform.position+_castPos, _castRadius);
        }
        else if (castType == CastingType.Box)
        {
            Gizmos.DrawWireCube((Vector2)transform.position + _castPos, _castSize);
        }
    }
}
