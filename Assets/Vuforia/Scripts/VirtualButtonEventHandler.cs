using UnityEngine;
using System.Collections.Generic;
using Vuforia;

public class VirtualButtonEventHandler : MonoBehaviour, IVirtualButtonEventHandler {

	private Transform wand;

    private GameObject trex;
    private GameObject spaceship;
	private GameObject cube;
	private GameObject sphere;

	public GameObject trexPrefab;
	public GameObject spaceshipPrefab;
	public GameObject cubePrefab;
	public GameObject spherePrefab;

  
    void Start() {

        // Search for all Children from this ImageTarget with type VirtualButtonBehaviour
        VirtualButtonBehaviour[] vbs = GetComponentsInChildren<VirtualButtonBehaviour>();
        for (int i = 0; i < vbs.Length; ++i) {
            // Register with the virtual buttons TrackableBehaviour
            vbs[i].RegisterEventHandler(this);
        }
			
		wand = GameObject.Find ("Wand").transform;

    }
 
    /// <summary>
    /// Called when the virtual button has just been pressed:
    /// </summary>
    public void OnButtonPressed(VirtualButtonAbstractBehaviour vb) {
		//Debug.Log(vb.VirtualButtonName);
		Debug.Log("Button pressed!");
        
		switch(vb.VirtualButtonName) {
		case "trex":
			trex = Instantiate(trexPrefab, wand.position + wand.up * 2.0f, Quaternion.identity) as GameObject;
			trex.transform.parent = wand;
            break;
		case "spaceship":
			spaceship = Instantiate(spaceshipPrefab, wand.position + wand.up * 2.0f, Quaternion.identity) as GameObject;
			spaceship.transform.parent = wand;
            break;
		case "cube":
			cube = Instantiate(cubePrefab, wand.position + wand.up * 2.0f, Quaternion.identity) as GameObject;
			cube.transform.parent = wand;
			break;
		case "sphere":
			sphere = Instantiate(spherePrefab, wand.position + wand.up * 2.0f, Quaternion.identity) as GameObject;
			sphere.transform.parent = wand;
			break;
		
        }
        
    }

    /// Called when the virtual button has just been released:
    public void OnButtonReleased(VirtualButtonAbstractBehaviour vb) { 
		Debug.Log("Button released!");
	}
}