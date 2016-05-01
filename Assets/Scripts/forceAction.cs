using UnityEngine;
using System.Collections;

public class forceAction : MonoBehaviour {
	
	LineRenderer lineRenderer;
	int x = 0;
	private int flag;

	void Awake () {
		lineRenderer = GetComponent<LineRenderer> ();
	}

	void Start(){
		flag = 0;
	}

	void Update () {
		Laser ();
	}

	void Laser(){
		RaycastHit hit;
		Ray ray = new Ray(transform.position, -transform.up);
		lineRenderer.enabled = true;         
		lineRenderer.SetPosition (0, transform.position);

		//lineRenderer.enabled = true;         
		//lineRenderer.SetPosition(0, transform.position); 
		//lineRenderer.SetPosition(1, transform.position + 5 * transform.up);   


		if (Physics.Raycast (transform.position, -1 * transform.up, out hit, Mathf.Infinity)) {
			Debug.Log ("hit" + x);
			x = x + 1;
			 
			lineRenderer.SetPosition (1, hit.point);   

			Debug.Log (hit.collider.name);

			if (hit.collider.tag == "Enemy") {
				StartCoroutine ("Select");
			}

			if (flag == 1) {
				var exp = GetComponent<ParticleSystem> ();
				exp.Play ();
				Destroy (hit.collider, exp.duration);
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
			
		}
	}
}
