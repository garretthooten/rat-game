using UnityEditor.ShaderGraph.Internal;
using UnityEngine;


[CreateAssetMenu(fileName = "WeaponRarity", menuName = "ScriptableObjects/WeaponRarity")]
public class WeaponRarity : ScriptableObject
{
    public string name;
    public Color color;
    public float damageMultiplerMin;
    public float damageMultiplierMax;
    public float fireRateMultiplierMin;
    public float fireRateMultiplerMax;
    public float spreadMultiplerMin;
    public float spreadMultiplerMax;
    public float magCapacityMultiplerMin;
    public float magCapacityMultiplerMax;
}
