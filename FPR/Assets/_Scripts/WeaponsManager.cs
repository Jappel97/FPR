using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsManager : MonoBehaviour
{

    public GameObject weaponHolder;
    private CharacterSheet myStats;
    
    // Start is called before the first frame update
    void Start()
    {
        myStats = this.GetComponent<CharacterSheet>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void swapWeapon()
    {
        weaponHolder.GetComponent<WeaponSwap>().upgradeSword();
        myStats.minPower = weaponHolder.GetComponent<WeaponSwap>().getLow();
        myStats.maxPower = weaponHolder.GetComponent<WeaponSwap>().getHigh();
    }
}
