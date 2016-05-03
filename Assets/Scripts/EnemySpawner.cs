using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {

    public Enemy enemyPrefab;
    public float spawnTime = 1;
    public int maxEnemies = 10;
    public Vector3 spawnDists;
    public float spawnAngle = 30;

    public List<Enemy> Enemies { get; set; }

    private int maxSpawnTries = 5;
    private float timeSinceLastSpawn = 0;

	// Use this for initialization
	void Start () {
        Enemies = new List<Enemy>();
	}
	
	// Update is called once per frame
	void Update () {
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= spawnTime && Enemies.Count < maxEnemies)
        {
            spawnEnemy();
            timeSinceLastSpawn = 0;
        }
	}

    public void DestroyEnemy(Enemy e)
    {
        Enemies.Remove(e);
        Destroy(e.gameObject);
    }

    void spawnEnemy() {
        Vector3 spawnPos = transform.position;
        int numTries = 0;
        do
        {
            spawnPos.x += spawnDists.x * (Random.value * 2 - 1);
            spawnPos.y += spawnDists.y * (Random.value * 2 - 1);
            spawnPos.z += spawnDists.z * (Random.value * 2 - 1);
            numTries++;
        } while (!tooClose(spawnPos) && numTries < maxSpawnTries);

        if (!tooClose(spawnPos))
        {
            Quaternion spawnQuat = Quaternion.Euler(0, spawnAngle * (Random.value * 2 - 1), 0);
            Enemy enemy = Instantiate(enemyPrefab, spawnPos, spawnQuat) as Enemy;
            enemy.Origin = this;
            enemy.transform.parent = transform;
            Enemies.Add(enemy);
        }
    }

    bool tooClose(Vector3 pos)
    {
        foreach (Enemy enemy in Enemies)
            if (Vector3.Distance(enemy.transform.position, pos) < enemy.minDistFromEnemies)
                return true;

        return false;
    }
}
