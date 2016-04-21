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

    void spawnEnemy() {
        Vector3 spawnPos = transform.position;
        int numTries = 0;
        do
        {
            spawnPos.x += spawnDists.x * Random.Range(-1, 1);
            spawnPos.y += spawnDists.y * Random.Range(-1, 1);
            spawnPos.z += spawnDists.z * Random.Range(-1, 1);
            numTries++;
        } while (!tooClose(spawnPos) && numTries < maxSpawnTries);

        if (!tooClose(spawnPos))
        {
            Quaternion spawnQuat = Quaternion.Euler(0, spawnAngle * Random.Range(-1, 1), 0);
            Enemies.Add(Instantiate(enemyPrefab, spawnPos, spawnQuat) as Enemy);
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
