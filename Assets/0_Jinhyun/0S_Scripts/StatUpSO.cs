using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatUpSO", menuName = "SO/StatUpSO")]
public class StatUpSO : ScriptableObject
{
    public int speed;
    public int health;
    public int attackSpeed;
    public int attackRange;
    public int strength;
}
