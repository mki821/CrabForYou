using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Stat/Player")]
public class PlayerStat : ScriptableObject
{
    public Stat maxHealth;
    public Stat moveSpeed;
    public Stat jumpForce;
    public Stat attack;
    public Stat attackSpeed;
    public Stat attackRange;

    protected Dictionary<StatType, Stat> _statDictionary;

    public float MaxHealth => maxHealth.GetValue();
    public float MoveSpeed => moveSpeed.GetValue();
    public float JumpForce => jumpForce.GetValue();
    public float Attack => attack.GetValue();
    public float AttackSpeed => attackSpeed.GetValue();
    public float AttackRange => attackRange.GetValue();

    private void OnEnable() {
        _statDictionary = new Dictionary<StatType, Stat>();

        Type playerStatType = typeof(PlayerStat);

        foreach(StatType statType in Enum.GetValues(typeof(StatType))) {
            string fieldName = LowerFirstChar(statType.ToString());

            try {
                FieldInfo playerStatField = playerStatType.GetField(fieldName);
                Stat stat = playerStatField.GetValue(this) as Stat;

                _statDictionary.Add(statType, stat);
            }
            catch {
                Debug.Log($"There are no Stat Field in Player : {fieldName}");
            }
        }
    }

    public Stat GetStatByType(StatType statType) {
        return _statDictionary[statType];
    }

    private string LowerFirstChar(string input) {
        return char.ToLower(input[0]) + input.Substring(1);
    }
}
