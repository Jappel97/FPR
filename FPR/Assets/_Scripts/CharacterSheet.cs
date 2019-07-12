using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterSheet : MonoBehaviour
{

    public int Health;
    public int Defense;
    public int minPower;
    public int maxPower;
    public PlayerAnimation pAnim;

    public abstract void Death();
    public abstract void setKiller(GameObject killer);
}
