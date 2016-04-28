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

		//lineRenderer.enabled = true;         
		//lineRenderer.SetPosition(0, transform.position); 
		//lineRenderer.SetPosition(1, transform.position + 5 * transform.up);   


		if(Physics.Raycast(transform.position, -1*transform.up, out hit,Mathf.Infinity)){
			Debug.Log ("hit" + x);
			x = x + 1;
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
