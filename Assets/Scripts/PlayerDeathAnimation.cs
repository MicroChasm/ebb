using UnityEngine;
using System.Collections;

public class PlayerDeathAnimation : MonoBehaviour 
{
  public FluidSimulator fluid;
  public ParticlesArea particlesArea;
  public float startRadius = 5.0f;
  public Vector3 startOffset = Vector3.zero;
  public int numParticles = 1000;
  public int randomLocations = 200;
  public int maxHits = 200;
  public float velocity = 1.0f;

  public bool playing = false;
  public bool firstFrame = true;
  public int addParticleFrames = 10;

  private Sprite targetSprite;
  private SpriteRenderer targetSpriteRenderer;
  private RaycastHit rayHit = new RaycastHit();
  private Collider fluidCollider;
  private GameObject player;

  private Vector2[] hitLocations;
  private int numHits = 0;

  private byte[][] frames;
  private int frameCounter = 0;
  private bool rewind = false;

  //Notes: needs to get raw data from fluid simulator to rewind
  //velocity doesn't work

	void Start () 
  {
    fluid = GetComponent<FluidSimulator>();
    particlesArea = GetComponent<ParticlesArea>();
    fluidCollider = fluid.GetComponent<Collider>();

    player = GameObject.Find("Player Object");
    targetSpriteRenderer = player.GetComponent<SpriteRenderer>();
    targetSprite = targetSpriteRenderer.sprite;

    frames = new byte[100][];
	}

  public void Play()
  {
    playing = true;

    //transform.position = player.transform.position;

    //targetSpriteRenderer.enabled = false;
    hitLocations = new Vector2[maxHits];
    numHits = 0;
    frameCounter = 0;
    firstFrame = true;
    player.GetComponent<SpriteRenderer>().enabled = false;
  }
	
  void LateUpdate () 
  {
    int xLocation;
    int yLocation;
    Vector3 offset = Vector3.zero;
    Ray ray = new Ray();
    ray.direction = Vector3.forward;
    int hits = 0;

    //particlesArea.AddParticles(new Vector3(0.5f, 0.5f, 0f), 10, 100);
    //ray.origin = transform.parent.position;
    //if (fluidCollider.Raycast(ray, out rayHit, 1000))
    //{
    //  fluid.AddVelocity(rayHit.textureCoord, Vector3.left, 2);
    //  Debug.Log("Adding velocity");
    //}

    if (Input.GetKeyDown("i"))
    {
      player.GetComponent<SpriteRenderer>().enabled = false;
      Play();
    }
    if (playing)
    {
      //frames[frameCounter] = targetSprite.texture.GetRawTextureData();
      frameCounter++;

      Vector3 point = player.GetComponent<BoxCollider2D>().bounds.center;
      if (frameCounter < addParticleFrames)particlesArea.AddParticles(new Vector3(0.5f, 0.5f, 0), startRadius, numParticles);
      ray.origin =  point;
      if (fluidCollider.Raycast(ray, out rayHit, 1000))
      {
        fluid.AddVelocity(rayHit.textureCoord, Vector3.up * velocity, startRadius);
        Debug.Log("Adding velocity");
      }
      Debug.Log(GetComponent<Renderer>().material.mainTexture);

      /*
      for (int locationIndex = 0; locationIndex < randomLocations && numHits < maxHits; locationIndex++)
      {
        xLocation  = Mathf.FloorToInt(Random.Range(0f, 1f) * 512);
        yLocation  = Mathf.FloorToInt(Random.Range(0f, 1f) * 512);

        if (targetSprite.texture.GetPixel(xLocation, yLocation).a != 0f)
        {
          hits++;
          //Debug.Log("Found Black pixel " + xLocation + ", " + yLocation);
          offset.x = (xLocation / 512f) * targetSprite.bounds.size.x;
          offset.y = (yLocation / 512f) * targetSprite.bounds.size.y;
          ray.origin = transform.position + startOffset + targetSprite.bounds.min + offset;
          //Debug.Log("Offset was " + offset);
          //Debug.Log("Ray cast from " + ray);
          if (fluidCollider.Raycast(ray, out rayHit, 1000))
          {
            //Debug.Log("Hit fluid area: " + rayHit.textureCoord);
            hitLocations[numHits] = rayHit.textureCoord;
            numHits++;
          }
        }
      }

      for (int hitIndex = 0; hitIndex < numHits; hitIndex++)
      {
        particlesArea.AddParticles(hitLocations[hitIndex], startRadius, numParticles);
      }
      fluid.AddVelocity(hitLocations[0], Vector3.right, velocity);
      */

      //Debug.Log("Hits = " + hits);
      //if (frameCounter > 90)
      //{
      //  rewind = true;
      //  playing = false;
      //}
      firstFrame = false;
    }
    else if (rewind)
    {
      Debug.Log("rewinding");
      targetSpriteRenderer.enabled = true;
      targetSprite.texture.LoadRawTextureData(frames[frameCounter]);
      targetSprite.texture.Apply();
      frameCounter--;
      if (frameCounter == 0)
      {
        rewind = false;
      }
    }
  }
}
