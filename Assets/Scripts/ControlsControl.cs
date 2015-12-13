using UnityEngine;
using System.Collections;

public class ControlsControl : MonoBehaviour {

    public float timeCounter = 0;


	// Use this for initialization


    // Update is called once per frame
    void Update() {
        timeCounter++;

        if (timeCounter > 250)
        {
            Debug.Log("controls working");

            Destroy(gameObject);
        }
    }
}
