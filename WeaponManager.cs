using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour  {

    public static WeaponManager Instance { get; private set; }

    public WeaponInfo[] weaponSOArray;
    private Dictionary<string, WeaponInfo> weaponSOs;

    private void Awake() {

        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }

        weaponSOs = new Dictionary<string, WeaponInfo>();

        foreach (var weaponSO in weaponSOArray) {
            weaponSOs.Add(weaponSO.WeaponName, weaponSO);
        }

        weaponSOArray = null;
    }

    public GameObject GetWeapon(string weaponName) {

        if (weaponSOs.ContainsKey(weaponName)) {
            return weaponSOs[weaponName].Prefab;
        }
        else {
            Debug.LogError("Weapon not found: " + weaponName);
            return null;
        }
    }
}