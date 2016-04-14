using UnityEngine;
using System.Collections;

public class Transformer : MonoBehaviour {

	private GameObject selObj;
	private GameObject wand;
//	private Control camControl;
	private Vector3 preLoc;
	private Vector3 deltaMov;


	private string groundObj;
	private string workObj;
	private Vector3 deltaMovWorkspace;

	private GameObject prevObj;

	public int flag = 0;



	// Use this for initialization
	void Start () {
		wand = GameObject.Find ("Wand");
		deltaMov = Vector3.zero;
		preLoc = wand.transform.position;
//		camControl = GameObject.Find ("ARCamera").GetComponent<Control> ();
	}
	
	// Update is called once per frame
	void Update () {
		deltaMov = (wand.transform.position - preLoc);

		if(flag==2){
			Rotate ();
		}
		else if(flag == 1){
			Scale ();
		}

		preLoc = wand.transform.position;
	}

	public void Create(GameObject o){
		UnSelect (o);
		selObj = Instantiate (o, transform.position, Quaternion.identity) as GameObject;
		selObj.transform.localScale = new Vector3(2,2,2);
		selObj.tag = "Respawn";
		selObj.transform.parent = transform;
		selObj.GetComponent<Renderer> ().material.color = Color.green;

		if (o.name == "Trex") {
			selObj.transform.localScale = new Vector3(2,2,2);
			selObj.transform.localRotation = o.transform.localRotation;
		}
	}

	public void Select(GameObject o){
		UnSelect (o);
		selObj = o;
		if (o.name == "Workspace") {
//			GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
//			plane.transform.parent = o.transform;
//			plane.GetComponent<Renderer> ().material.color = Color.green;
		} else {
			selObj.GetComponent<Renderer> ().material.color = Color.green;
		}

	}
		

	public void UnSelect(GameObject o){
		if (selObj) {
			if(selObj != o){
				selObj.GetComponent<Renderer> ().material.color = Color.white;
			}
		}
	}

	public void Scale(){
		selObj.transform.localScale = new Vector3 (selObj.transform.localScale.x + deltaMov.x, selObj.transform.localScale.y + deltaMov.x, selObj.transform.localScale.z + deltaMov.x);
	}

	public void Rotate(){

		selObj.transform.RotateAround(selObj.transform.position, selObj.transform.up, deltaMov.x * 180);
		selObj.transform.RotateAround(selObj.transform.position, selObj.transform.forward, deltaMov.y * 180);
		selObj.transform.RotateAround(selObj.transform.position, selObj.transform.right, deltaMov.z * 180);

	}

	public void Translate(){
		selObj.transform.position = wand.transform.position + wand.transform.forward * 2;
	}

	public void Delete(){
		Destroy (selObj);
	}

	public void Workspace(){
		UnSelect (selObj);
		GameObject works = GameObject.Find ("Workspace");

		foreach (Transform child in works.transform) {
			GameObject.Destroy(child.gameObject);
		}

		groundObj = selObj.name;

		selObj = Instantiate (selObj, works.transform.position, Quaternion.identity) as GameObject;
		selObj.transform.localScale = new Vector3(2,2,2);
		selObj.tag = "Respawn";
		selObj.transform.parent = works.transform;
		selObj.GetComponent<Renderer> ().material.color = Color.green;

		workObj = selObj.name;
	}

	public void Ground(){
		GameObject works = GameObject.Find ("Workspace");
		deltaMovWorkspace = selObj.transform.position - works.transform.position;

		selObj = GameObject.Find (groundObj);
		prevObj = GameObject.Find (workObj);
		selObj.transform.localRotation = prevObj.transform.localRotation;
		selObj.transform.localScale = prevObj.transform.localScale;
		selObj.transform.position += deltaMovWorkspace;
	}
}
