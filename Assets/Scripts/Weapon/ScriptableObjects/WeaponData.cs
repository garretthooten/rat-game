using UnityEngine;

public enum Firetype
{
    FullyAutomatic, SemiAutomatic
}

public enum AmmoType
{
    Light, Medium, Heavy, Shotgun
}

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string displayName;
    public float damagePerBullet;
    public float fireRate;
    public float spread;
    public int magCapacity;
    public Firetype fireType;
    public AmmoType ammoType;
}
