using UnityEngine;
using System.Collections;

public class MissileHandler : MonoBehaviour {

    public Vector3 Velocity { get; set; }
    public Fire Origin { get; set; }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += Velocity * Time.deltaTime;
	}

    void OnTriggerEnter(Collider col)
    {
        Enemy e = col.gameObject.GetComponent<Enemy>();
        if(e != null)
        {
            e.Origin.DestroyEnemy(e);
            Origin.DestroyMissile(gameObject);
        }
    }
}
