using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStats : CharacterSheet
{

    public GameObject loot1;
    public AnimationClip dying;
    private GameObject Killer;
    
    // Start is called before the first frame update
    void Start()
    {
        Health = 20;
        Defense = 1;
        minPower = 1;
        maxPower = 5;
        pAnim = this.GetComponent<PlayerAnimation>();
    }

    public override void Death()
    {
        pAnim.Death();
        System.Random rand = new System.Random();
        Singleton.Enemies.Remove(this.gameObject);
        Vector3 pos = this.transform.position;
        Quaternion rot = this.transform.rotation;
        GameObject.Destroy(this.gameObject, dying.length + 0.5f);
        if (rand.Next() % 2 == 0)
        {
            Instantiate(loot1, pos + new Vector3(0, 1, 0), rot);
        }
        if (Killer != null)
        {
            Killer.GetComponent<PlayerMovement>().sensorBlock.GetComponent<MovementSensor>().intersection = false;
        }
        else
        {
            Singleton.Players[0].GetComponent<PlayerMovement>().sensorBlock.GetComponent<MovementSensor>().intersection = false;
        }
    }

    public override void setKiller(GameObject k)
    {
        Killer = k;
    }
}
