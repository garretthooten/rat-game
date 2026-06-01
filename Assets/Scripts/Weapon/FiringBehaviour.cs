using UnityEngine;

public abstract class FiringBehaviour : MonoBehaviour
{
    private WeaponInstance _weaponInstance;

    public abstract void Fire();

    public abstract void StopFiring();

    protected virtual void Reload()
    {
        int availableAmmoInStash, ammoToLoad, leftoverAmmo;

        switch(_weaponInstance.ammoType)
        {
            case AmmoType.Light:
                availableAmmoInStash = PlayerCombat.Instance.lightAmmoCount;
                break;
            case AmmoType.Medium:
                availableAmmoInStash = PlayerCombat.Instance.mediumAmmoCount;
                break;
            case AmmoType.Heavy:
                availableAmmoInStash = PlayerCombat.Instance.heavyAmmoCount;
                break;
            case AmmoType.Shotgun:
                availableAmmoInStash = PlayerCombat.Instance.shotgunAmmoCount;
                break;
            default:
                Debug.LogError("Failed to get ammoType in weapon instance (1)");
                return;
        }

        if(availableAmmoInStash <= _weaponInstance.magCapacity)
        {
            ammoToLoad = availableAmmoInStash;
            leftoverAmmo = 0;
        }
        else
        {
            ammoToLoad = _weaponInstance.magCapacity;
            leftoverAmmo = availableAmmoInStash - ammoToLoad;
            if(leftoverAmmo <= 0) leftoverAmmo = 0;
        }

        _weaponInstance.currentAmmo = ammoToLoad;
        switch(_weaponInstance.ammoType)
        {
            case AmmoType.Light:
                PlayerCombat.Instance.lightAmmoCount = leftoverAmmo;
                break;
            case AmmoType.Medium:
                PlayerCombat.Instance.mediumAmmoCount = leftoverAmmo;
                break;
            case AmmoType.Heavy:
                PlayerCombat.Instance.heavyAmmoCount = leftoverAmmo;
                break;
            case AmmoType.Shotgun:
                PlayerCombat.Instance.shotgunAmmoCount = leftoverAmmo;
                break;
            default:
                Debug.LogError("Failed to get ammoType in weapon instance (2)");
                return;
        }
    }
}
