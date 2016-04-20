using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fire : MonoBehaviour {

	public float fireRate = 1f;
	public float speed = 5f;
	public Transform camera;
	public float xBound = 3f;
	public float yBound = 3f;
	public float maxRadius = 100f;
	public GameObject missilePrefab;
	public Material missileMaterial;

	private List<GameObject> missiles;

	void Start() {
		missiles = new List<GameObject> ();

		InvokeRepeating ("Shoot",1, fireRate);
	}


	// Update is called once per frame
	void Update () {


	}


	void FixedUpdate () {
		// for debug
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");
		Vector3 movement = new Vector3 (moveHorizontal, moveVertical, 0);
		camera.transform.Translate (movement);

		// clean up OOB missiles
		List<GameObject> toRemove = new List<GameObject>();
		foreach (GameObject m in missiles) {
			if (Vector3.Distance(m.transform.position, Vector3.zero) > maxRadius) {
				toRemove.Add (m);
			}
		}
		foreach (GameObject x in toRemove) {
			missiles.Remove (x);
			Destroy (x);
		}
	}


	void Shoot() {
		GameObject missile = Instantiate (missilePrefab, transform.position, Quaternion.identity) as GameObject;
		//missile.transform.parent = transform;
		missile.transform.GetComponent<Renderer> ().material = missileMaterial;
		missile.transform.localScale = new Vector3 (1f, 1f, 1f);
		missile.SetActive (true);

		float x = (2*Random.value-1) * xBound;
		float y = (2*Random.value-1) * yBound;
		Vector3 center = camera.position;
		center.x += x;
		center.y += y;
		Vector3 aim = (center - transform.position).normalized;

		missile.GetComponent<Rigidbody>().velocity = aim*speed;

		missiles.Add (missile);
	}

	void OnCollisionEnter (Collision col) {

	}
}
