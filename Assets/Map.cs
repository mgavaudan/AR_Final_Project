using UnityEngine;
using System.Collections.Generic;

public class Map : MonoBehaviour {

    public Transform force;
    public Transform ground;
    public Transform orbTarget;
    public Canvas display;

    public int width = 2;
    public int height = 2;
    public int startX = 0;
    public int startY = 0;
    public Room roomPrefab;

    public Room CurrentRoom { get; private set; }

    bool initialized = false;
    public void Initialize()
    {
        if(!initialized)
        {
            List<List<Room>> map = new List<List<Room>>(width);
            for (int x = 0; x < width; x++)
            {
                List<Room> row = new List<Room>(height);
                for (int y = 0; y < height; y++)
                {
                    Room room = Instantiate(roomPrefab) as Room;
                    room.transform.parent = transform;
                    row.Add(room);
                }
                map.Add(row);
            }

            for(int x = 0; x < width; x++)
                for(int y = 0; y < height; y++)
                {
                    Room northRoom = null, eastRoom = null, southRoom = null, westRoom = null;
                    if (y + 1 < height)
                        northRoom = map[x][y + 1];
                    if (x + 1 < width)
                        eastRoom = map[x + 1][y];
                    if (y > 0)
                        southRoom = map[x][y - 1];
                    if (x > 0)
                        westRoom = map[x - 1][y];
                    map[x][y].Initialize(northRoom, eastRoom, southRoom, westRoom, force, ground, orbTarget, display);
                    map[x][y].gameObject.SetActive(false);
                }

            CurrentRoom = map[startX][startY];
            CurrentRoom.gameObject.SetActive(true);
            CurrentRoom.StartShooting();

            isActive = true;
            initialized = true;
        }
    }
    
    private bool isCrossing;
    private bool doneCrossing;

    public void crossHall()
    {
        Room nextRoom = CurrentRoom.CurrentHall.oppositeRoom;
        // TODO: something has to set completed to be true when all enemies are defeated
        if(nextRoom != null && CurrentRoom.CurrentHall.completed)
        {
            CurrentRoom.StopShooting();
            CurrentRoom.gameObject.SetActive(false);
            int oldHallInd = CurrentRoom.CurrentHallIndex;

            CurrentRoom = nextRoom;
            CurrentRoom.gameObject.SetActive(true);
            CurrentRoom.CurrentHallIndex = oldHallInd;

            isCrossing = true;
            doneCrossing = false;
        }
    }

    bool isActive = false;
    public void Activate()
    {
        if (!isActive)
        {
            if (initialized)
                CurrentRoom.StartShooting();
            else
                Initialize();
            isActive = true;
        }
    }

    public void Deactivate()
    {
        if (initialized)
        {
            CurrentRoom.StopShooting();
            isActive = false;
        }
    }

	// Update is called once per frame
	void Update ()
    {
        if (initialized)
        {
            if (isCrossing)
            {
                // TODO: make animation while crossing, set doneCrossing at end of animation
                doneCrossing = true;

                if (doneCrossing)
                {
                    isCrossing = false;
                    CurrentRoom.StartShooting();
                }
            }
        }
	}
}
