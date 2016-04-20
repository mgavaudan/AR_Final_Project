using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fire : MonoBehaviour {

	public float fireRate = 1f;
	public float speed = 5f;
	public Transform camera;
	public float xBound = 3f;
	public float yBound = 3f;
	public GameObject missilePrefab;
	public Material missileMaterial;

	private List<GameObject> missiles;
	private Vector3 aim;

	void Start() {
		aim = Vector3.up;
		missiles = new List<GameObject> ();

		InvokeRepeating ("Shoot",1, fireRate);
	}


	// Update is called once per frame
	void Update () {


	}

	// for debug
	void FixedUpdate () {
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, moveVertical, 0);

		camera.transform.Translate (movement);
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
		aim = (center - transform.position).normalized;

		missile.GetComponent<Rigidbody>().velocity = aim*speed;

		missiles.Add (missile);
	}

	void OnCollisionEnter (Collision col) {

	}
}
