using UnityEngine;
using System.Collections;

public class MissileHandler : MonoBehaviour {

    public Vector3 Velocity { get; set; }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += Velocity * Time.deltaTime;
	}
}
