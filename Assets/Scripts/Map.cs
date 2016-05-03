using UnityEngine;
using System.Collections.Generic;

public class Map : MonoBehaviour {

    public Transform force;
    public Transform ground;
    public Transform orbTarget;
	public Transform minimap;
    public Canvas display;
	public Camera camera;

    public int width = 2;
    public int height = 2;
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

			float widthOffset = 0f;
			float heightOffset = 0f;

			Vector3 startingPos = minimap.transform.position;

			for (int x = 0; x < width; x++) {
				heightOffset = 0;

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
					map [x] [y].Initialize (northRoom, eastRoom, southRoom, westRoom, force, ground, orbTarget, display);
					map [x] [y].gameObject.SetActive (false);

					GameObject block = GameObject.CreatePrimitive (PrimitiveType.Cube);
					block.transform.parent = minimap.transform;
					block.transform.localPosition = new Vector3 (x*60, -60 * y, 0);
					block.transform.localScale = new Vector3 (50, 50, 0);
					block.GetComponent<Renderer>().material.color = Color.white;
				}
			}


            CurrentRoom = map[startX][startY];
            CurrentRoom.gameObject.SetActive(true);
            CurrentRoom.StartShooting();

            isActive = true;
            initialized = true;
        }
    }


    public void crossHall()
    {
        Room nextRoom = CurrentRoom.CurrentHall.oppositeRoom;
		if(nextRoom != null && !isCrossing)
        {
            CurrentRoom.StopShooting();
            CurrentRoom.gameObject.SetActive(false);
            int oldHallInd = CurrentRoom.CurrentHallIndex;

            CurrentRoom = nextRoom;
            CurrentRoom.gameObject.SetActive(true);
            CurrentRoom.CurrentHallIndex = oldHallInd;

			CurrentRoom.transform.position -= ground.up * crossTime * crossSpeed;

            isCrossing = true;
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
				timeCrossing += Time.deltaTime;
				Vector3 movement = Vector3.MoveTowards (CurrentRoom.transform.position, transform.position, crossSpeed * Time.deltaTime);
				CurrentRoom.transform.position += movement;
				if (timeCrossing >= crossTime)
                {
                    isCrossing = false;
                    CurrentRoom.StartShooting();
					timeCrossing = 0;
                }
            }
        }
	}

}
