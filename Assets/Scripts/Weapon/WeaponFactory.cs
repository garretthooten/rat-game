//using System;
using UnityEngine;

public class WeaponFactory
{
    public static WeaponInstance Generate(WeaponData data, WeaponRarity rarity)
    {
        WeaponInstance instance = new WeaponInstance();

        float magCapacityMultipler = Random.Range(rarity.magCapacityMultiplerMin, rarity.magCapacityMultiplerMax);
        float fireRateMultiplier = Random.Range(rarity.fireRateMultiplierMin, rarity.fireRateMultiplerMax);
        float spreadMultipler = Random.Range(rarity.spreadMultiplerMin, rarity.spreadMultiplerMax);

        instance.ammoType = data.ammoType;
        instance.magCapacity = (int)(data.magCapacity * magCapacityMultipler);
        instance.fireRate = (data.fireRate * fireRateMultiplier);
        instance.spread = (data.spread * spreadMultipler);
        instance.weaponName = $"{rarity.name} {data.name}";
        instance.color = rarity.color;
        instance.rarity = rarity.name;

        return instance;
    }
}
