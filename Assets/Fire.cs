using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour {

	public float fireRate = 1f;
	public float speed = 5f;

	public GameObject missilePrefab;
	private GameObject missile;


	// Update is called once per frame
	void Update () {
		StartCoroutine("Shoot");

	}


	IEnumerator Shoot() {
		missile = Instantiate (missilePrefab, transform.position, Quaternion.identity) as GameObject;
		missile.transform.parent = transform;

		missile.GetComponent<Rigidbody>().AddForce(Vector3.up * speed);

		yield return new WaitForSeconds(fireRate);
	}
}
