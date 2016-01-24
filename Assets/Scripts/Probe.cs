using UnityEngine;
using System.Collections;

public class Probe : MonoBehaviour 
{
  public string probeName = "";

  public bool hit = false;
  public RaycastHit2D hitInfo;
  public RaycastHit2D[] raycastHits = new RaycastHit2D[1];
  public Ray2D ray;
  public Vector2 startPoint = Vector2.zero;
  public Vector2 endPoint = Vector2.zero;
  public Color gizmoColor = Color.green;
  public string collisionLayer = "";
  public int mask = -1;
  public float waitTime = 0.25f;
  public bool fixedUpdate = false;
  public ProbeType probeType = ProbeType.PROBE_TYPE_WAIT_SECONDS;

  public float hitDistance = -1;
  public float probeDirection = 1;

  public enum ProbeType
  {
    PROBE_TYPE_WAIT_SECONDS,
    PROBE_TYPE_FIXED_UPDATE,
    PROBE_TYPE_TRIGGER
  };

	void Start () 
  {
      hit = false;
      hitInfo = new RaycastHit2D();
      ray = new Ray2D(startPoint, (endPoint - startPoint).normalized);

      if (collisionLayer != "")
      {
        mask = LayerMask.GetMask(collisionLayer);
      }
      if (probeType != ProbeType.PROBE_TYPE_TRIGGER)
      {
        StartCoroutine(PeriodicProbe());
      }
	}
	
	void Update () 
  {
	
	}
  
  IEnumerator PeriodicProbe()
  {
    while (true)
    {
      Trigger();

      switch (probeType)
      {
        case ProbeType.PROBE_TYPE_WAIT_SECONDS:
          yield return new WaitForSeconds(waitTime);
          break;
        case ProbeType.PROBE_TYPE_FIXED_UPDATE:
          yield return new WaitForFixedUpdate();
          break;
        case ProbeType.PROBE_TYPE_TRIGGER:
          Debug.LogError("Trigger probe " + probeName + " should not have a corountine running");
          break;
      }
    }
  }

  void OnDrawGizmosSelected()
  {
    Gizmos.color = gizmoColor;
    Gizmos.DrawLine(startPoint + (Vector2)transform.parent.position, endPoint + (Vector2)transform.parent.position);
  }

  public RaycastHit2D GetHitInfo()
  {
    return hitInfo;
  }

  public bool Hit()
  {
    return hit;
  }

  public float Distance()
  {
    float dist = 0;
    if (hit)
    {
      dist = Vector2.Distance(startPoint + (Vector2)transform.parent.position, hitInfo.point);
    }
    return dist;
  }

  public void Trigger()
  {
    int numRaycastHits = 0;

    //numRaycastHits = Physics2D.LinecastNonAlloc(CorrectDirection(startPoint) + (Vector2)transform.position, CorrectDirection(endPoint) + (Vector2)transform.position, raycastHits, mask);
    numRaycastHits = Physics2D.LinecastNonAlloc(startPoint + (Vector2)transform.parent.position, endPoint + (Vector2)transform.parent.position, raycastHits, mask);

    hitInfo = raycastHits[0];

    hit = numRaycastHits != 0;

    hitDistance = Distance();
  }

  public void SetProbeDirection(float direction)
  {
    if (probeDirection != direction)
    {
      probeDirection = direction;
      startPoint.Set(startPoint.x * -1f, startPoint.y);
      endPoint.Set(endPoint.x * -1f, endPoint.y);
    }
  }

  public float Length()
  {
    return Vector2.Distance(startPoint, endPoint);
  }
}
