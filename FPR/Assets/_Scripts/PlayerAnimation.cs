using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    private Animator anim;
    private string MOVE = "Moving";

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    
    public void Movement(bool Moving)
    {
        anim.SetBool(MOVE, Moving);
    }
}
