using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RoomRotator : MonoBehaviour
{
    private class Hall
    {
        public Hall(EnemySpawner spawner, Fire orb/*, RoomManager oppositeRoom*/)
        {
            this.spawner = spawner;
            this.orb = orb;
        }
        public EnemySpawner spawner;
        public Fire orb;
//        public RoomManager oppositeRoom; <- to be set by room generator
    }

    public EnemySpawner enemySpawnerPrefab;
    public Fire orbPrefab;
    public int numRooms = 4;
    public Transform roomSwitcher;
    public float switchAngle = 70;
    public Transform ground;
    public float rotationTime = 1;
    public Transform startPos;
	public Transform arrow;
	public Canvas display;

    private List<Hall> halls = new List<Hall>();
    private int activeHall = 0;

    private Vector3 normal;
    private Quaternion startQuat;
    private float normalAngleEpsilon = 15;

    private bool readyToSwitch = true;
    private int rotateDirection = 0;
    private float angleRotated = 0;

    private bool initialized = false;
	Image selectedPanel;
	private Color selectedPanelColor = Color.blue;
	private Color unselectedPanelColor = Color.white;

    // Use this for initialization
    void Start()
    { }

    public void initialize()
    {
        if(!initialized)
        {
            transform.position = startPos.position;
            normal = ground.up;
            startQuat = ground.rotation;

            float orbRadius = Vector3.Distance(transform.position, ground.position) * 0.95f;
            float spawnerRadius = orbRadius * 0.8f;

            for (int i = 0; i < numRooms; i++)
            {
                Vector3 dir = Quaternion.AngleAxis(i * 360 / numRooms, Vector3.up) * (-normal);
                Vector3 pos = transform.position + dir * spawnerRadius;

                EnemySpawner spawner = Instantiate(enemySpawnerPrefab, pos,
                        Quaternion.identity) as EnemySpawner;
                spawner.transform.parent = transform;

                pos = transform.position + dir * orbRadius;
                Fire orb = Instantiate(orbPrefab, pos, Quaternion.identity) as Fire;
                orb.transform.parent = transform;
                orb.target = startPos;

                if (i > 0)
                {
                    spawner.gameObject.SetActive(false);
                    orb.gameObject.SetActive(false);
                    orb.StopShooting();
                }

                halls.Add(new Hall(spawner, orb));
            }
            initialized = true;

            arrow.gameObject.SetActive (false); // hide arrow
            selectedPanel = display.transform.Find ("Room " + (activeHall+1) + " Panel").GetComponent<Image>();
            selectedPanelColor.a = 0.4f;
            unselectedPanelColor.a = 0.4f;
            selectedPanel.color = selectedPanelColor; // highlight room on "map"
        }
	}

    // Update is called once per frame
    void Update()
    {
        if (initialized)
        {
            float forceAngle = (Quaternion.Inverse(startQuat) * roomSwitcher.rotation).eulerAngles.z;
            if (readyToSwitch && Vector3.Angle(normal, roomSwitcher.up) < normalAngleEpsilon)
            {
                if (forceAngle > switchAngle)
                    rotateRoom(1);
                else if (forceAngle < -switchAngle)
                    rotateRoom(-1);
            }


            if (rotateDirection == 0 && Mathf.Abs(forceAngle) < switchAngle * 0.75f)
            {
                readyToSwitch = true;
                arrow.parent = transform; // set arrow as child of room manager
                arrow.position = transform.position; // set arrow in front of user
                arrow.gameObject.SetActive(true); // make arrow visible
            }

            if (rotateDirection != 0)
            {
                float rotateAngle = 360 / numRooms / rotationTime * Time.deltaTime;
                transform.RotateAround(Vector3.up, transform.position, rotateDirection * rotateAngle);

                angleRotated += rotateAngle;
                if (angleRotated >= 360 / numRooms)
                {
                    arrow.gameObject.SetActive(false); // hides arrow
                    halls[activeHall].orb.gameObject.SetActive(false);
                    halls[activeHall].spawner.gameObject.SetActive(false);

                    activeHall = (activeHall + rotateDirection) % numRooms;
                    if (activeHall < 0)
                        activeHall += numRooms;
                    halls[activeHall].orb.gameObject.SetActive(true);
                    halls[activeHall].orb.StartShooting();
                    halls[activeHall].spawner.gameObject.SetActive(true);

                    selectedPanel.color = unselectedPanelColor; // unhighlight previously active room on "map'
                    selectedPanel = display.transform.Find ("Room " + (activeHall+1) + " Panel").GetComponent<Image>(); // change selected panel to currently active room
                    selectedPanel.color = selectedPanelColor; // highlight currently active room on "map"

                    rotateDirection = 0;
                }
            }
        }
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
            halls[activeHall].orb.StartShooting();
    }

    public void StopShooting()
    {
        if(initialized)
            halls[activeHall].orb.StopShooting();
    }

    enum Direction { Clockwise, Counterclockwise, None };

    private void rotateRoom(int dir)
    {
        readyToSwitch = false;
        rotateDirection = dir;
        angleRotated = 0;
        halls[activeHall].orb.StopShooting();
    }

    public Fire activeOrb()
    {
        return halls[activeHall].orb;
    }

    public EnemySpawner activeEnemySpawner()
    {
        return halls[activeHall].spawner; ;
    }
}
