using UnityEngine;
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
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (initialized)
        {
            float forceAngle = (Quaternion.Inverse(startQuat) * roomSwitcher.rotation).eulerAngles.z;
            Debug.Log(forceAngle);
            if (readyToSwitch && Vector3.Angle(normal, roomSwitcher.up) < normalAngleEpsilon)
            {
                if (forceAngle > switchAngle)
                    rotateRoom(1);
                else if (forceAngle < -switchAngle)
                    rotateRoom(-1);
            }

            if (rotateDirection == 0 && Mathf.Abs(forceAngle) < switchAngle * 0.75f)
                readyToSwitch = true;

            if (rotateDirection != 0)
            {
                float rotateAngle = 360 / numRooms / rotationTime * Time.deltaTime;
                transform.Rotate(Vector3.up, rotateDirection * rotateAngle, Space.World);
                // TODO: make arrow rotate as child of RoomManager

                angleRotated += rotateAngle;
                if (angleRotated >= 360 / numRooms)
                {
                    // TODO: delete arrow here

                    orbs[activeRoom].gameObject.SetActive(false);
                    enemySpawners[activeRoom].gameObject.SetActive(false);

                    activeRoom = (activeRoom + rotateDirection) % numRooms;
                    if (activeRoom < 0)
                        activeRoom += numRooms;

                    orbs[activeRoom].gameObject.SetActive(true);
                    orbs[activeRoom].StartShooting();
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
