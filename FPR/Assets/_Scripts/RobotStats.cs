using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotStats : CharacterSheet
{
    public GameObject loot1;
    public AnimationClip dying;
    private GameObject Killer;


    // Start is called before the first frame update
    void Start()
    {
        Health = 50;
        Defense = 5;
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
        pAnim.Death();
        Singleton.Enemies.Remove(this.gameObject);
        Vector3 pos = this.transform.position;
        Quaternion rot = this.transform.rotation;
        GameObject.Destroy(this.gameObject, dying.length + 0.5f);
        Instantiate(loot1, pos + new Vector3(-1, 1.2f, 0), Quaternion.Euler(-90, rot.y, 0));
        if (Killer != null)
        {
            Killer.GetComponent<PlayerMovement>().sensorBlock.GetComponent<MovementSensor>().intersection = false;
        }
        else
        {
            Singleton.Players[0].GetComponent<PlayerMovement>().sensorBlock.GetComponent<MovementSensor>().intersection = false;
        }
    }

    public override void setKiller(GameObject killer)
    {
        return;
    }
}
