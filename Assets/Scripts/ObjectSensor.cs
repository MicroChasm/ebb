using UnityEngine;
using System.Collections;

public class ObjectSensor : MonoBehaviour {

    public string collisionTag;
    //int mask = 0;
    public bool colliding = false;

    // Use this for initialization
    void Start () {
       // mask = LayerMask.GetMask(collisionLayer);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if ((other.tag == collisionTag))
        {
            colliding = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == collisionTag)
        {
            colliding = false;
        }
    }
}
