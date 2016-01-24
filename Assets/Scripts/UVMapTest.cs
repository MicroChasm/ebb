using UnityEngine;
using System;
using System.Collections;

public class UVMapTest : MonoBehaviour 
{
  Mesh mesh;
  Vector2[] originalUVs;
  Vector2[] uvs;
  RaycastHit raycastHitInfo;
  Camera mainCamera;
  float[] xOffsets;

  public float distScale = 0.1f;
  public float walkScale = 0.1f;
  public bool reset = false;

	void Start () 
  {
    mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    mesh = GetComponent<MeshFilter>().mesh;
    uvs = mesh.uv;
    originalUVs = new Vector2[mesh.uv.Length];
    Array.Copy(mesh.uv, originalUVs, mesh.uv.Length);
    xOffsets = new float[mesh.uv.Length];

    foreach(Vector2 uv in uvs)
    {
      Debug.Log("uv = " + uv);
    }
	
	}
	
	void Update () 
  {
    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

    for (int uvIndex= 0; uvIndex < uvs.Length; uvIndex++)
    {
      xOffsets[uvIndex] = Mathf.Clamp(xOffsets[uvIndex] + (2*UnityEngine.Random.value-1)*walkScale, -1, 1);
      //uvs[uvIndex] = originalUVs[uvIndex] + (raycastHitInfo.textureCoord - originalUVs[uvIndex]).normalized * 0.1f;
      uvs[uvIndex] = originalUVs[uvIndex] + new Vector2(Mathf.Sin(xOffsets[uvIndex])*distScale, 0);
    }

    if (reset)
    {
      reset = false;
      Array.Copy(originalUVs, uvs, uvs.Length);
    }

    mesh.uv = uvs;

    //if (GetComponent<Collider>().Raycast(ray, out raycastHitInfo, Mathf.Infinity))
    //{
    //}

	
	}
}
