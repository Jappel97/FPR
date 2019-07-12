using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private MoveCore movement;
    private PlayerAnimation pAnim;
    public string actionAnimator;

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<MoveCore>();
        pAnim = GetComponent<PlayerAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleAnimations();
    }


    void HandleAnimations()
    {
        if (actionAnimator == "MOVE")
        {
            pAnim.Movement(movement.moving);
        }
        else if(actionAnimator == "ATTACK")
        {
            pAnim.Attack();
        }
        else
        {
            pAnim.Movement(false);
        }
    }
}
