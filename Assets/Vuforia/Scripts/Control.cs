using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour {

	private bool isWorkspace = false;
	private Transformer s;

	public int flag = 0;

	// Use this for initialization
	void Start () {
		s = GameObject.Find("Ground").GetComponent<Transformer> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{

		GUI.Box (new Rect (10, 10, 150, 180), "Control Mode");
			
		if (GUI.Button (new Rect (20, 40, 120, 20), "Translate")) {
			s.Translate ();
		}

		if (flag == 0 || flag == 1) {
			if (GUI.Button (new Rect (20, 70, 120, 20), "Rotate")) {
				s.flag =2;
				flag = 2;
			}
		} else {
			if (GUI.Button (new Rect (20, 70, 120, 20), "Stop Rotation")) {
				s.flag =0;
				flag = 0;
			}
		}
		if (flag == 0 || flag == 2) {
			if (GUI.Button (new Rect (20, 100, 120, 20), "Scale")) {
				s.flag = 1;
				flag = 1;
			}
		} else {
			if (GUI.Button (new Rect (20, 100, 120, 20), "Stop Scaling")) {
				s.flag = 0;
				flag = 0;
			}
		}

		if (GUI.Button (new Rect (20, 160, 120, 20), "Delete")) {
			s.Delete ();
		}
			
		if (isWorkspace == false) {
			if (GUI.Button (new Rect (20, 130, 120, 20), "Ground Coor")) {
				isWorkspace = true;
				s.Workspace ();
			}
		} 
		else {
			if (GUI.Button (new Rect (20, 130, 120, 20), "Workspace Coor")) {
				isWorkspace = false;
				s.Ground ();
			}
		}

			

	}

}
