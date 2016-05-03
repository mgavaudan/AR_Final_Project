using UnityEngine;
using System.Collections;

public class forceAction : MonoBehaviour {
	
	LineRenderer lineRenderer;
	float hitTime = 0;
	Vector3 lastPos;
	public float waitTime = 0.5f;

	void Awake () {
		lineRenderer = GetComponent<LineRenderer> ();
	}

	void Start(){
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

		int layermask = 1 << LayerMask.NameToLayer ("ReflectTarget");
		layermask |= 1 << LayerMask.NameToLayer ("DoorLayer");
		if (Physics.Raycast (transform.position, -1 * transform.up, out hit, float.PositiveInfinity, layermask)) {
			Debug.Log ("hit" + hitTime);
			hitTime += Time.deltaTime;
			 
			lineRenderer.SetPosition (1, hit.point);  

			if (hit.collider.tag == "Enemy") {
				float c = (waitTime - hitTime) / waitTime;
				hit.transform.GetComponent<Renderer>().material.color = new Color(1,c,c);
				StartCoroutine ("Select");
			}
			else if (hit.collider.tag == "SF_Door") {
				Map m = GameObject.Find ("Map").GetComponent<Map> ();
				m.crossHall();
			}


		} else {
			hitTime = 0;
			lineRenderer.SetPosition(1, ray.GetPoint(100));
		}
	}

	void LaserCheck(){
		RaycastHit hit;

		if(Physics.Raycast(transform.position,-1*transform.up,out hit,Mathf.Infinity)){

			if (hit.collider.tag == "Enemy") {
				ParticleSystem exp = hit.transform.gameObject.GetComponent<ParticleSystem> ();
				exp.Play ();
				Destroy (hit.transform.gameObject, exp.duration);

			} 
		}

	}
		
	IEnumerator Select(){
		yield return new WaitForSeconds (waitTime);
		LaserCheck ();
	}
		

}
