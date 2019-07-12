using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerStats : CharacterSheet
{
    private WeaponSwap weaponManager;

    // Start is called before the first frame update
    void Start()
    {
        Health = 100;
        Defense = 3;
        weaponManager = this.GetComponentInChildren<WeaponSwap>();
        minPower = 2;
        maxPower = 7;
        pAnim = this.GetComponent<PlayerAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Death()
    {
        Singleton.Players.Remove(this.gameObject);
        pAnim.Death();
        if (this.gameObject.GetComponent<NetworkIdentity>().isClient)
        {
            NetworkManager.singleton.StopClient();
        }
        else
        {
            NetworkManager.singleton.StopHost();
        }
    }

    public override void setKiller(GameObject killer)
    {
        return;
    }
}
