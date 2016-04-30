using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    public class Hall
    {
        public Hall(EnemySpawner spawner, Fire orb, Room oppositeRoom)
        {
            this.spawner = spawner;
            this.orb = orb;
            this.oppositeRoom = oppositeRoom;
        }

        public EnemySpawner spawner;
        public Fire orb;
        public Room oppositeRoom;

        // TODO: track when this is completed
        public bool completed = false;
    }

    public EnemySpawner enemySpawnerPrefab;
    public Fire orbPrefab;

    public float switchAngle = 70;
    public float rotationTime = 1;

    private Transform force;
	private Canvas display;

    public const int numHalls = 4;

    public Hall CurrentHall { get { return halls[activeHall]; } }
    private List<Hall> halls = new List<Hall>();
    private int activeHall = 0;
    public int CurrentHallIndex
    {
        get
        {
            return activeHall;
        }

        set
        {
            CurrentHall.orb.StopShooting();
            CurrentHall.orb.gameObject.SetActive(false);
            CurrentHall.spawner.gameObject.SetActive(false);

            float angle = 360 / numHalls * (activeHall - value);

            activeHall = mod(value, numHalls);
            transform.RotateAround(transform.position, rotationAxis, angle);

            CurrentHall.orb.gameObject.SetActive(true);
            CurrentHall.spawner.gameObject.SetActive(true);
        }
    }

    private Vector3 forceNormal;
    private Vector3 rotationAxis;
    private Quaternion startQuat;
    private float normalAngleEpsilon = 15;

    private bool readyToSwitch = true;
    private int rotateDirection = 0;
    private float angleRotated = 0;

    private bool initialized = false;
	Image selectedPanel;
	private Color selectedPanelColor = Color.blue;
	private Color unselectedPanelColor = Color.white;

    public void Initialize(Room northRoom, Room eastRoom, Room southRoom, Room westRoom,
        Transform force, Transform ground, Transform orbTarget, Canvas display)
    {
        if(!initialized)
        {
            this.force = force;
            this.display = display;

            List<Room> rooms = new List<Room> { northRoom, eastRoom, southRoom, westRoom };

            forceNormal = ground.up;
            rotationAxis = ground.forward;
            startQuat = ground.rotation;

            float orbRadius = Vector3.Distance(transform.position, ground.position) * 0.95f;
            float spawnerRadius = orbRadius * 0.8f;

            for (int i = 0; i < numHalls; i++)
            {
                Vector3 dir = Quaternion.AngleAxis(i * 360 / numHalls, Vector3.up) * (-forceNormal);
                Vector3 pos = transform.position + dir * spawnerRadius;

                EnemySpawner spawner = Instantiate(enemySpawnerPrefab, pos,
                        Quaternion.identity) as EnemySpawner;
                spawner.transform.parent = transform;

                pos = transform.position + dir * orbRadius;
                Fire orb = Instantiate(orbPrefab, pos, Quaternion.identity) as Fire;
                orb.transform.parent = transform;
                orb.target = orbTarget;

                if (i > 0)
                {
                    spawner.gameObject.SetActive(false);
                    orb.gameObject.SetActive(false);
                }

                halls.Add(new Hall(spawner, orb, rooms[i]));
            }

            selectedPanel = display.transform.Find ("Room " + (activeHall+1) + " Panel").GetComponent<Image>();
            selectedPanelColor.a = 0.4f;
            unselectedPanelColor.a = 0.4f;
            selectedPanel.color = selectedPanelColor; // highlight room on "map"

            initialized = true;
        }
	}

    // Update is called once per frame
    void Update()
    {
        if (initialized)
        {
            float forceAngle = clipAngle((Quaternion.Inverse(startQuat) * force.rotation).eulerAngles.y);
            if (readyToSwitch && Vector3.Angle(forceNormal, force.up) < normalAngleEpsilon)
            {
                if (forceAngle > switchAngle)
                    rotateRoom(1);
                else if (forceAngle < -switchAngle)
                    rotateRoom(-1);
            }

            if (rotateDirection == 0 && Mathf.Abs(forceAngle) < switchAngle * 0.5f)
            {
                readyToSwitch = true;
            }

            if (rotateDirection != 0)
            {
                float rotateAngle = 360 / numHalls * Time.deltaTime / rotationTime;
                transform.RotateAround(transform.position, rotationAxis, rotateDirection * rotateAngle);

                angleRotated += rotateAngle;
                if (angleRotated >= 360 / numHalls)
                {
                    Hall oldHall = halls[mod(activeHall + rotateDirection, numHalls)];
                    if (oldHall != null)
                    {
                        oldHall.orb.gameObject.SetActive(false);
                        oldHall.spawner.gameObject.SetActive(false);
                    }

                    if(CurrentHall == null)
                        rotateRoom(rotateDirection);
                    else
                        rotateDirection = 0;
                }
            }
        }
    }

    // force angle between -180 and 180
    private float clipAngle(float angle)
    {
        if (angle <= 180)
            return angle;
        return angle - 360;
    }

    private bool oldReadyToSwitch;
    public void LostWand()
    {
        oldReadyToSwitch = readyToSwitch;
        readyToSwitch = false;
    }
    
    public void FoundWand()
    {
        readyToSwitch = oldReadyToSwitch;
    }

    public void StartShooting()
    {
        if(initialized)
            CurrentHall.orb.StartShooting();
    }

    public void StopShooting()
    {
        if(initialized)
            CurrentHall.orb.StopShooting();
    }

    enum Direction { Clockwise, Counterclockwise, None };

    private void rotateRoom(int dir)
    {
        readyToSwitch = false;
        rotateDirection = dir;
        angleRotated = 0;
        CurrentHall.orb.StopShooting();

        activeHall = mod(activeHall - rotateDirection, numHalls);
        if (activeHall < 0)
            activeHall += numHalls;
        CurrentHall.orb.gameObject.SetActive(true);
        CurrentHall.orb.StartShooting();
        CurrentHall.spawner.gameObject.SetActive(true);

        selectedPanel.color = unselectedPanelColor; // unhighlight previously active room on "map'
        selectedPanel = display.transform.Find("Room " + (activeHall + 1) + " Panel").GetComponent<Image>();
        selectedPanel.color = selectedPanelColor; // highlight currently active room on "map"
    }

    private int mod(int a, int b)
    {
        int c = a % b;
        if (c < 0)
            c += b;
        return c;
    }

    public Fire activeOrb()
    {
        return CurrentHall.orb;
    }

    public EnemySpawner activeEnemySpawner()
    {
        return CurrentHall.spawner; ;
    }
}
