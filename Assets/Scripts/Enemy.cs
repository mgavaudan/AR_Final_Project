using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public float minDistFromEnemies = 0.3f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    void OnTriggerEnter(Collider col)
    {
        // TODO: remove from enemy spawner list
        Destroy(col.gameObject);
        Destroy(gameObject);
    }
}
