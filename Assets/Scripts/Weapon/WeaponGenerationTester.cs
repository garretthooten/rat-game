using Unity.VisualScripting;
using UnityEngine;

public class WeaponGenerationTester : MonoBehaviour
{
    public WeaponData[] weaponDatas;
    public WeaponRarity[] weaponRarities;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateRandomWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateRandomWeapon()
    {
        int weaponIndex = Random.Range(0, weaponDatas.Length);
        int rarityIndex = Random.Range(0, weaponRarities.Length);
        Debug.Log($"weaponIndex: {weaponIndex}, rarityIndex: {rarityIndex}");

        //WeaponInstance generatedInstance = WeaponFactory.Generate(weaponDatas[weaponIndex], weaponRarities[rarityIndex]);
        var existingInstance = GetComponent<WeaponInstance>();
        if(existingInstance != null)
            Destroy(existingInstance);
        WeaponInstance instance = this.AddComponent<WeaponInstance>();
        instance.Generate(weaponDatas[weaponIndex], weaponRarities[rarityIndex]);
    }
}
