using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WispController : MonoBehaviour {
  public float wispRadius = 50f;
  public float floatRadius = 2;
  public float floatSpeed = 0.5f;
  //private RaycastHit2D[] raycastHits = new RaycastHit2D[1];
  private CircleCollider2D circleCollider;
  private int groundLayerMask = 0;
  private int direction = 1;
  private List<Vector2> controlPoints;
  private bool runningSegment = false;
  private int numControlPoints = 4;
  private float timeSpent = 0;
  private float expectedTime = 0;
  private Vector2 centerPoint = Vector2.zero;

  //wisp effect variables
  public float numParticles = 10;
  private FluidSimulator fluid;
  private ParticlesArea particlesArea;
  private Collider fluidCollider;
  public float intensity = 1f;
  public float pathLength = 0f;
  private float intensityX = 0;
  private float intensityY = 0;
  public float trailVelocty = 1;
  public float trailRadius = 1;

  private Vector3 previousPosition;
  private Vector2 fluidCenter = new Vector2(0.5f, 0.5f);

  private const int PATH_LENGTH_SEGMENTS = 10;

  enum BezierRank
  {
    BEZIER_LINEAR    = 2,
    BEZIER_QUADRATIC = 3,
    BEZIER_CUBIC     = 4
  };

  //Notes: particle life of 0.5 helps pulsing intensity look good and trail evaporating before hitting wall
 
  void Start () 
  {
    circleCollider = GetComponent<CircleCollider2D>();
    groundLayerMask = LayerMask.GetMask("Ground");
    controlPoints = new List<Vector2>();

    fluid = transform.parent.GetComponent<FluidSimulator>();
    particlesArea = transform.parent.GetComponent<ParticlesArea>();
    fluidCollider = transform.parent.GetComponent<Collider>();

    float maxStartDistX = fluidCollider.bounds.extents.x - floatRadius;
    float maxStartDistY = fluidCollider.bounds.extents.y - floatRadius;
    centerPoint = transform.position + new Vector3(Random.Range(-floatRadius, floatRadius),
                                                   Random.Range(-floatRadius, floatRadius),
                                                   0);
    transform.position = centerPoint;

    intensityY = Random.Range(0f, 100f);
    intensityX = 0f;
    
    previousPosition = transform.position;
  }

  void Update ()
  {
    float angle = 0;
    float radial = 0;
    float t = 0;

    timeSpent += Time.deltaTime;
    t = timeSpent / expectedTime;

    if (t >= 1)
    {
      runningSegment = true;
      controlPoints.Clear();
      controlPoints.Add(transform.position);
      for (int controlIndex = 0; controlIndex < numControlPoints-1; controlIndex++)
      {
        radial = Random.Range(0, floatRadius);
        angle = Random.Range(0, 2 * Mathf.PI);
        controlPoints.Add(centerPoint + new Vector2(Mathf.Cos(angle) * radial, Mathf.Sin(angle) * radial));
      }
      
      pathLength = PathLength(controlPoints);

      expectedTime = pathLength / floatSpeed;

      t = 0;
      timeSpent = 0;
    }
    intensityX += Time.deltaTime;
    intensity = Mathf.PerlinNoise(intensityX, intensityY);
    transform.position = Interpolate(t, controlPoints);
    //Debug.Log("wisp position = " + controlPoints[0] + ", " + controlPoints[1]);
  }

  void LateUpdate()
  {
    Vector2 position = transform.position - transform.parent.position;
    position = fluidCenter + (new Vector2(position.x / fluidCollider.bounds.size.x, position.y / fluidCollider.bounds.size.y));

    //Vector3 trail = (previousPosition - transform.position);
    //previousPosition = transform.position;
    
    for (int circleIndex = 0; circleIndex < 5; circleIndex++)
    {
      particlesArea.AddParticles(position, wispRadius / (circleIndex+1),   intensity * numParticles);
    }

    //fluid.AddVelocity(fluidCenter, trail, trailRadius);
  }

  private float PathLength(List<Vector2> points)
  {
    float distance = 0;
    Vector2 prevPoint = points[0];
    Vector2 curPoint = Vector2.zero;
    float tDivide = 1 / PATH_LENGTH_SEGMENTS;
    for (int i = 0; i < 10; i++)
    {
      curPoint = Interpolate(tDivide * i, points);
      distance += Vector2.Distance(prevPoint, curPoint);
      prevPoint = curPoint;
    }

    distance += Vector2.Distance(prevPoint, points[points.Count-1]);

    return distance;
  }

  private Vector2 Interpolate(float t, List<Vector2> points)
  {
    Vector2 result = Vector2.zero;
    Vector2 p0 = Vector2.zero;
    Vector2 p1 = Vector2.zero;
    Vector2 p2 = Vector2.zero;
    Vector2 p3 = Vector2.zero;

    if (points.Count == (int)BezierRank.BEZIER_LINEAR) //Linear
    {
      p0 = points[0];
      p1 = points[1];
      result = (1 - t) * p0 + t * p1;
    }
    else if (points.Count == (int)BezierRank.BEZIER_QUADRATIC) //Quadratic
    {
      p0 = points[0];
      p1 = points[1];
      p2 = points[2];
      result = Mathf.Pow(1-t, 2.0f) * p0 + 2.0f * (1-t) * t * p1 + Mathf.Pow(t, 2.0f) * p2;

    }
    else if (points.Count == (int)BezierRank.BEZIER_CUBIC)//Cubic
    {
      p0 = points[0];
      p1 = points[1];
      p2 = points[2];
      p3 = points[3];
      result = (Mathf.Pow(1-t, 3.0f) * p0)              +
               (3.0f * Mathf.Pow(1-t, 2) * t * p1)      +
               (3.0f * (1-t) * Mathf.Pow(t, 2.0f) * p2) +
               (Mathf.Pow(t, 3) * p3);
    }
    return result;
  }
}
