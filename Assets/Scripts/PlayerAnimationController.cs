using UnityEngine;
using System.Collections;

public class PlayerAnimationController : MonoBehaviour {

    public playerController player;
    public GroundCollision grounded;
    Animator anim;


    void FixedUpdate()
    {
        anim.SetFloat("VerticalSpeed", player.currentSpeedy);

        //jumping state
        if ((grounded.grounded == false) && (player.currentSpeedy > 0))
        {
            anim.SetBool("Up", true);
        }
        else
        {
            anim.SetBool("Up", false);
        }

        //falling state
        if ((grounded.grounded == false) && (player.currentSpeedy <= 0))
        {
            anim.SetBool("Down", true);
        }
        else
        {
            anim.SetBool("Down", false);
        }

        if (grounded.grounded == true)
        {
            anim.SetBool("Grounded", true);
            anim.SetBool("Landing", false);
        }
        if (grounded.grounded == false)
        {
            player.groundCounter = 0;
            anim.SetBool("Grounded", false);
        }

        //landing state
        if ((grounded.grounded == true) && (player.groundCounter < 47))
        {
            player.groundCounter++;
        }
        
        //falling fast enough for a landing animation
        if (player.currentSpeedy < -80)
        {
            player.fastFall = true;
        }
        
        if (player.groundCounter == 45)
        {
            player.fastFall = false;
        }

        if ((player.groundCounter > 0) && (player.groundCounter < 47))
        {
            player.landing = true;
        }

        if ((player.groundCounter == 0) || (player.groundCounter == 47))
        {
            player.landing = false;
        }
      
        if (player.landing == true)
        {
            anim.SetBool("Landing", true);
        }
        if (player.landing == false)
        {
            anim.SetBool("Landing", false);
        }
        

        if ((Mathf.Abs(player.currentSpeedx) > 0) && (grounded.grounded == true))
        {
            anim.SetFloat("Speed", Mathf.Abs(player.currentSpeedx));
        }
        

        if ((Mathf.Abs(player.currentSpeedx) <= 0) && (grounded.grounded == true))
        {
            anim.SetFloat("Speed", Mathf.Abs(player.currentSpeedx));
        }
        
        if (player.fastFall == true)
        {
            anim.SetBool("FastFall", true);
        }

        if (player.fastFall == false)
        {
            anim.SetBool("FastFall", false);
        }
    }
}
