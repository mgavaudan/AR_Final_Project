using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fire : MonoBehaviour {

	public float fireRate = 1f;
    public Transform target;
	public float speed = 5f;
	public float xBound = 3f;
	public float yBound = 3f;
	public float maxRadius = 100f;
	public MissileHandler missilePrefab;

	private List<GameObject> missiles;

	void Start() {
		missiles = new List<GameObject>();
	}

	void Update () {
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");
		Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0);
        transform.position += movement * speed;

		// clean up out-of-bounds missiles
		List<GameObject> toRemove = new List<GameObject>();
		foreach (GameObject m in missiles) {
			if (Vector3.Distance(m.transform.position, Vector3.zero) > maxRadius) {
				toRemove.Add(m);
			}
		}
		foreach (GameObject x in toRemove) {
			missiles.Remove(x);
			Destroy(x);
		}
	}

	public void StartShooting () {
		InvokeRepeating("Shoot", 0, fireRate);
	}

	public void StopShooting() {
		CancelInvoke();
	}

	private void Shoot() {
		MissileHandler missile = Instantiate(missilePrefab, transform.position, Quaternion.identity) as MissileHandler;
        missile.transform.parent = transform;

		float x = (2*Random.value-1) * xBound;
		float z = (2*Random.value-1) * yBound;
        Vector3 center = target.position;
		center.x += x;
		center.z += z;
		Vector3 aim = (center - transform.position).normalized;

		missile.Velocity = aim*speed;

		missiles.Add(missile.gameObject);
	}
}
