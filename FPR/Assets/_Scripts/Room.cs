using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public int centerX, centerY;

    //these are i and j in the algorithm
    //These numbers represent a radius rather than a diameter. They are similar to values a and b in an ellipse
    public int vertAxisLength, horzAxisLength;

    public bool placed;

    public List<Room> neighbors;

    public int ID;
    public Room()
    {
        this.placed = false;
        neighbors = new List<Room>();
    }

    public Room(int vertAxis, int horzAxis, int ID)
    {
        this.vertAxisLength = vertAxis;
        this.horzAxisLength = horzAxis;
        this.ID = ID;
        this.placed = false;
        neighbors = new List<Room>();
    }
}
