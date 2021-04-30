using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    private GameObject currentWeaponObject;

    public List<GameObject> weapons = new List<GameObject>();
    private List<GameObject> spawnedWeapons = new List<GameObject>();
    public Transform gunsTransform;

    private IGun currentWeapon;
    int currentWeaponIndex = 0;
    // Start is called before the first frame update
    private void Awake()
    {
        foreach (GameObject obj in weapons)
        {
            GameObject go = Instantiate(obj, gunsTransform);
            spawnedWeapons.Add(go);
            go.SetActive(false);
        }
        SpawnWeapon(spawnedWeapons[0]);
    }

    public void OnShoot()
    {
        currentWeapon.Shoot();
    }

    public void OnUnShoot()
    {
        currentWeapon.UnShoot();
    }

    public void OnChangeWeapon()
    {
        if(currentWeaponIndex < weapons.Count - 1)
        {
            currentWeaponIndex++;
        }
        else
        {
            currentWeaponIndex = 0;
        }
        SpawnWeapon(spawnedWeapons[currentWeaponIndex]);
    }

    void SpawnWeapon(GameObject obj)
    {
        if (currentWeaponObject)
        {
            currentWeaponObject.SetActive(false); 
        }
        currentWeapon = obj.GetComponent<IGun>();
        currentWeapon.SwitchIn();
        currentWeaponObject = obj;
        currentWeaponObject.SetActive(true);
    }
}
