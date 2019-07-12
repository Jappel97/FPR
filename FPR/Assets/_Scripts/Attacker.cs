using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    public MovementSensor sensor;
    private CharacterSheet myStats;
    
    // Start is called before the first frame update
    void Start()
    {
        myStats = this.gameObject.GetComponent<CharacterSheet>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int attackNPC()
    {
        System.Random rand = new System.Random();
        int damage = rand.Next(myStats.minPower, myStats.maxPower + 1);
        if (sensor.queryAdjacency())
        {
            GameObject myTarget = sensor.getTarget();
            if(myTarget != null)
            {
                damage = (damage - myTarget.GetComponent<CharacterSheet>().Defense) < 0 ? 0 : damage - myTarget.GetComponent<CharacterSheet>().Defense;
                myTarget.GetComponent<CharacterSheet>().Health -= damage;
                return myTarget.GetComponent<CharacterSheet>().Health;
            }
        }
        return -255;
    }

    public int attackPlayer()
    {
        Debug.Log("I'm a player and I'm attacking");
        System.Random rand = new System.Random();
        int damage = rand.Next(myStats.minPower, myStats.maxPower + 1);
        if (sensor.queryAdjacency())
        {
            Debug.Log("I'm adjacent");
            GameObject myTarget = sensor.getEnemy();
            if (myTarget != null)
            {
                Debug.Log("I'm attacking this guy");
                damage = (damage - myTarget.GetComponent<CharacterSheet>().Defense) < 0 ? 0 : damage - myTarget.GetComponent<CharacterSheet>().Defense;
                myTarget.GetComponent<CharacterSheet>().Health -= damage;
                myTarget.GetComponent<CharacterSheet>().setKiller(this.gameObject);
                return myTarget.GetComponent<CharacterSheet>().Health;
            }
        }
        return -255;
    }
}
