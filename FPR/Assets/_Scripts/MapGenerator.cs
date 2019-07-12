using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    /*
     * Before I begin writing the map generation program I must make a few notes
     * Given the current scale of the map tiles, each one must be two Unity Units away from each other
     * We also consider here a few options for map generation algorithms:
     * Option 1:
     *  The algorithm considers a 255 by 255 grid which it will populate with some number of rectangular rooms size MxN
     *  The various rooms are then connected via hallways whose paths are created via a squarewave with a random length of period and random amplitude
     *  These hallways are produced such that any room has a path to any other room
     * Option 2:
     *  We literally just copy the algorithm from Rogue
     * Option 3:
     *  We copy the algorithm used in the Donjon dungeon generator
     * Option 4:
     *  Something else??
     *  
     *  
     *  The previous notes are now depricated. I have designed an algorithm which I will try to implement.
     *  Find more info about this algorithm in the text file in this directory.
     */


    public GameObject roomCenter, roomWall, roomCorner;
    public GameObject hall, hallCorner;
    public GameObject hallThree, hallFour;
    public GameObject Spawnpoint;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Starting generation");
        this.gameObject.GetComponent<MapGenerator>().DrawMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrawMap()
    {
        LevelMap currentMap = GenerateMap();
        GameObject positionMarker = new GameObject();
        positionMarker.transform.position = new Vector3(0, 0, 0);
        List<GameObject> TileList = new List<GameObject>();
        // add 2 to x: positionMarker.transform.position += (new Vector3(2, 0, 0));
        //add 2 to z: positionMarker.transform.position += (new Vector3(0, 0, 2));

        //set tiles
        for (int i = 0; i < LevelMap.size; i++)
        {
            for(int j = 0; j < LevelMap.size; j++)
            {
                //TODO
                switch (currentMap.map[i,j])
                {
                    case '.':
                        TileList.Add(Instantiate(roomCenter, positionMarker.transform.position, Quaternion.identity));
                        break;
                    case '|':
                        TileList.Add(Instantiate(roomWall, positionMarker.transform.position, Quaternion.identity));
                        break;
                    case '=':
                        TileList.Add(Instantiate(roomCorner, positionMarker.transform.position, Quaternion.identity));
                        break;
                    case '#':
                        TileList.Add(Instantiate(hall, positionMarker.transform.position, Quaternion.identity));
                        break;
                    case ' ':
                        break;
                    default:
                        break;
                }
                positionMarker.transform.position += (new Vector3(2, 0, 0));
            }
            positionMarker.transform.position += (new Vector3(-(2*LevelMap.size), 0, 2));
        }

        foreach (GameObject tile in TileList)
        {
            int myX = (int)tile.transform.position.x / 2;
            int myY = (int)tile.transform.position.z / 2;
            char up = currentMap.map[myY + 1, myX];
            char down = currentMap.map[myY - 1, myX];
            char left = currentMap.map[myY, myX - 1];
            char right = currentMap.map[myY, myX + 1];
            //orient tiles by type
            //orient walls
            //TODO
            if (tile.tag.Equals("WallTile"))
            {
                if(up == ' ')
                {
                    //rotate 270 along Y
                    tile.transform.rotation = Quaternion.Euler(0, 270, 0);
                }
                else if(down == ' ')
                {
                    //rotate 90 along Y
                    tile.transform.rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (right == ' ')
                {
                    //rotate 0
                }
                else if (left == ' ')
                {
                    //rotate 180 along Y
                    tile.transform.rotation = Quaternion.Euler(0, 180, 0);
                }
            }
            //orient corners
            //TODO
            else if (tile.tag.Equals("CornerTile"))
            {
                if(up == ' ' && left == ' ')
                {
                    tile.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else if(up == ' ' && right == ' ')
                {
                    tile.transform.rotation = Quaternion.Euler(0, 90, 0);
                }
                else if(down == ' ' && left == ' ')
                {
                    tile.transform.rotation = Quaternion.Euler(0, 270, 0);
                }
                else if(down == ' ' && right == ' ')
                {
                    tile.transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else if(currentMap.map[myY - 1, myX - 1] == '.')
                {
                    tile.transform.rotation = Quaternion.Euler(0, 270, 0);
                }
                else if (currentMap.map[myY + 1, myX - 1] == '.')
                {
                    tile.transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (currentMap.map[myY + 1, myX + 1] == '.')
                {
                    tile.transform.rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (currentMap.map[myY - 1, myX + 1] == '.')
                {
                    tile.transform.rotation = Quaternion.Euler(0, 360, 0);
                }
            }
            //orient halls
            //TODO
            else if (tile.tag.Equals("HallTile"))
            {
                //stuff
                //(up == '#' || up == '.') && (down == '#' || down == '.') && (left == '#' || left == '.') && (right == '#' || right == '.') && (!(up == '.' && down == '.') && !(up == '.' && left == '.') && !(up =='.' && right =='.') && !(left == '.' && right == '.') && !(left == '.' && down =='.') && !(down == '.' && right == '.'))
                if (dotCheck(new char[] {up, down, left, right }))
                {
                    positionMarker.transform.position = tile.transform.position;
                    GameObject.Destroy(tile);
                    Instantiate(hallFour, positionMarker.transform.position, Quaternion.Euler(0, 0, 0));
                }
                else if ((up == '#' || up == '.') && (down == '#' || down == '.') && (right == '#' || right == '.') && (dotCount(new char[] { up, down, right}) <= 1))
                {
                    positionMarker.transform.position = tile.transform.position;
                    GameObject.Destroy(tile);
                    Instantiate(hallThree, positionMarker.transform.position, Quaternion.Euler(0, 0, 0));
                }
                else if ((up == '#' || up == '.') && (down == '#' || down == '.') && (left == '#' || left == '.') && (dotCount(new char[] { up, down, left }) <= 1))
                {
                    positionMarker.transform.position = tile.transform.position;
                    GameObject.Destroy(tile);
                    Instantiate(hallThree, positionMarker.transform.position, Quaternion.Euler(0, 180, 0));
                }
                else if ((up == '#' || up == '.') && (left == '#' || left == '.') && (right == '#' || right == '.') && (dotCount(new char[] { up, left, right }) <= 1))
                {
                    positionMarker.transform.position = tile.transform.position;
                    GameObject.Destroy(tile);
                    Instantiate(hallThree, positionMarker.transform.position, Quaternion.Euler(0, 270, 0));
                }
                else if ((down == '#' || down == '.') && (right == '#' || right == '.') && (left == '#' || left == '.') && (dotCount(new char[] { left, down, right }) <= 1))
                {
                    positionMarker.transform.position = tile.transform.position;
                    GameObject.Destroy(tile);
                    Instantiate(hallThree, positionMarker.transform.position, Quaternion.Euler(0, 90, 0));
                }
                else if (up == '#' && down == '#')
                {
                    
                }
                else if ((left == '#' || left == '.') && (right == '#' || right == '.') && (!(left == '.' && right == '.')))
                {
                    tile.transform.rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (up == '#' && left == '#')
                {
                    positionMarker.transform.position = tile.transform.position;
                    GameObject.Destroy(tile);
                    Instantiate(hallCorner, positionMarker.transform.position, Quaternion.Euler(0, 180, 0));
                }
                else if (up == '#' && right == '#')
                {
                    positionMarker.transform.position = tile.transform.position;
                    GameObject.Destroy(tile);
                    Instantiate(hallCorner, positionMarker.transform.position, Quaternion.Euler(0, 270, 0));

                }
                else if (down == '#' && left == '#')
                {
                    positionMarker.transform.position = tile.transform.position;
                    GameObject.Destroy(tile);
                    Instantiate(hallCorner, positionMarker.transform.position, Quaternion.Euler(0, 90, 0));

                }
                else if (down == '#' && right == '#')
                {
                    positionMarker.transform.position = tile.transform.position;
                    GameObject.Destroy(tile);
                    Instantiate(hallCorner, positionMarker.transform.position, Quaternion.Euler(0, 0, 0));

                }

            }
        }

        GameObject.Destroy(positionMarker);

        String str = "";
        for(int i = 0; i < LevelMap.size; i++)
        {
            for(int j = 0; j < LevelMap.size; j++)
            {
                str = str + currentMap.map[i, j];
            }
            str = str + "\n";
        }

        SetSpawns();

        System.IO.File.WriteAllText(@"C:\Users\Josh\Desktop\Debug.txt", str);
        Singleton.currentLevel = currentMap;
    }

    private void SetSpawns()
    {
        Room myRoom = Singleton.Roomlist[0];
        int myX = 2* myRoom.centerX;
        int myY = 2* myRoom.centerY;
        for(int i = -1; i < 2; i++)
        {
            for(int j = -1; j < 2; j++)
            {
                if (i != 0 && j != 0)
                {
                    GameObject.Instantiate(Spawnpoint, new Vector3(myX + 2 * j, 0, myY + 2 * i), Quaternion.identity);
                }
            }
        }
    }

    private bool dotCheck(char[] v)
    {
        int hashes = 0, dots = 0;
        for(int i = 0; i < v.Length; i++)
        {
            if(v[i] == '#')
            {
                hashes++;
            }
            else if(v[i] == '.')
            {
                dots++;
            }
        }
        return (hashes == 4) ^ (dots == 1 && hashes == 3);
    }

    private int dotCount(char[] v)
    {
        int dots = 0;
        for(int i = 0; i < v.Length; i++)
        {
            if(v[i] == '.')
            {
                dots++;
            }
        }
        return dots;
    }


    //generates the map in memory and returns a 2D character array describing the map
    //each character in the array represents a tile.
    // . is a center room tile
    // | is a room wall
    // = is a room corner
    // # is a hall
    // L is a hall corner
    private LevelMap GenerateMap()
    {
        LevelMap workingMap = new LevelMap();
        List<Room> roomList = new List<Room>();
        System.Random rand = new System.Random();
        //this represents the number K in the algorithm

        //Create rooms
        int numRooms = rand.Next(5, 9);
        for(int i = 0; i < numRooms; i++)
        {
            roomList.Add(new Room(rand.Next(3, 5), rand.Next(3, 5), i));
        }

        //Place rooms
        foreach(Room r in roomList)
        {
            while (!r.placed)
            {
                r.centerX = rand.Next(0 + r.horzAxisLength + 1, LevelMap.size - r.horzAxisLength - 1);
                r.centerY = rand.Next(0 + r.vertAxisLength + 1, LevelMap.size - r.vertAxisLength - 1);
                int cornerX = r.centerX - r.horzAxisLength - 1;
                int cornerY = r.centerY - r.vertAxisLength - 1;
                bool overlap = false;
                //Walk the area checking for intersections
                for(int i = 0; i < ((2 * r.vertAxisLength) + 3); i++)
                {
                    for(int j = 0; j < ((2 * r.horzAxisLength) + 3); j++)
                    {
                        if(workingMap.map[cornerY + i, cornerX + j] != ' ')
                        {
                            overlap = true;
                            break;
                        }
                    }
                    if (overlap)
                    {
                        break;
                    }
                }
                if (overlap)
                {
                    continue;
                }
                else
                {
                    r.placed = true;
                }
                for(int i = 0; i < ((r.vertAxisLength * 2) + 1); i++)
                {
                    for(int j = 0; j < ((r.horzAxisLength * 2) + 1); j++)
                    {
                        workingMap.map[cornerY + 1 + i, cornerX + 1 + j] = '.';
                        if ((i == 0 || i == (r.vertAxisLength * 2)) && (j == 0 || j == (r.horzAxisLength * 2)))
                        {
                            workingMap.map[cornerY + 1 + i, cornerX + 1 + j] = '=';
                        }
                        else if (j == 0 || j == (r.horzAxisLength * 2))
                        {
                            workingMap.map[cornerY + 1 + i, cornerX + 1 + j] = '|';
                        }
                        else if((i == 0 || i == (r.vertAxisLength * 2)))
                        {
                            workingMap.map[cornerY + 1 + i, cornerX + 1 + j] = '|';
                        }
                    }
                }
            }
        }
        

        //Find neighbors
        foreach (Room r in roomList)
        {
            int numConnections = rand.Next(2, 4);
            List<KeyValuePair<Room, double>> distances = new List<KeyValuePair<Room, double>>();
            foreach(Room other in roomList)
            {
                if(other != r)
                {
                    distances.Add(new KeyValuePair<Room, double>(other, Distance(r.centerX, r.centerY, other.centerX, other.centerY)));
                }
            }
            distances.Sort((x, y) => x.Value.CompareTo(y.Value));
            for(int i = 0; i < numConnections; i++)
            {
                r.neighbors.Add(distances[i].Key);
            }
            
        }

        workingMap = placeCorridors(workingMap, roomList);
        return workingMap;
    }

    private LevelMap placeCorridors(LevelMap workingMap, List<Room> roomList)
    {
        System.Random rand = new System.Random();

        //place corridors
        foreach (Room r in roomList)
        {
            foreach (Room neighbor in r.neighbors)
            {
                //Between any room and a neighbor we wish to connect to, we shall have a corridor.
                //The corridor shall run two legs of the triangle connecting the rooms' centers
                //From the room we currently reside in, we will draw either the horizontal or vertical leg first, depending on the slope of the hypotenues
                //The leg will begin on a randomly chosen tile between the center and the outmost wall
                //We will then continue to a randomly chosen point which aligns between the center and outermost wall of the other room
                //We continue this behavior for all rooms and neighbors
                //and we hope that there are no collisions between corridors O.o
                double angle = findSlope(r.centerX, r.centerY, neighbor.centerX, neighbor.centerY);
                char direction = System.Math.Abs(angle) > 1 ? 'v' : 'h';
                System.Math.Sign(angle);
                bool roomHit = false;
                int yMultiplier = System.Math.Sign(neighbor.centerY - r.centerY);
                int xMultiplier = System.Math.Sign(neighbor.centerX - r.centerX);
                if (direction == 'h')
                {

                    int startY = r.centerY;
                    //Check for overlap along the Y axis
                    //We will do this through a simple branch block
                    //Remember that down is positive on Y during this, so our first question is if the neighbor is lower than the room
                    if(r.centerY == lowerOf(r.centerY, neighbor.centerY))
                    {
                        if((r.centerY + r.vertAxisLength -1) > (neighbor.centerY - neighbor.vertAxisLength + 1))
                        {
                            //This means that the south wall of our room is LOWER than the north wall of the neighbor.
                            //So we're gonna choose some place between these two walls.
                            startY = rand.Next(neighbor.centerY - neighbor.vertAxisLength + 1, r.centerY + r.vertAxisLength - 1);
                        }
                        else
                        {
                            startY = r.centerY + (yMultiplier * rand.Next(0, r.vertAxisLength - 1));
                        }
                    }
                    //Then we check to see if maybe the neighbor is higher than the room
                    else if(neighbor.centerY == lowerOf(r.centerY, neighbor.centerY))
                    {
                        if((r.centerY - r.vertAxisLength + 1) < (neighbor.centerY + neighbor.vertAxisLength - 1))
                        {
                            startY = rand.Next(r.centerY - r.vertAxisLength + 1, neighbor.centerY + neighbor.vertAxisLength - 1);
                        }
                        else
                        {
                            startY = r.centerY + (yMultiplier * rand.Next(0, r.vertAxisLength - 1));
                        }
                    }

                    //Go horizontally
                    //We need to choose a random stopping point between the center X coord of the neighbor and the neighbor's nearest wall
                    //We can do this by calculating the distance between our X and the neighbor's X
                    //Then we subtract a random value between 0 and the horizontal length of the neighbor
                    //This is how long we travel from our center.
                    int travDist = System.Math.Abs(r.centerX - neighbor.centerX) - rand.Next(0, neighbor.horzAxisLength - 1);
                    for(int i = 0; i < travDist; i++)
                    {
                        if(workingMap.map[startY, r.centerX + (i * xMultiplier)] == ' ')
                        {
                            workingMap.map[startY, r.centerX + (i * xMultiplier)] = '#';
                            
                        }
                        else if (workingMap.map[startY, r.centerX + (i * xMultiplier)] == '.')
                        {
                            if (i > r.horzAxisLength)
                            {
                                roomHit = true;
                                break;
                            }
                        }
                        else if(workingMap.map[startY, r.centerX + (i * xMultiplier)] == '|')
                        {
                            workingMap.map[startY, r.centerX + (i * xMultiplier)] = '#';
                            if (i > r.horzAxisLength)
                            {
                                roomHit = true;
                                break;
                            }
                        }
                        else if (workingMap.map[startY, r.centerX + (i * xMultiplier)] == '=')
                        {
                            workingMap.map[startY, r.centerX + (i * xMultiplier)] = '|';
                            if (i > r.horzAxisLength)
                            {
                                roomHit = true;
                                break;
                            }
                        }
                    }

                    if (roomHit)
                    {
                        continue;
                    }
                    //Go vertically
                    int yDist = System.Math.Abs(startY - neighbor.centerY);
                    for(int i = 0; i < yDist; i++)
                    {
                        if (workingMap.map[startY + (i * yMultiplier), r.centerX + ((travDist - 1) * xMultiplier)] == ' ')
                        {
                            workingMap.map[startY + (i * yMultiplier), r.centerX + ((travDist - 1) * xMultiplier)] = '#';
                        }
                        else if (workingMap.map[startY + (i * yMultiplier), r.centerX + ((travDist - 1) * xMultiplier)] == '.')
                        {
                            roomHit = true;
                            break;
                        }
                        else if (workingMap.map[startY + (i * yMultiplier), r.centerX + ((travDist - 1) * xMultiplier)] == '|')
                        {
                            workingMap.map[startY + (i * yMultiplier), r.centerX + ((travDist - 1) * xMultiplier)] = '#';
                            roomHit = true;
                            break;
                        }
                        else if (workingMap.map[startY + (i * yMultiplier), r.centerX + ((travDist - 1) * xMultiplier)] == '=')
                        {
                            workingMap.map[startY + (i * yMultiplier), r.centerX + ((travDist - 1) * xMultiplier)] = '|';
                            roomHit = true;
                            break;
                        }
                    }


                }
                else if(direction == 'v')
                {
                    int startX = r.centerX;
                    //Check for overlap along the Y axis
                    //We will do this through a simple branch block
                    //Remember that down is positive on Y during this, so our first question is if the neighbor is lower than the room
                    if (r.centerX == lowerOf(r.centerX, neighbor.centerX))
                    {
                        if ((r.centerX + r.horzAxisLength - 1) > (neighbor.centerX - neighbor.horzAxisLength + 1))
                        {
                            //This means that the south wall of our room is LOWER than the north wall of the neighbor.
                            //So we're gonna choose some place between these two walls.
                            startX = rand.Next(neighbor.centerX - neighbor.horzAxisLength + 1, r.centerX + r.horzAxisLength - 1);
                        }
                        else
                        {
                            startX = r.centerX + (xMultiplier * rand.Next(0, r.horzAxisLength - 1));
                        }
                    }
                    //Then we check to see if maybe the neighbor is higher than the room
                    else if (neighbor.centerX == lowerOf(r.centerX, neighbor.centerX))
                    {
                        if ((r.centerX - r.horzAxisLength + 1) < (neighbor.centerX + neighbor.horzAxisLength - 1))
                        {
                            startX = rand.Next(r.centerX - r.horzAxisLength + 1, neighbor.centerX + neighbor.horzAxisLength - 1);
                        }
                        else
                        {
                            startX = r.centerX + (xMultiplier * rand.Next(0, r.horzAxisLength - 1));
                        }
                    }

                    //Go horizontally
                    //We need to choose a random stopping point between the center X coord of the neighbor and the neighbor's nearest wall
                    //We can do this by calculating the distance between our X and the neighbor's X
                    //Then we subtract a random value between 0 and the horizontal length of the neighbor
                    //This is how long we travel from our center.
                    int travDist = System.Math.Abs(r.centerY - neighbor.centerY) - rand.Next(0, neighbor.vertAxisLength - 1);
                    for (int i = 0; i < travDist; i++)
                    {
                        if (workingMap.map[ r.centerY + (i * yMultiplier), startX] == ' ')
                        {
                            workingMap.map[r.centerY + (i * yMultiplier), startX] = '#';
                        }
                        else if (workingMap.map[r.centerY + (i * yMultiplier), startX] == '.')
                        {
                            if (i > r.vertAxisLength)
                            {
                                roomHit = true;
                                break;
                            }
                        }
                        else if (workingMap.map[r.centerY + (i * yMultiplier), startX] == '|')
                        {
                            workingMap.map[r.centerY + (i * yMultiplier), startX] = '#';
                            if (i > r.vertAxisLength)
                            {
                                roomHit = true;
                                break;
                            }
                        }
                        else if (workingMap.map[r.centerY + (i * yMultiplier), startX] == '=')
                        {
                            workingMap.map[r.centerY + (i * yMultiplier), startX] = '|';
                            if (i > r.vertAxisLength)
                            {
                                roomHit = true;
                                break;
                            }
                        }
                    }

                    if (roomHit)
                    {
                        continue;
                    }
                    //Go horizontally
                    int xDist = System.Math.Abs(startX - neighbor.centerX);
                    for(int i = 0; i < xDist; i++)
                    {
                        if (workingMap.map[r.centerY + ((travDist - 1) * yMultiplier), startX + (i * xMultiplier)] == ' ')
                        {
                            workingMap.map[r.centerY + ((travDist - 1) * yMultiplier), startX + (i * xMultiplier)] = '#';
                        }
                        else if (workingMap.map[r.centerY + ((travDist - 1) * yMultiplier), startX + (i * xMultiplier)] == '.')
                        {
                            roomHit = true;
                            break;
                        }
                        else if (workingMap.map[r.centerY + ((travDist - 1) * yMultiplier), startX + (i * xMultiplier)] == '|')
                        {
                            workingMap.map[r.centerY + ((travDist - 1) * yMultiplier), startX + (i * xMultiplier)] = '#';
                            roomHit = true;
                            break;
                        }
                        else if (workingMap.map[r.centerY + ((travDist - 1) * yMultiplier), startX + (i * xMultiplier)] == '=')
                        {
                            workingMap.map[r.centerY + ((travDist - 1) * yMultiplier), startX + (i * xMultiplier)] = '|';
                            roomHit = true;
                            break;
                        }
                    }
                }
            }
        }

        Singleton.Roomlist = roomList;

        return workingMap;
    }

    private double findSlope(int x1, int y1, int x2, int y2)
    {
        int diff1 = y2 - y1;
        int diff2 = x2 - x1 != 0 ? x2 - x1 : 1;
        double slope = diff1 / diff2;
        return slope;
    }

    private double findIntercept(double slope, int pointX, int pointY)
    {
        double intercept;
        //y - y1 = m(x-x1)
        //y - y1 = mx - mx1
        double x1 = slope * -pointX;
        double b = x1 + pointY;
        intercept = b;
        return intercept;
    }

    private double Distance(int x1, int y1, int x2, int y2)
    {
        double d = 0.0;
        int a = x2 - x1;
        int b = y2 - y1;
        a = a * a;
        b = b * b;
        d = System.Math.Sqrt(a + b);
        return d;
    }

    private double lowerOf(double a, double b)
    {
        if(a > b)
        {
            return b;
        }
        else if(a < b)
        {
            return a;
        }
        else
        {
            return 0;
        }
    }

    private int lowerOf(int a, int b)
    {
        if(a > b)
        {
            return b;
        }
        else if (a < b)
        {
            return a;
        }
        else
        {
            return 0;
        }
    }

    private int higherOf(int a, int b)
    {
        if (a > b)
        {
            return a;
        }
        else if (a < b)
        {
            return b;
        }
        else
        {
            return 0;
        }
    }

    private double higherOf(double a, double b)
    {
        if (a > b)
        {
            return a;
        }
        else if (a < b)
        {
            return b;
        }
        else
        {
            return 0;
        }
    }
}
