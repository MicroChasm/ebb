using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpiderController : MonoBehaviour 
{
  public enum SpiderState
  {
    SPIDER_STATE_IDLE,
    SPIDER_STATE_WALKING,
    SPIDER_STATE_ATTACK,
    SPIDER_STATE_DEATH,
    SPIDER_STATE_PURSUE,
    SPIDER_STATE_FINDEDGE
  };

  GameObject spiderProbes;
  private Dictionary<string, Probe> probeMap;
  
  private Animator anim;

  public SpiderState spiderState = SpiderState.SPIDER_STATE_IDLE;

  public Probe forwardProbe;
  public Probe downProbe;
  public Probe groundAhead;

  public float probIdleToWalking = 0.5f;
  public float probWalkingToIdle = 0.01f;

  public float moveDirection = -1;
  public float walkSpeed = 1;
  public float pursueSpeed = 1.5f;
  public float closeAttackDistance = 3;
  public float offGroundDistance = 1;
  public float probEdgeToWalk = 0.75f;
  public bool playerInSight = false;
  public float edgeCounter = 0;
  public float maxEdgeCounter = 20;

  private ObjectSensor playerSensor;
  private GameObject player;

	void Start () 
  {
    Probe[] probes = GetComponentsInChildren<Probe>();
    probeMap = new Dictionary<string, Probe>();

    anim = GetComponent<Animator>();

    foreach (Probe probe in probes)
    {
      probeMap.Add(probe.probeName, probe);
    }
    forwardProbe = probeMap["Forward"];
    downProbe = probeMap["Down"];
    groundAhead = probeMap["GroundAhead"];

    playerSensor = GetComponentInChildren<ObjectSensor>();
    player = GameObject.Find("Player Object");
	}
	
	void Update () 
  {
    float distToTarget = 0.0f;
    bool successfulMovement;

    playerInSight = playerSensor.TargetInCollider();

    transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x) * moveDirection, transform.localScale.y, transform.localScale.z);
    switch (spiderState)
    {
      case SpiderState.SPIDER_STATE_IDLE:
        if (playerInSight)
        {
          spiderState = SpiderState.SPIDER_STATE_PURSUE;
        }
        else if (Random.value < probIdleToWalking)
        {
          spiderState = SpiderState.SPIDER_STATE_WALKING;
          if (Random.value < 0.25)Flip();
        }
        break;

      case SpiderState.SPIDER_STATE_WALKING:
        successfulMovement = MoveSpider(walkSpeed);

        if (!successfulMovement)
        {
          spiderState = SpiderState.SPIDER_STATE_FINDEDGE;
          edgeCounter = 0;
        }
        else
        {
          if (Random.value < probWalkingToIdle)
          {
            spiderState = SpiderState.SPIDER_STATE_IDLE;
          }
        }
        break;

      case SpiderState.SPIDER_STATE_ATTACK:
        distToTarget = Vector3.Distance(transform.position, player.transform.position);
        if (distToTarget > closeAttackDistance)
        {
          spiderState = SpiderState.SPIDER_STATE_PURSUE;
        }
        break;

      case SpiderState.SPIDER_STATE_DEATH:
        break;

      case SpiderState.SPIDER_STATE_PURSUE:
        moveDirection = Mathf.Sign(player.transform.position.x - transform.position.x);
        distToTarget = Vector3.Distance(transform.position, player.transform.position);
        if (distToTarget < closeAttackDistance)
        {
          spiderState = SpiderState.SPIDER_STATE_ATTACK;
        }
        else if (!playerInSight)
        {
          spiderState = SpiderState.SPIDER_STATE_WALKING;
        }
        else 
        {
          successfulMovement = MoveSpider(pursueSpeed);
          if (!successfulMovement)
          {
            edgeCounter = 0;
            spiderState = SpiderState.SPIDER_STATE_FINDEDGE;
          }
        }
        break;

      case SpiderState.SPIDER_STATE_FINDEDGE:
        edgeCounter++;
        if (edgeCounter > maxEdgeCounter)
        {
          if (Random.value < probEdgeToWalk)
          {
            spiderState = SpiderState.SPIDER_STATE_WALKING;
            Flip();
          }
          else
          {
            spiderState = SpiderState.SPIDER_STATE_IDLE;
          }
        }
        break;
    }
    anim.SetInteger("SpiderAnimationState", (int)spiderState);
	}

  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.name == "Player Object")
    {
      //Debug.Log("Saw player");
    }
  }

  //returns if moved successfully.
  bool MoveSpider(float speed)
  {
    bool result = true;
    Vector3 prevPosition = transform.position;

    prevPosition = transform.position;
    transform.position = new Vector3(transform.position.x + moveDirection * speed, transform.position.y, 0);

    groundAhead.Trigger();
    if (groundAhead.Hit())
    {
      downProbe.Trigger();
      transform.position = (Vector2)(offGroundDistance * (downProbe.startPoint - downProbe.endPoint).normalized) + downProbe.hitInfo.point;
    }
    else
    {
      transform.position = prevPosition;
      result = false;
    }

    return result;
  }

  void Flip()
  {
    moveDirection *= -1f;
    groundAhead.SetProbeDirection(-1f * moveDirection);
  }
}
