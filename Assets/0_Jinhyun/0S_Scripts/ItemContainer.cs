using System;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    End
}

public enum WeaponType
{ 
}

public class ItemContainer : MonoSingleton<ItemContainer>
{
    public Weapon curWeapon;
    public List<Item> items;

    public Dictionary<WeaponType, Weapon> weaponDic= new Dictionary<WeaponType, Weapon>();
    public Dictionary<ItemType, Item> itemDic = new();

    protected override void Awake()
    {
        base.Awake();

        foreach (WeaponType type in Enum.GetValues(typeof(WeaponType)))
        {
            string typeName = type.ToString();
            Type t = Type.GetType($"{typeName}");

            try
            {
                var weapon = Activator.CreateInstance(t, this, typeName) as Weapon;
                weaponDic.Add(type, weapon);
            }
            catch
            {
                Debug.LogError($"{typeName}");
            }
        }

        foreach (ItemType type in Enum.GetValues(typeof(ItemType)))
        {
            string typeName = type.ToString();
            Type t = Type.GetType($"{typeName}");

            try
            {
                var item = Activator.CreateInstance(t, this, typeName) as Item;
                itemDic.Add(type, item);
            }
            catch
            {
                Debug.LogError($"{typeName}");
            }
        }
    }
}
