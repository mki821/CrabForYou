using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private float _baseValue;

    public List<float> modifiers;

    public float GetValue() {
        float finalValue = _baseValue;
        foreach(float value in modifiers) {
            finalValue += value;
        }

        return finalValue;
    }

    public void AddModifier(int value) {
        if(value != 0)
            modifiers.Add(value);
    }

    public void RemoveModifier(int value) {
        if(value != 0)
            modifiers.Remove(value);
    }

    public void SetDefaultValue(int value) {
        _baseValue = value;
    }
}
