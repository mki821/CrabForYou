using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Objects/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    public WeaponType weaponType;
    public int _damage; 
    public float _lastAttackTime;
    public float _attackDelay;
    public Vector2 _castPos;
    public Vector2 _castSize;
    public float _castAngle;
    public float _castRadius;
    public CastingType castType = CastingType.None;
}
