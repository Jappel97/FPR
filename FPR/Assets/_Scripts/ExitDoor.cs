using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ExitDoor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject winner;

        if (other.gameObject.tag == "player")
        {
            winner = other.gameObject;
        }
        else
        {
            winner = null;
        }

        if(winner != null)
        {
            Singleton.Players.Remove(other.gameObject);

            if (other.gameObject.GetComponent<NetworkIdentity>().isClient)
            {
                NetworkManager.singleton.StopClient();
            }
            else
            {
                NetworkManager.singleton.StopHost();
            }
        }
    }
}
