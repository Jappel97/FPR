using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerActions : NetworkBehaviour
{
    public PlayerMovement MOVE;

    
    // Start is called before the first frame update
    void Start()
    {
        MOVE = this.gameObject.GetComponent<PlayerMovement>();
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
}
