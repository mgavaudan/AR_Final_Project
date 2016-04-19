using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour {

	public float fireRate = 1f;
	public float speed = 5f;
	public Transform camera;
	public float xBound = 3f;
	public float yBound = 3f;

	public GameObject missilePrefab;
	public Material missileMaterial;
	private GameObject missile;
	private Random rnd;
	private Vector3 aim;

	void Start() {
		rnd = new Random ();
		aim = Vector3.up;
	}


	// Update is called once per frame
	void Update () {
		StartCoroutine ("Shoot");

		//missile.GetComponent<Rigidbody>().AddForce(aim * speed);
	}


	IEnumerator Shoot() {
		missile = Instantiate (missilePrefab, transform.position, Quaternion.identity) as GameObject;
		missile.transform.parent = transform;
		missile.transform.GetComponent<Renderer> ().material = missileMaterial;
		missile.transform.localScale = new Vector3 (1f, 1f, 1f);
		missile.SetActive (true);

		float x = (2*Random.value-1) * xBound;
		float y = (2*Random.value-1) * yBound;
		Vector3 center = camera.position;
		center.x += x;
		center.y += y;
		aim = (center - transform.position).normalized;

		missile.GetComponent<Rigidbody>().AddForce(aim * speed);

		yield return new WaitForSeconds(fireRate);
	}

	void OnCollisionEnter (Collision col) {

	}
}
