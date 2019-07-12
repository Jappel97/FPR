using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBehavior : Behavior
{

    private bool seePlayer;
    private bool nextToPlayer;
    public GameObject peopleFinder;
    public GameObject sensorBar;

    // Start is called before the first frame update
    void Start()
    {
        mover = GetComponent<MoveCore>();
        hitter = GetComponent<Attacker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Singleton.Players.Count > 0)
        {
            this.transform.LookAt(nearestPlayer().transform);
            this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x, this.transform.rotation.eulerAngles.y, this.transform.rotation.eulerAngles.z);
        }
    }

    public override string FindBehavior()
    {
        string myBehavior = "WAIT";

        seePlayer = peopleFinder.GetComponent<PeopleFinderBehavior>().QuerySensory();
        nextToPlayer = sensorBar.GetComponent<MovementSensor>().queryAdjacency();
        if (seePlayer)
        {
            myBehavior = "MOVE";
        }
        if (nextToPlayer)
        {
            myBehavior = "ATTACK";
        }
        if (this.gameObject.GetComponent<CharacterSheet>().Health < 0)
        {
            myBehavior = "DEAD";
        }

        return myBehavior;
    }

    private GameObject nearestPlayer()
    {
        GameObject Player = null;
        foreach (GameObject P in Singleton.Players)
        {
            if (Player == null)
            {
                Player = P;
            }
            else
            {
                Player = (Distance((int)this.gameObject.transform.position.x, (int)this.gameObject.transform.position.z, (int)Player.transform.position.x, (int)Player.transform.position.z)
                    > Distance((int)this.gameObject.transform.position.x, (int)this.gameObject.transform.position.z, (int)P.transform.position.x, (int)P.transform.position.z) ? P : Player);
            }
        }
        return Player;
    }

    private double Distance(int x1, int y1, int x2, int y2)
    {
        double d = 0.0;
        int a = x2 - x1;
        int b = y2 - y1;
        a = a * a;
        b = b * b;
        d = a + b;
        return d;
    }
}
