using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public Behavior myBehavior;
    
    // Start is called before the first frame update
    void Start()
    {
        myBehavior = GetComponent<Behavior>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetAction()
    {
         return myBehavior.FindBehavior();
    }

    
}
