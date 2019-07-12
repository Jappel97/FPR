using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerActions : NetworkBehaviour
{
    public PlayerMovement MOVE;
    public Attacker HIT;
    public GameObject playerHolder, weaponHolder;
    public GameObject[] weapons;
    private Camera myCam;
    public MouseLook[] mouseLookers;
    
    // Start is called before the first frame update
    void Start()
    {
        MOVE = this.gameObject.GetComponent<PlayerMovement>();
        HIT = this.gameObject.GetComponent<Attacker>();

        if (isLocalPlayer)
        {
            playerHolder.layer = LayerMask.NameToLayer("Ally");
            foreach(Transform child in playerHolder.transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("Ally");
            }
            for(int i = 0; i < weapons.Length; i++)
            {
                weapons[i].layer = LayerMask.NameToLayer("Ally");
            }

            weaponHolder.layer = LayerMask.NameToLayer("Player");
        }
        if (!isLocalPlayer)
        {
            playerHolder.layer = LayerMask.NameToLayer("Player");
            foreach (Transform child in playerHolder.transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("Player");
            }
            for (int i = 0; i < weapons.Length; i++)
            {
                weapons[i].layer = LayerMask.NameToLayer("Player");
            }

            weaponHolder.layer = LayerMask.NameToLayer("Ally");
            foreach (Transform C in weaponHolder.transform)
            {
                C.gameObject.layer = LayerMask.NameToLayer("Ally");
            }

            for (int i = 0; i < mouseLookers.Length; i++)
            {
                mouseLookers[i].enabled = false;
            }
            transform.Find("PlayerView").Find("PlayerCamera").gameObject.SetActive(false);
        }
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        Singleton.Players.Add(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string getBehavior()
    {
        if (isLocalPlayer)
        {
            string action = "";
            if (Input.GetKeyDown(KeyCode.W))
            {
                action = "Move";
            }
            else if (Input.GetKeyDown(KeyCode.P))
            {
                action = "Wait";
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject.Instantiate(weaponHolder.GetComponent<EffectViewer>().slashEffect, weaponHolder.transform.position, weaponHolder.transform.rotation);
                action = "Attack";
            }
            if (this.gameObject.GetComponent<CharacterSheet>().Health < 0)
            {
                action = "Dead";
            }
            return action;
        }
        return "";
    }
}
