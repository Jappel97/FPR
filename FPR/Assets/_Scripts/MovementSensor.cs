using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSensor : MonoBehaviour
{
    public bool intersection = false;
    private bool personAdjacent = false;
    private GameObject target;
    public GameObject adjacentEnemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Sensor")
        {
            intersection = true;
        }
        if(other.gameObject.tag == "player")
        {
            Debug.Log("I see the player");
            personAdjacent = true;
            target = other.gameObject;
            intersection = true;
        }
        if(other.gameObject.tag == "Enemy")
        {
            personAdjacent = true;
            intersection = true;
            adjacentEnemy = other.gameObject;
        }
        if(other.gameObject.tag == "Treasure")
        {
            intersection = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Sensor")
        {
            intersection = false;
        }
        if(other.gameObject.tag == "player")
        {
            Debug.Log("I don't see the player");
            personAdjacent = false;
            target = null;
        }
        if(other.gameObject.tag == "Enemy")
        {
            adjacentEnemy = null;
        }
    }

    public bool queryAdjacency()
    {
        return personAdjacent;
    }

    public GameObject getTarget()
    {
        return target;
    }

    public GameObject getEnemy()
    {
        return adjacentEnemy;
    }
}
