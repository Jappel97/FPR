using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Singleton
{
    public static LevelMap currentLevel;
    public static List<Room> Roomlist;
    public static List<GameObject> Players = new List<GameObject>();
}
