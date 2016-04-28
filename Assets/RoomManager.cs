using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
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

    private List<EnemySpawner> enemySpawners = new List<EnemySpawner>();
    private List<Fire> orbs = new List<Fire>();
    private int activeRoom = 0;

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
                enemySpawners.Add(spawner);

                pos = transform.position + dir * orbRadius;
                Fire orb = Instantiate(orbPrefab, pos, Quaternion.identity) as Fire;
                orb.transform.parent = transform;
                orb.target = startPos;
                orbs.Add(orb);

                if (i > 0)
                {
                    enemySpawners[i].gameObject.SetActive(false);
                    orbs[i].gameObject.SetActive(false);
                    orbs[i].StopShooting();
                }
            }
            initialized = true;

            arrow.gameObject.SetActive (false); // hide arrow
            selectedPanel = display.transform.Find ("Room " + (activeRoom+1) + " Panel").GetComponent<Image>();
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
                transform.Rotate(Vector3.up, rotateDirection * rotateAngle, Space.World);

                angleRotated += rotateAngle;
                if (angleRotated >= 360 / numRooms)
                {
                    arrow.gameObject.SetActive(false); // hides arrow
                    orbs[activeRoom].gameObject.SetActive(false);
                    enemySpawners[activeRoom].gameObject.SetActive(false);

                    activeRoom = (activeRoom + rotateDirection) % numRooms;
                    if (activeRoom < 0)
                        activeRoom += numRooms;
                    orbs[activeRoom].gameObject.SetActive(true);
                    orbs[activeRoom].StartShooting();
                    enemySpawners[activeRoom].gameObject.SetActive(true);

                    selectedPanel.color = unselectedPanelColor; // unhighlight previously active room on "map'
                    selectedPanel = display.transform.Find ("Room " + (activeRoom+1) + " Panel").GetComponent<Image>(); // change selected panel to currently active room
                    selectedPanel.color = selectedPanelColor; // highlight currently active room on "map"


                    orbs[activeRoom].gameObject.SetActive(true);
                    enemySpawners[activeRoom].gameObject.SetActive(true);

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
            orbs[activeRoom].StartShooting();
    }

    public void StopShooting()
    {
        if(initialized)
            orbs[activeRoom].StopShooting();
    }

    enum Direction { Clockwise, Counterclockwise, None };

    private void rotateRoom(int dir)
    {
        readyToSwitch = false;
        rotateDirection = dir;
        angleRotated = 0;
        orbs[activeRoom].StopShooting();
    }

    public Fire activeOrb()
    {
        return orbs[activeRoom];
    }

    public EnemySpawner activeEnemySpawner()
    {
        return enemySpawners[activeRoom];
    }
}
