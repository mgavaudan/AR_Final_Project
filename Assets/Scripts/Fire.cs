using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour {

	public float fireRate = 1f;
	public float speed = 5f;
	public Transform camera;
	public float xBound = 3f;
	public float yBound = 3f;
	public int maxMissiles = 10;
	public GameObject missilePrefab;
	public Material missileMaterial;

	private GameObject[] missiles;
	private Vector3 aim;
	private int count = 0;

	void Start() {
		count = 0;
		aim = Vector3.up;
		missiles = new GameObject[maxMissiles];
	}


	// Update is called once per frame
	void Update () {
		StartCoroutine ("Shoot");

		Debug.Log (missiles[0].transform.position);
		//missile.GetComponent<Rigidbody>().AddForce(aim * speed);
	}

	// for debug
	void FixedUpdate () {
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0, moveVertical);

		camera.transform.Translate (movement);
	}


	IEnumerator Shoot() {
		GameObject missile = Instantiate (missilePrefab, transform.position, Quaternion.identity) as GameObject;
		//missile.transform.parent = transform;
		missile.transform.GetComponent<Renderer> ().material = missileMaterial;
		missile.transform.localScale = new Vector3 (1f, 1f, 1f);
		missile.SetActive (true);

		float x = (2*Random.value-1) * xBound;
		float y = (2*Random.value-1) * yBound;
		Vector3 center = camera.position;
		//center.x += x;
		//center.y += y;
		aim = -(center - transform.position).normalized;

		missile.GetComponent<Rigidbody>().AddForce(aim * speed);

		count = count % maxMissiles;
		if (missiles [count] != null)
			Destroy (missiles [count]);
		missiles [count] = missile;
		count++;

		yield return new WaitForSeconds(fireRate);
	}

	void OnCollisionEnter (Collision col) {

	}
}
