using UnityEngine;
using System.Collections;

public class Selector : MonoBehaviour {

	private GameObject selected;

	private Texture tex;
	private Color col;

	public bool mobile = true;

	RaycastHit hit;


	void Update () {

		if ( Input.touchCount != 0 || Input.GetMouseButtonDown(0))
		{
			Ray ray;
			Touch touch;

			if (mobile) {
				touch = Input.touches [0];
				ray = Camera.main.ScreenPointToRay (touch.position);
			}
			else {
				ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			}

			// if an object is hit
			if ( Physics.Raycast(ray, out hit, Mathf.Infinity ) )
			{
				if (selected) {
					// unselecting current object
					if (hit.transform.gameObject.name == selected.name) {
						UnSelectObj ();
					} 
					// changing from one object to another
					else {
						UnSelectObj ();
						SelectObj ();

					}
				}
				// selecting new object
				else {
					SelectObj ();
				}

			}
		}

	}


	void SelectObj(){

		// Check out what object is hit -  && touch.phase == TouchPhase.Began
		if(hit.transform.gameObject.tag == ("Player") ){

			selected = hit.transform.gameObject;
			Transformer s = GameObject.Find("Ground").GetComponent<Transformer> ();
			s.Create(selected);
		}   

		else if(hit.transform.gameObject.tag == ("Respawn") ){
			// save texture
			Material mat = hit.transform.gameObject.GetComponent<Renderer> ().material;
			tex = mat.mainTexture;
			col = mat.color;

			// then set them to null and green
			mat.mainTexture = null;
			mat.color = Color.green;

			selected = hit.transform.gameObject;
			Transformer s = GameObject.Find("Ground").GetComponent<Transformer> ();
			s.Select(hit.transform.gameObject);
		}   
	

	}




	void UnSelectObj(){

		if(selected.tag == ("Player")){
//			Transformer s = selected.GetComponent<Transformer> ();
//			s.UnSelect();
		}   
		else if(selected.tag == ("Respawn")){
			selected.GetComponent<Renderer> ().material.mainTexture=tex;
			selected.GetComponent<Renderer> ().material.color=col;

//			Transformer s = selected.GetComponent<Transformer> ();
//			s.UnSelect();
		}   
			

	}



}
