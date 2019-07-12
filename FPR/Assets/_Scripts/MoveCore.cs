using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class MoveCore : NetworkBehaviour
{
    public bool moving;
    public abstract void StartMove(float angle);
    public abstract bool DoMove();
}
