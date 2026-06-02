using UnityEngine;

public class WeaponInstance : MonoBehaviour
{
    public AmmoType ammoType;
    public int magCapacity;
    public int currentAmmo;
    public float fireRate;
    public float spread;
    public string weaponName;
    public Color color;
    public string rarity;

    public void Generate(WeaponData data, WeaponRarity weaponRarity)
    {
        //WeaponInstance instance = new WeaponInstance();

        float magCapacityMultipler = Random.Range(weaponRarity.magCapacityMultiplerMin, weaponRarity.magCapacityMultiplerMax);
        float fireRateMultiplier = Random.Range(weaponRarity.fireRateMultiplierMin, weaponRarity.fireRateMultiplerMax);
        float spreadMultipler = Random.Range(weaponRarity.spreadMultiplerMin, weaponRarity.spreadMultiplerMax);

        ammoType = data.ammoType;
        magCapacity = (int)(data.magCapacity * magCapacityMultipler);
        fireRate = (data.fireRate * fireRateMultiplier);
        spread = (data.spread * spreadMultipler);
        weaponName = $"{weaponRarity.name} {data.displayName}";
        color = weaponRarity.color;
        rarity = weaponRarity.name;

    }
}
