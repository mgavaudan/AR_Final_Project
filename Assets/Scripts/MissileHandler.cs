using UnityEngine;
using System.Collections;

public class MissileHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnCollisionEnter (Collision col) {
		Debug.Log ("collision");
		if (col.transform.name == "Orb") {
			// hit the orb
			Debug.Log("Orb hit");
		} else {
			// hit the lightsaber
			transform.GetComponent<Rigidbody>().velocity *= -1;
		}
	}
}
