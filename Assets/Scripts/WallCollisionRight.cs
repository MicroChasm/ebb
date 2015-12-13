using UnityEngine;
using System.Collections;

public class WallCollisionRight : MonoBehaviour {

	public bool wallRight;
	public bool wallNearRight;
    public bool wallLeft;
    public bool wallNearLeft;
    public GroundCollision grounded;
    public playerController player;
    BoxCollider2D boxCollider;
    
    void Start () {
        boxCollider = GetComponent<BoxCollider2D>();
    }
	
	public IEnumerator Deactivate(){
        ResetWallState();
        gameObject.SetActive(false);
		yield return new WaitForSeconds(.5f);
		gameObject.SetActive(true);
	}

    void OnTriggerStay2D(Collider2D other)
    {
        bool onWall = false;

        if (other.tag == "Ground")
        {
            onWall = (grounded.grounded == false) && (player.currentSpeedy <= 0);
            if (other.OverlapPoint(new Vector2(transform.position.x - boxCollider.size.x/2, transform.position.y)))
            {
                wallNearLeft = true;
                wallLeft = onWall && Input.GetKey("a");
            }
            else if (other.OverlapPoint(new Vector2(transform.position.x + boxCollider.size.x / 2, transform.position.y)))
            {
                wallNearRight = true;
                wallRight = onWall && Input.GetKey("d");
            }
        }
    }

    void ResetWallState()
    {
        wallLeft = false;
        wallRight = false;
        wallNearRight = false;
        wallNearLeft = false;
    }
	
	void OnTriggerExit2D(Collider2D other)
	{

        if (other.tag == "Ground"){
            ResetWallState();
        }
    }	
	
	void Update () {

        if (grounded.grounded == true)
        {
            wallRight = false;
            wallLeft = false;
		}
	}
}
