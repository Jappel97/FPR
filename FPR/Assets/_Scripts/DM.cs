using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DM : NetworkBehaviour
{
    NetworkManager nm;
    private bool turnFinished;
    //private int tempCounter = 0;
    private Dictionary<GameObject, string> playerTurn;
    private Dictionary<GameObject, string> enemyTurn;
    private List<GameObject> finishedPlayers;
    private List<GameObject> finishedEnemies;
    public GameObject Slime;
    public GameObject Robot;
    public GameObject armor;
    public GameObject weapon;
    private bool playersDone = false;
    private bool enemiesReady = false;
    private string currTurn;
    //Did you come here
    private GameObject toDie;
    //Nah mate, I came 'ere
    private GameObject YesterDie;
    
    // Start is called before the first frame update
    void Start()
    {
        nm = this.gameObject.GetComponentInParent<NetworkManager>();
        playerTurn = new Dictionary<GameObject, string>();
        enemyTurn = new Dictionary<GameObject, string>();
        finishedPlayers = new List<GameObject>();
        finishedEnemies = new List<GameObject>();
        turnFinished = false;
        PlaceEnemies(Slime, Robot);
        PlaceTreasure(armor, weapon);
    }

    private void PlaceTreasure(GameObject armor, GameObject weapon)
    {
        System.Random Rand = new System.Random();

        foreach (Room R in Singleton.Roomlist)
        {
            if (R.ID > 0 && R.ID < Singleton.Roomlist.Count - 1)
            {
                int myX = 2 * R.centerX;
                int myY = 2 * R.centerY;
                switch(Rand.Next() % 3)
                {
                    case 0:
                        Instantiate(armor, new Vector3(myX, 1, myY), Quaternion.identity);
                        break;
                    case 1:
                        Instantiate(weapon, new Vector3(myX, 1, myY), Quaternion.identity);
                        break;
                    case 2:
                        break;
                    default:
                        break;
                }
                
            }
        }
    }

    private void PlaceEnemies(GameObject Enemy1, GameObject Boss)
    {
        System.Random Rand = new System.Random();

        foreach (Room R in Singleton.Roomlist)
        {
            if(R.ID > 0 && R.ID < Singleton.Roomlist.Count - 1)
            {
                int myX = 2 * R.centerX;
                int myY = 2 * R.centerY;
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (i != 0 && j != 0 && Rand.Next()%2 == 0)
                        {
                            Singleton.Enemies.Add(Instantiate(Enemy1, new Vector3(myX + 2 * j, 0, myY + 2 * i), Quaternion.identity));
                        }
                    }
                }
            }
            else if(R.ID >= Singleton.Roomlist.Count - 1)
            {
                Singleton.Enemies.Add(Instantiate(Boss, new Vector3(R.centerX * 2, 0, R.centerY * 2), Quaternion.identity));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Check to see if the turn is done. If so, clean up to prep for the next turn, otherwise it's the first turn.
        if (turnFinished)
        {
            turnFinished = false;
            playersDone = false;
        }
        else
        {
            if (playersDone)
            {
                enemiesReady = true;
                foreach(GameObject E in Singleton.Enemies)
                {
                    if (!enemyTurn.ContainsKey(E) && isServer && !finishedEnemies.Contains(E))
                    {
                        currTurn = E.GetComponent<AIController>().GetAction();
                        if(currTurn == "MOVE")
                        {
                            if (E.name.Contains("Slime"))
                            {
                                E.GetComponent<AIController>().myBehavior.mover.StartMove(E.transform.rotation.eulerAngles.y - 90);
                            }
                            else
                            {
                                E.GetComponent<AIController>().myBehavior.mover.StartMove(E.transform.rotation.eulerAngles.y);
                            }
                            AddEnemyTurn(E, currTurn);
                        }
                        else if(currTurn == "WAIT")
                        {
                            AddEnemyTurn(E, currTurn);
                        }
                        else if(currTurn == "ATTACK")
                        {
                            AddEnemyTurn(E, currTurn);
                        }
                        else if(currTurn == "DEAD")
                        {
                            AddEnemyTurn(E, currTurn);
                        }
                        E.GetComponent<AnimationManager>().actionAnimator = currTurn;
                    }
                }
            }
            else
            {
                //Query each player for input, and then add their input to the turn queue.
                foreach (GameObject P in Singleton.Players)
                {
                    if (!InQueue(P) && !finishedPlayers.Contains(P))
                    {
                        switch (P.GetComponent<PlayerActions>().getBehavior())
                        {
                            case "Move":
                                P.GetComponent<PlayerActions>().MOVE.StartMove(P.transform.rotation.eulerAngles.y);
                                AddTurn(P, "Move");
                                P.GetComponent<AnimationManager>().actionAnimator = "MOVE";
                                break;
                            case "Wait":
                                AddTurn(P, "Wait");
                                P.GetComponent<AnimationManager>().actionAnimator = "WAIT";
                                break;
                            case "Attack":
                                AddTurn(P, "Attack");
                                P.GetComponent<AnimationManager>().actionAnimator = "ATTACK";
                                break;
                            case "Dead":
                                AddTurn(P, "Dead");
                                break;
                            case "":
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            
        }
    }

    private void LateUpdate()
    {
        if (isServer)
        {
            if (nm.numPlayers == playerTurn.Count)
            {
                //Since we have a queue as long as the number of players, that means each player has input for their turn.
                //Now we need to process the queue.
                //When each turn returns a finish command, we can then process the AI and move to the next turn.
                foreach (GameObject P in playerTurn.Keys)
                {
                    switch (playerTurn[P])
                    {
                        case "Move":
                            if (!P.GetComponent<PlayerActions>().MOVE.DoMove())
                            {
                                finishedPlayers.Add(P);
                                P.GetComponent<AnimationManager>().actionAnimator = "EMPTY";
                            }
                            break;
                        case "Wait":
                            finishedPlayers.Add(P);
                            P.GetComponent<AnimationManager>().actionAnimator = "EMPTY";
                            break;
                        case "Attack":
                            P.GetComponent<PlayerActions>().HIT.attackPlayer();
                            P.GetComponent<AnimationManager>().actionAnimator = "EMPTY";
                            finishedPlayers.Add(P);
                            break;
                        case "Dead":
                            toDie = P;
                            P.GetComponent<AnimationManager>().actionAnimator = "EMPTY";
                            finishedPlayers.Add(P);
                            break;
                        default:
                            P.GetComponent<AnimationManager>().actionAnimator = "EMPTY";
                            finishedPlayers.Add(P);
                            break;
                    }
                }
                if (finishedPlayers.Count == playerTurn.Count)
                {
                    CountTurns();
                }
            }
            if (enemiesReady)
            {
                Debug.Log("AI Ready");
                foreach(GameObject E in Singleton.Enemies)
                {
                    Debug.Log("Hey I'm getting ready to do my thing, my name is " + E.name);
                    if (enemyTurn.ContainsKey(E))
                    {
                        Debug.Log("I am " + E.name + "And I want to " + enemyTurn[E]);
                        switch (enemyTurn[E])
                        {
                            case "MOVE":
                                if (!E.GetComponent<AIController>().myBehavior.mover.DoMove())
                                {
                                    finishedEnemies.Add(E);
                                    E.GetComponent<AnimationManager>().actionAnimator = "EMPTY";
                                }
                                break;
                            case "WAIT":
                                finishedEnemies.Add(E);
                                E.GetComponent<AnimationManager>().actionAnimator = "EMPTY";
                                break;
                            case "ATTACK":
                                E.GetComponent<AIController>().myBehavior.hitter.attackNPC();
                                finishedEnemies.Add(E);
                                E.GetComponent<AnimationManager>().actionAnimator = "EMPTY";
                                break;
                            case "DEAD":
                                YesterDie = E;
                                finishedEnemies.Add(E);
                                break;
                            default:
                                finishedEnemies.Add(E);
                                E.GetComponent<AnimationManager>().actionAnimator = "EMPTY";
                                break;
                        }
                        
                    }
                }
                PassEnemy();
            }
        }
        //BringOutYourDead();
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
            playersDone = true;
            if (toDie != null)
            {
                toDie.GetComponent<CharacterSheet>().Death();
            }
        }
    }
    
    private void PassEnemy()
    {
        foreach(GameObject E in finishedEnemies)
        {
            enemyTurn.Remove(E);
        }
        if(enemyTurn.Count == 0)
        {
            finishedEnemies.Clear();
            turnFinished = true;
            enemiesReady = false;
            if (YesterDie != null)
            {
                YesterDie.GetComponent<CharacterSheet>().Death();
            }
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

    private void AddEnemyTurn(GameObject e, string v)
    {
        if (enemyTurn.ContainsKey(e))
        {
            enemyTurn[e] = v;
        }
        else
        {
            enemyTurn.Add(e, v);
        }
    }

    private void BringOutYourDead()
    {
        finishedPlayers.Remove(toDie);
        playerTurn.Remove(toDie);
        toDie.GetComponent<CharacterSheet>().Death();
        finishedEnemies.Remove(YesterDie);
        enemyTurn.Remove(YesterDie);
        YesterDie.GetComponent<CharacterSheet>().Death();
    }
}
