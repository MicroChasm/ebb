using UnityEngine;
using System.Collections;

public class BirdSpriteAnimator : MonoBehaviour {

    public int frame = 1;
    Animator anim;
    public Rigidbody2D body;
    private bool animating = true;

    enum birdOrientation
    {
        BIRD_UP = 0,
        BIRD_MID_UP = 1,
        BIRD_MID = 2,
        BIRD_MID_LOW = 3
    };

    private static birdOrientation[] animationNames =
  {
     birdOrientation.BIRD_MID, birdOrientation.BIRD_MID_UP, birdOrientation.BIRD_UP,
     birdOrientation.BIRD_UP, birdOrientation.BIRD_MID_UP, birdOrientation.BIRD_MID,
     birdOrientation.BIRD_MID, birdOrientation.BIRD_MID_LOW, birdOrientation.BIRD_UP,
     birdOrientation.BIRD_UP, birdOrientation.BIRD_MID_UP, birdOrientation.BIRD_MID,
     birdOrientation.BIRD_MID
  }; 

    // Use this for initialization
    void Start () {

        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        if (animating)
        {
            frame++;
            if (frame > 56)
            {
                frame = 1;
            }
            anim.SetFloat("Frame", frame);
            setAnimation();
        }
    }

    void setAnimation()
    {
        int xDirection = 1;
        int yDirection = 1;
        float rotation = body.rotation % 360;
        Vector3 theScale = transform.localScale;

        //frameTime = animator.animation[animationName].time;
        if (rotation < 0)
        {
            rotation = 360 + rotation;
        }

        anim.SetInteger("Orientation", (int)animationNames[Mathf.FloorToInt(rotation / 30)]);

        if (rotation > 90 && rotation < 270)
        {
            xDirection = -1;
        }
        theScale.x = Mathf.Abs(transform.localScale.x) * xDirection;

        if (rotation > 240 && rotation < 300)
        {
            yDirection = -1;
        }
        theScale.y = Mathf.Abs(transform.localScale.y) * yDirection;
        transform.localScale = theScale;
    }

    public void StopAnimation()
    {
        animating = false;
    }
}
