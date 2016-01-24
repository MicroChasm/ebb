using UnityEngine;
using System.Collections;

public class MeasuringTape : MonoBehaviour 
{
  Vector2 startPoint;
  Vector2 endPoint;

	// Use this for initialization
	void Start () 
  {
	
	}
	
	// Update is called once per frame
	void Update () 
  {
	
	}

  void OnDrawGizmos()
  {
    Gizmos.color = Color.white;
    Gizmos.DrawLine(startPoint + (Vector2)transform.position, endPoint + (Vector2)transform.position);
  }
}
