using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {

    public Enemy enemyPrefab;
    public float spawnTime = 1;

	private int enemiesSpawned = 0;
    public int maxEnemies = 5;

    public Vector3 spawnDists;
    public float spawnAngle = 30;

    public List<Enemy> Enemies { get; set; }

    private int maxSpawnTries = 5;
    private float timeSinceLastSpawn = 0;

	private bool isCompleted = false;
	public bool IsCompleted
	{
		get {
			return isCompleted;
		}
	}

	void Start () {
        Enemies = new List<Enemy>();
	}
	
	void Update () {
		// spawn enemy every spawTime seconds
        timeSinceLastSpawn += Time.deltaTime;

		if (timeSinceLastSpawn >= spawnTime && checkEnemies())
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
		enemiesSpawned++;
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

	bool checkEnemies(){
		if (enemiesSpawned == maxEnemies) {
			isCompleted = true;
			return false;
		}
		return true;
	}

    bool tooClose(Vector3 pos)
    {
		foreach (Enemy enemy in Enemies)
			if (enemy != null) {
				if (Vector3.Distance (enemy.transform.position, pos) < enemy.minDistFromEnemies)
					return true;
			}

        return false;
    }
}
