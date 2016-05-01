using UnityEngine;
using System.Collections;

public class forceAction : MonoBehaviour {
	
	LineRenderer lineRenderer;
	int x = 0;
	private int flag;
	Vector3 lastPos;

	void Awake () {
		lineRenderer = GetComponent<LineRenderer> ();
	}

	void Start(){
		flag = 0;
		lastPos = transform.position;
	}

	void Update () {
		Laser ();
	}

	void Laser(){
		RaycastHit hit;
		Ray ray = new Ray(transform.position, -transform.up);
		lineRenderer.enabled = true;         
		lineRenderer.SetPosition (0, transform.position);

		if (Physics.Raycast (transform.position, -1 * transform.up, out hit, Mathf.Infinity)) {
			Debug.Log ("hit" + x);
			x = x + 1;

			hit.transform.GetComponent<Renderer>().material.color = new Color(x,1,1);

<<<<<<< HEAD
			if (hit.collider.tag == "Enemy") {
				StartCoroutine ("Select");

			}

			if (hit.collider.tag == "Door") {
				StartCoroutine ("MoveForward");
			}
=======
			 
			lineRenderer.SetPosition (1, hit.point);  
>>>>>>> eceee67b9c4ac8eba33774aef78372bdf7b6f7ca

			if (flag == 1) {
				ParticleSystem exp = hit.transform.gameObject.GetComponent<ParticleSystem> ();
				exp.Play ();
				Destroy (hit.transform.gameObject, exp.duration);

				flag = 0;
			}
			else if (hit.collider.tag == "Enemy") {
				StartCoroutine ("Select");
			}


		} else {
		
			lineRenderer.SetPosition(1, ray.GetPoint(100));
		
		}
	}

	bool LaserCheck(){
		RaycastHit hit;

		if(Physics.Raycast(transform.position,-1*transform.up,out hit,Mathf.Infinity)){

			if (hit.collider.tag == "Enemy") {
				return true;
			} 
		}

		return false;
	}


	IEnumerator Select(){
		yield return new WaitForSeconds (0.5f);
		if (LaserCheck ()) {
			flag = 1;
		}
	}

	bool ForceUp(){
		if (transform.position.z - lastPos.z > 2.0f) {
			lastPos = transform.position; 
			return true;
		}
		lastPos = transform.position;
		return false;
	}

	IEnumerator MoveForward(){
		yield return new WaitForSeconds (0.5f);

	}
}
