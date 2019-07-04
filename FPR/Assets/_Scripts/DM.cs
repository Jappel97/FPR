using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DM : NetworkBehaviour
{
    NetworkManager nm;
    private bool turnFinished;
    private int tempCounter = 0;
    private Dictionary<GameObject, string> playerTurn;
    private List<GameObject> finishedPlayers;
    private int AIReady = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        nm = this.gameObject.GetComponentInParent<NetworkManager>();
        playerTurn = new Dictionary<GameObject, string>();
        finishedPlayers = new List<GameObject>();
        turnFinished = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Check to see if the turn is done. If so, clean up to prep for the next turn, otherwise it's the first turn.
        if (turnFinished)
        {
            turnFinished = false;
            AIReady = 0;
        }
        else
        {
            //Query each player for input, and then add their input to the turn queue.
            foreach (GameObject P in Singleton.Players)
            {
                if (P.GetComponent<NetworkIdentity>().isLocalPlayer && !InQueue(P))
                {
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        P.GetComponent<PlayerActions>().MOVE.StartMove();
                        AddTurn(P, "Move");
                    }
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (isServer)
        {
            if(nm.numPlayers == playerTurn.Count)
            {
                //Since we have a queue as long as the number of players, that means each player has input for their turn.
                //Now we need to process the queue.
                //When each turn returns a finish command, we can then process the AI and move to the next turn.
                foreach(GameObject P in playerTurn.Keys)
                {
                    switch (playerTurn[P])
                    {
                        case "Move":
                            if (!P.GetComponent<PlayerActions>().MOVE.DoMove())
                            {
                                finishedPlayers.Add(P);
                                AIReady++;
                            }
                            break;
                        default:
                            break;
                    }
                }
                CountTurns();
                if(AIReady == nm.numPlayers)
                {
                    //This means both players have had their turns processed, and it's time for the AI to go.
                    //DO STUFF
                    turnFinished = true;
                }
            }
        }
    }

    private bool InQueue(GameObject P)
    {
        return playerTurn.ContainsKey(P);
    }

    private void CountTurns()
    {
        foreach(GameObject P in finishedPlayers)
        {
            playerTurn.Remove(P);
        }
        if(playerTurn.Count == 0)
        {
            finishedPlayers.Clear();
        }
    }

    private void AddTurn(GameObject p, string v)
    {
        if (playerTurn.ContainsKey(p))
        {
            playerTurn[p] = v;
        }
        else
        {
            playerTurn.Add(p, v);
        }
    }
}
