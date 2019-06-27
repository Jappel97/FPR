using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMap
{
    //this is the n in the algorithm
    public const int size = 80;

    public char[,] map = new char[size, size];

    public LevelMap()
    {
        for (int i = 0; i < LevelMap.size; i++)
        {
            for (int j = 0; j < LevelMap.size; j++)
            {
                this.map[i, j] = ' ';
            }
        }
    }
}
