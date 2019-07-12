using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleFinderBehavior : MonoBehaviour
{
    public bool personFound;
    
    // Start is called before the first frame update
    void Start()
    {
        personFound = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "player")
        {
            personFound = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "player")
        {
            personFound = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "player")
        {
            personFound = false;
        }
    }

    public bool QuerySensory()
    {
        return personFound;
    }

}
