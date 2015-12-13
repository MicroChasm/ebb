using UnityEngine;
using System.Collections;

public class Attack1Collision : MonoBehaviour {

    public playerController player;
    public Renderer rend;
    public int displayCounter = 0;
    public AudioSource Attack1Sound;
         
    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if ((int)player.attack == 1)
        {
            displayCounter++;

        }
        else
        {
            displayCounter = 0;
        }

        if ((displayCounter > 3) && (displayCounter < 13))
        {
            rend.enabled = true;
            
        }
        else if ((displayCounter > 70) && (displayCounter < 80))
            {
                rend.enabled = true;

            }
            else
        {
            rend.enabled = false;

        }

        if (displayCounter == 1)
        {
            Attack1Sound.PlayOneShot(Attack1Sound.clip);
        }

        if (displayCounter == 65)
        {
            Attack1Sound.PlayOneShot(Attack1Sound.clip);
        }

    }
}
