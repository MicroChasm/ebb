using UnityEngine;
using System.Collections;

public class CrowAnimationController : MonoBehaviour {

    public int unfurlCounter;
    public int trigger;
    Animator anim;

	
    void Start() {
                anim= GetComponent<Animator>();
        }

	// Update is called once per frame
	void FixedUpdate () {
        
        unfurlCounter++;
        if (unfurlCounter > 239)
        {
            trigger = Random.Range(0, 10);
            anim.SetInteger("UnfurlCounter", trigger);
            unfurlCounter = 0;
        }

    }
}
