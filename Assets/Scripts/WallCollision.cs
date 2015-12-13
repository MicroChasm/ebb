using UnityEngine;
using System.Collections;

public class WallCollision : MonoBehaviour {
    public bool wallRight;
    public bool wallNearRight;
    public bool wallLeft;
    public bool wallNearLeft;
    public GroundCollision grounded;
    public playerController player;


    // Use this for initialization
    void Start()
    {

    }


    //check wall left

    public IEnumerator Deactivate()
    {
        wallNearRight = false;
        wallRight = false;
        wallNearLeft = false;
        wallLeft = false;
        gameObject.SetActive(false);
        yield return new WaitForSeconds(.1f);
        gameObject.SetActive(true);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        wallNearRight = true;

        if ((other.tag == "Ground") && (grounded.grounded == false) && (player.currentSpeedy <= 0) && (Input.GetKey("d")))
        {
            wallRight = true;

        }

        if ((other.tag == "Ground") && (grounded.grounded == false) && (player.currentSpeedy <= 0) && (Input.GetKey("a")))
        {
            wallLeft = true;

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        wallNearRight = false;
        if (other.tag == "Ground")
        {
            wallRight = false;
        }

    }


    /*void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Ground")
        {
            wallLeft = false;
			}
    }
*/

    // Update is called once per frame
    void Update()
    {

        if (grounded.grounded == true)
        {
            wallRight = false;

        }

    }
}