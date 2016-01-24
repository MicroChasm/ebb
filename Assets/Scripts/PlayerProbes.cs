using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerProbes : MonoBehaviour 
{
  public float slopeTolerance = 0.1f;
  public float groundNearTolerance = 2.0f;
  public float wallGrabNearTolerance = 0.1f;

  public Probe groundLeft;
  public Probe groundRight;

  public Probe groundFar;

  public Probe findGround;

  public Probe forwardLow;
  public Probe forwardHigh;

  public Probe backwardHigh;

  public Probe headLeft;
  public Probe headRight;

  public float probeDirection = -1f;

  private Dictionary<string, Probe> probeMap;

	void Start () 
  {
    Probe[] probes = GetComponents<Probe>();
    probeMap = new Dictionary<string, Probe>();

    foreach (Probe probe in probes)
    {
      probeMap.Add(probe.probeName, probe);
    }

    groundLeft = probeMap["GroundLeft"];
    groundRight = probeMap["GroundRight"];

    forwardLow = probeMap["ForwardLow"];
    forwardHigh = probeMap["ForwardHigh"];

    headLeft = probeMap["HeadLeft"];
    headRight = probeMap["HeadRight"];

    backwardHigh = probeMap["BackwardHigh"];

    groundFar = probeMap["GroundFar"];

    findGround = probeMap["FindGround"];
	}
	
	void Update () 
  {
	
	}

  public bool CheckGrounded()
  {
    return groundLeft.Hit() || groundRight.Hit();
  }

  public bool CheckWall()
  {
    return CheckWallAhead() || backwardHigh.Hit();
  }

  public bool CheckWallAhead()
  {
    return forwardHigh.Hit() || forwardLow.Hit();
  }

  public bool CheckWallBehind()
  {
    return backwardHigh.Hit();
  }

  public bool CheckWallGrab()
  {
    bool result = false;
    if (forwardHigh.Hit())
    {
      result = Vector2.Distance(forwardHigh.hitInfo.point, forwardHigh.startPoint + (Vector2)transform.parent.position) < wallGrabNearTolerance;
    }
    return result;
  }

  public float WallDirectionRelative()
  {
    float direction = 0.0f;

    if (backwardHigh.Hit())
    {
      direction = -1f;
    }
    else if (forwardHigh.Hit())
    {
      direction = 1f;
    }

    return direction;
  }

  public float WallDirection()
  {
    float relativeDirection = WallDirectionRelative();
    return Mathf.Sign(transform.parent.localScale.x) * relativeDirection;
  }

  public Vector2 WallPoint()
  {
    Vector2 wallPoint = Vector2.zero;
    if (forwardHigh.Hit())
    {
      if (forwardLow.Hit())
      {
        if (forwardHigh.Distance() > forwardLow.Distance())
        {
          wallPoint = forwardLow.hitInfo.point;
        }
        else
        {
          wallPoint = forwardHigh.hitInfo.point;
        }
      }
      else
      {
        wallPoint = forwardHigh.hitInfo.point;
      }
    }
    else if (forwardLow.Hit())
    {
      wallPoint = forwardLow.hitInfo.point;
    }
    return wallPoint;
  }

  public bool CheckGroundFar()
  {
    return groundFar.Hit();
  }

  public bool CheckGroundNear()
  {
    bool groundNear = false;
    float groundDist = 100000000000;
    if (CheckGrounded())
    {
      groundDist = Vector2.Distance(GroundPoint(), (Vector2)transform.parent.position + groundLeft.startPoint);
      if (groundDist < groundNearTolerance)
      {
        groundNear = true;
      }
    }
    //Debug.Log("Ground near = " + groundNear + ", dist = " + groundDist);
    return groundNear;
  }

  public bool CheckHeadHit()
  {
    return headLeft.Hit() || headRight.Hit();
  }

  public bool CheckHitAbove()
  {
    return headLeft.Hit() || headRight.Hit();
  }

  public Vector2 HeadPoint()
  {
    Vector2 result = Vector2.zero;

    if (headLeft.Hit())
    {
      result = headLeft.hitInfo.point;
    }
    else if (headRight.Hit())
    {
      result = headRight.hitInfo.point;
    }

    return result;
  }

  public Vector2 GroundPoint()
  {
    Vector2 groundPoint = Vector2.zero;
    if (CheckGrounded())
    {
      if (groundRight.Hit())
      {
        if (groundLeft.Hit())
        {
          groundPoint = (groundRight.GetHitInfo().point + groundLeft.GetHitInfo().point) / 2;
        }
        else
        {
          groundPoint = groundRight.GetHitInfo().point;
        }
      }
      else
      {
          groundPoint = groundLeft.GetHitInfo().point;
      }
    }
    return groundPoint;
  }

  public bool CheckSlope()
  {
    bool sloped = false;
    if (CheckGrounded())
    {
      if (Mathf.Abs(groundRight.GetHitInfo().distance - groundLeft.GetHitInfo().distance) > slopeTolerance)
      {
        sloped = true;
      }
    }
    return sloped;
  }

  void OnDrawGizmosSelected()
  {
    Vector2 groundStartLeft;
    Vector2 groundStartRight;
    Vector2 forwardHighStart;

    if (groundLeft != null && groundRight != null && forwardHigh != null)
    {
      groundStartLeft  = groundLeft.startPoint  + (Vector2)transform.parent.position;
      groundStartRight = groundRight.startPoint + (Vector2)transform.parent.position;
      forwardHighStart = forwardHigh.startPoint + (Vector2)transform.parent.position;

      Gizmos.color = Color.white;

      Gizmos.DrawLine(groundStartLeft,  groundStartLeft  + (Vector2.down * groundNearTolerance));
      Gizmos.DrawLine(groundStartRight, groundStartRight + (Vector2.down * groundNearTolerance));
      Gizmos.DrawLine(forwardHighStart, forwardHighStart + (Vector2.right * probeDirection * wallGrabNearTolerance));
    }
  }

  public Vector2 FindGroundPostMovement()
  {
    findGround.Trigger();
    return findGround.hitInfo.point;
  }

  public bool CheckCliff()
  {
    return groundLeft.Hit() ^ groundRight.Hit();
  }

  public void SetDirection(float direction)
  {
    probeDirection = direction;

    foreach (Probe probe in probeMap.Values)
    {
      probe.SetProbeDirection(probeDirection);
    }
  }
}

