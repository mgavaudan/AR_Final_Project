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

			 
			lineRenderer.SetPosition (1, hit.point);  

			if (flag == 1) {
				ParticleSystem exp = hit.transform.gameObject.GetComponent<ParticleSystem> ();
				exp.Play ();
				Destroy (hit.transform.gameObject, exp.duration);

				flag = 0;
			}
			else if (hit.collider.tag == "Enemy") {
				hit.transform.GetComponent<Renderer>().material.color = new Color(x,1,1);
				StartCoroutine ("Select");
			}
			else if (hit.collider.tag == "SF_Door") {
//				StartCoroutine ("Select");
				Map m = GameObject.Find ("Map").GetComponent<Map> ();
				m.moveForward ();
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
		

}
