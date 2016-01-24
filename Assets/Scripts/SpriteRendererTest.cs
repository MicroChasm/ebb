using UnityEngine;
using System.Collections;
using System.IO;

public class SpriteRendererTest : MonoBehaviour 
{
  Texture tex;

	void Start () 
  {
    //Sprite object has reference to texture2d
    Texture2D tex2D;//= GetComponent<SpriteRenderer>().material.mainTexture as Texture2D;
    Color color;
    tex2D = new Texture2D(256, 256);
    for (int i = 0; i < 256; i++)
    {
      for (int j = 0; j < 256; j++)
      {
        //if (Mathf.Sqrt(i*i + j*j) < 100)
          color = Color.black / (Mathf.Sqrt(i*i + j*j) / 256);
        //else
          //color = Color.white;
        tex2D.SetPixel(i, j, color);
      }
    }
    //tex2D.SetPixel(1, 1, Color.black);
    //tex2D.SetPixel(2, 2, Color.white);
    //tex2D.SetPixel(3, 3, Color.black);
    //tex2D.SetPixel(4, 4, Color.white);
    tex2D.Apply();
    byte[] pngBytes = tex2D.EncodeToPNG();
    //File.WriteAllBytes(Application.dataPath + "/../test.png", pngBytes);
	}
	
	void Update () 
  {
	
	}
}
