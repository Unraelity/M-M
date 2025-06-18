using UnityEngine;

[CreateAssetMenu(menuName = "WeaponInfo")]
public class WeaponInfo : ScriptableObject
{
    [Tooltip("Name that will be displayed on the Weapon's Menu")]
    [SerializeField] private string weaponName;
    [Tooltip("Description that will be displayed on the Weapon's Menu")]
    [SerializeField] private string description;
    [Header("Bindings")]
    [SerializeField] private Sprite sprite;
    [SerializeField] private GameObject prefab;

    public string WeaponName => weaponName;
    public string Description => description;
    public Sprite Sprite => sprite;
    public GameObject Prefab => prefab;
}
