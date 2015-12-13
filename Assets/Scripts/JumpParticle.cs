using UnityEngine;
using System.Collections;

public class JumpParticle : MonoBehaviour {

    public int destroyCount = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        destroyCount++;

        if (destroyCount > 1000)
        {
            Destroy(this);
        }
	}
}
