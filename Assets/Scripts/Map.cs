using UnityEngine;
using System.Collections.Generic;

public class Map : MonoBehaviour {

    public Transform force;
    public Transform ground;
    public Transform orbTarget;
	public MinimapManager minimap;
	public Camera arcamera;

    public int width = 3;
    public int height = 3;
    public int startX = 0;
    public int startY = 0;
    public Room roomPrefab;

	public float crossSpeed = 1;
	public float crossTime = 2;

	private float timeCrossing = 0;

	private bool isActive = false;


    public Room CurrentRoom { get; private set; }

	private forceAction forceScript;

    bool initialized = false;

	private bool isCrossing;


    public void Initialize()
    {
        if(!initialized)
        {
			forceScript = GameObject.Find ("Force").GetComponent<forceAction>();
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

			for (int x = 0; x < width; x++) {

				for (int y = 0; y < height; y++) {
					Room northRoom = null, eastRoom = null, southRoom = null, westRoom = null;
					if (y + 1 < height)
						northRoom = map [x] [y + 1];
					if (x + 1 < width)
						eastRoom = map [x + 1] [y];
					if (y > 0)
						southRoom = map [x] [y - 1];
					if (x > 0)
						westRoom = map [x - 1] [y];
					map [x] [y].Initialize (northRoom, eastRoom, southRoom, westRoom, force, ground, orbTarget, minimap, x*width + y);
					map [x] [y].gameObject.SetActive (false);
				}
			}

			CurrentRoom = map[startX][startY];
			CurrentRoom.gameObject.SetActive(true);
			CurrentRoom.StartShooting();

            minimap.Initialize(height, width, startX, startY);

			isActive = true;
			initialized = true;

            CurrentRoom.CurrentHallIndex = 1;
        }
    }


    public void crossHall()
    {
        Room.Hall hall = CurrentRoom.CurrentHall;
		if(hall != null && !isCrossing)
        {
            CurrentRoom.StopShooting();
            CurrentRoom.gameObject.SetActive(false);
            int hallInd = CurrentRoom.CurrentHallIndex;

            CurrentRoom = hall.oppositeRoom;
            CurrentRoom.gameObject.SetActive(true);
            CurrentRoom.CurrentHallIndex = hallInd;

			CurrentRoom.transform.position -= ground.up * crossTime * crossSpeed;

            isCrossing = true;
            minimap.crossHall();
        }
    }

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
				CurrentRoom.transform.position += ground.up * crossSpeed * Time.deltaTime;
				timeCrossing += Time.deltaTime;
				if (timeCrossing >= crossTime)
                {
                    while (CurrentRoom.CurrentHall == null)
                        CurrentRoom.CurrentHallIndex++;
                    isCrossing = false;
                    CurrentRoom.StartShooting();
					timeCrossing = 0;
                }
            }
        }
	}

}
