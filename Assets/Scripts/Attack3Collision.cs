using UnityEngine;
using System.Collections;

public class Attack3Collision : MonoBehaviour
{
  public playerController player;
  public Renderer rend;
  public GroundCollision grounded;
  public int displayCounter = 0;
  public bool attack3Running = false;
  public ParticleSystem ParticlesAttack3;
  public AudioSource Attack3Sound;

  // Use this for initialization
  void Start()
  {
    rend = GetComponent<Renderer>();
    rend.enabled = false;

  }

  // Update is called once per frame
  void Update()
  {
    if ((int)player.attackState == 3)
    {
      if (player.attack3Pound)
      {
        displayCounter++;

      }
      else
      {
        displayCounter = 0;
      }

      if (displayCounter == 1)
      {
        Attack3Sound.PlayOneShot(Attack3Sound.clip);
      }

      if ((displayCounter > 3) && (displayCounter < 30))
      {
        rend.enabled = true;
        attack3Running = true;
        ParticlesAttack3.emissionRate = 10000;
      }
      else
      {
        rend.enabled = false;
        attack3Running = false;
        ParticlesAttack3.emissionRate = 0;
      }
    }
    else
    {
      rend.enabled = false;
      ParticlesAttack3.emissionRate = 0;
    }
  }
}
