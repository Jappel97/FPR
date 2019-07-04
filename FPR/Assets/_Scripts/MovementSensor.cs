using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSensor : MonoBehaviour
{
    public bool intersection;

    private void OnTriggerEnter(Collider other)
    {
        intersection = true;
    }

    private void OnTriggerExit(Collider other)
    {
        intersection = false;
    }
}
