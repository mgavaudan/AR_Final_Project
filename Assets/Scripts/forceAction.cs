using UnityEngine;
using System.Collections;

public class forceAction : MonoBehaviour {
	
	LineRenderer lineRenderer;
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


		if(Physics.Raycast(transform.position, transform.up, out hit,Mathf.Infinity)){

			lineRenderer.enabled = true;         
			lineRenderer.SetPosition(0, transform.position); 
			lineRenderer.SetPosition(1, hit.point);   

			if(hit.collider.tag == "Enemy"){
				StartCoroutine ("Select");
			}

			if (flag == 1) {
				var exp = GetComponent<ParticleSystem>();
				exp.Play();
				Destroy(hit.collider, exp.duration);
			}
		}
	}

	bool LaserCheck(){
		RaycastHit hit;

		if(Physics.Raycast(transform.position,Vector3.forward,out hit,Mathf.Infinity)){

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
