using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class Behavior : MonoBehaviour
{
    public MoveCore mover;
    public Attacker hitter;
    public abstract string FindBehavior();
}
