﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : MoveCore
{

    private Vector3 startPos;
    private Vector3 destination;
    private bool safe2Move;
    private float startTime;
    private float ETA;
    private float percentage;
    public GameObject sensorBlock;
    private MovementSensor mySensor;


    private CharacterController charcon;



    private void Awake()
    {
        charcon = GetComponent<CharacterController>();
        mySensor = sensorBlock.GetComponent<MovementSensor>();
    }

    


    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
    }

    //takes an angle in degrees.
    public override void StartMove(float angle)
    {
        if (!moving)
        {
            startPos = this.transform.position;
            destination = GetDestination(90 - angle);
            moving = true;
            safe2Move = !mySensor.intersection;
            startTime = Time.time;
            ETA = 0.5f;
            percentage = 0;
        }
    }

    public override bool DoMove()
    {
        if (moving)
        {
            if (safe2Move)
            {
                HandleMovement();
            }
            if ((startPos.x == destination.x && startPos.z == destination.z) || (startTime + 2 <= Time.time))
            {
                moving = false;
            }
        }

        return moving;
    }

    private void HandleMovement()
    {
        if (percentage < 1)
        {
            float elapsed = Time.time - startTime;
            percentage = elapsed / ETA;
            this.transform.position = Vector3.Lerp(startPos, destination, percentage);
        }
        else
        {
            moving = false;
        }
    }

    //Takes an angle in degrees and calculates a destination based on the current heading.
    private Vector3 GetDestination(float angle)
    {
        Vector3 myPos = this.transform.position;
        Vector3 desPos;
        int xAdd = 0;
        int zAdd = 0;
        angle = angle * Mathf.Deg2Rad;

        //We are going to work in 8 directional movement, with each direction having an equal size movement area
        //Thus we will perform some basic trig.
        //We want every angle to collapse down. So 22.5 collapses to 0, while 67.5 collapses to 45
        //We need some equation by which we may collapse these
        if(System.Math.Abs( System.Math.Sin(angle)) > System.Math.Abs(System.Math.Sin(22.5 * Mathf.Deg2Rad)))
        {
            zAdd = 2 * System.Math.Sign(System.Math.Sin(angle));
        }
        if(System.Math.Abs(System.Math.Cos(angle)) >= System.Math.Abs(System.Math.Cos(67.5 * Mathf.Deg2Rad)))
        {
            xAdd = 2 * System.Math.Sign(System.Math.Cos(angle));
        }

        desPos = myPos + new Vector3(xAdd, 0, zAdd);
        return desPos;
    }
}
