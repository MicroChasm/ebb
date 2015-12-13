using UnityEngine;
using System.Collections;

public class WallCollisionLeft : MonoBehaviour {

    public bool wallLeft;
	public bool wallNearLeft;
    public GroundCollision grounded;
    public playerController player;


    // Use this for initialization
    void Start () {
	
	}


    //check wall left

	public IEnumerator Deactivate(){
		wallNearLeft = false;
		wallLeft = false;
		gameObject.SetActive(false);
		yield return new WaitForSeconds(.1f);
		gameObject.SetActive(true);
	}

    void OnTriggerStay2D(Collider2D other)
    {
		wallNearLeft = true;

        if (other.tag == "Ground")
            {
            Debug.Log("wall collide left");
        }


        if ((other.tag == "Ground") && (grounded.grounded == false) && (player.currentSpeedy <= 0) && (Input.GetKey ("a")))
		{
            wallLeft = true;

        }
    }

	void OnTriggerExit2D(Collider2D other)
	{
		wallNearLeft = false;
		if (other.tag == "Ground"){
			wallLeft = false;
		}

	}




    // Update is called once per frame
    void Update () {
	
		if (grounded.grounded == true){
			wallLeft = false;

		}

	}
}
