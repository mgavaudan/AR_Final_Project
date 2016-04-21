using UnityEngine;
using System.Collections.Generic;

public class LightSaber : MonoBehaviour {

    public EnemySpawner enemySpawner;
    public int aimAssistDirsPerQuadrant = 2;
    public float aimAssistMaxConeAngle = 15;
    public int aimAssistNumCones = 3;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    void OnCollisionEnter(Collision col) {
        MissileHandler missile = col.rigidbody.gameObject.GetComponent<MissileHandler>();
        if (missile != null)
        {
            Vector3 reflectDir = Vector3.Reflect(missile.Velocity, col.contacts[0].normal);
            Vector3 aimAssistDir = sampleDirs(col.contacts[0].point, reflectDir);
            missile.Velocity = aimAssistDir.normalized * missile.Velocity.magnitude;
        }
	}

    private Vector3 sampleDirs(Vector3 centerPos, Vector3 centerDir)
    {
        int layerMask = 1 << LayerMask.NameToLayer("ReflectTarget");

        Ray r = new Ray(centerPos, centerDir);
        if (Physics.Raycast(r, float.PositiveInfinity, layerMask))
            return centerDir;

        foreach(Vector3 perpDir in genPerpDirs(centerPos, centerDir))
        {
            r = new Ray(centerPos, centerDir + perpDir);
            if (Physics.Raycast(r, float.PositiveInfinity, layerMask))
                return centerDir + perpDir;
        }

        return centerDir;
    }

    private List<Vector3> genPerpDirs(Vector3 centerPos, Vector3 centerDir)
    {
        List<Vector3> perpDirs = new List<Vector3>();

        Vector3 perpDir1 = Vector3.up;
        if (centerDir == perpDir1 || centerDir == -perpDir1)
            perpDir1 = Vector3.right;

        float angle = Vector3.Angle(centerDir, perpDir1);
        if (angle > 90)
            perpDir1 = Vector3.RotateTowards(perpDir1, centerDir,
                Mathf.Deg2Rad * (angle - 90), float.PositiveInfinity);
        else
            perpDir1 = Vector3.RotateTowards(perpDir1, -centerDir,
                Mathf.Deg2Rad * (90 - angle), float.PositiveInfinity);

        Vector3 perpDir2 = Vector3.Cross(centerDir, perpDir1);

        for (int c = 0; c < aimAssistNumCones; c++)
        {
            float perpMagnitude = centerDir.magnitude * Mathf.Tan(Mathf.Deg2Rad
                * aimAssistMaxConeAngle * c / aimAssistNumCones);
            float iToRads = Mathf.PI / (2 * aimAssistDirsPerQuadrant);
            for (int i = 0; i < aimAssistDirsPerQuadrant; i++)
            {
                Vector3 v1 = Vector3.RotateTowards(perpDir1, perpDir2, iToRads * i, float.PositiveInfinity);
                v1.Normalize();
                v1 *= perpMagnitude;
                perpDirs.Add(v1);
                perpDirs.Add(-v1);

                Vector3 v2 = Vector3.RotateTowards(perpDir1, -perpDir2, iToRads * i, float.PositiveInfinity);
                v2.Normalize();
                v2 *= perpMagnitude;
                perpDirs.Add(v2);
                perpDirs.Add(-v2);
            }
        }

        return perpDirs;
    }
}
