using UnityEngine;
using System.Collections;

public class createPrefab : MonoBehaviour {

	private GameObject wand;
	private GameObject selObj;
	private Transformer s;

	// Use this for initialization
	void Start () {	
		s = GameObject.Find ("Ground").GetComponent<Transformer> ();
	}

	void OnTriggerEnter(Collider other){
		
		Debug.Log ("collided");

		if (other.gameObject.tag == "Player") {
			s.Create (other.gameObject);
		}
		else if(other.gameObject.tag == "Respawn"){
			s.Select (other.gameObject);
		}
		else if(other.gameObject.name == "Workspace"){
			s.Select (other.gameObject);
		}
	}
}
