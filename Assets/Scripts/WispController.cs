using UnityEngine;
using System.Collections;

public class WispController : MonoBehaviour {
    public float groundDistance = 2;
    public float forwardDistance = 1;
    RaycastHit2D[] raycastHits = new RaycastHit2D[1];
    CircleCollider2D circleCollider;
    public float speed = 1;
    int groundLayerMask = 0;
    int direction = 1;

    // Use this for initialization
    void Start () {
        circleCollider = GetComponent<CircleCollider2D>();
        groundLayerMask = LayerMask.GetMask("Ground");
    }
	
	// Update is called once per frame
	void Update () {
        int numRaycastHits = 0;
        float upVel = 0;
        float distanceToGround = 0;
        Vector2 startLine;
        Vector2 endLine;

        startLine = circleCollider.transform.position;
        endLine = new Vector2(circleCollider.transform.position.x, circleCollider.transform.position.y - groundDistance);

        Debug.DrawLine(startLine, endLine, Color.red);
        numRaycastHits = Physics2D.LinecastNonAlloc(startLine,
                                                    endLine,
                                                    raycastHits,
                                                    groundLayerMask);
        if (numRaycastHits > 0)
        {
            distanceToGround = Vector2.Distance(transform.position, raycastHits[0].point);
            if (distanceToGround < (groundDistance/2))
            {
                upVel = 1;
            }
            else if (distanceToGround > groundDistance)
            {
                upVel = -1;
            }
            upVel += Random.Range(-1, 1) / 10;
            transform.position += (Vector3.right + Vector3.up * upVel) * direction * Time.deltaTime * speed;
        }
        else
        {
            direction *= -1;
            transform.position += new Vector3(speed, 0, 0) * Time.deltaTime * direction;
        }

        startLine = circleCollider.transform.position;
        endLine = new Vector2(circleCollider.transform.position.x + forwardDistance*direction, circleCollider.transform.position.y);

        Debug.DrawLine(startLine, endLine, Color.red);
        numRaycastHits = Physics2D.LinecastNonAlloc(startLine,
                                                    endLine,
                                                    raycastHits,
                                                    groundLayerMask);
        if (numRaycastHits > 0)
        {
            direction *= -1;
        }
    }
}
