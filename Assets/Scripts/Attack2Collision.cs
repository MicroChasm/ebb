using UnityEngine;
using System.Collections;

public class Attack2Collision : MonoBehaviour {

    public playerController player;
    public Renderer rend;
    public int displayCounter = 0;
    public AudioSource Attack2Sound;

    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if ((int)player.attack == 2)
        {
            displayCounter++;

        }
        else
        {
            displayCounter = 0;
        }

        if (displayCounter == 10)
        {
            Attack2Sound.PlayOneShot(Attack2Sound.clip);
        }

        if ((displayCounter > 55) && (displayCounter < 65))
        {
            rend.enabled = true;

        }
        else
        {
            rend.enabled = false;

        }

    }
}
