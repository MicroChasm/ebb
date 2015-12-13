using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour {
    public Rigidbody2D player;
   // public int yPosition;

    // Use this for initialization
    void Start () {
        player = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update () {
        float position = player.position.y;
       // yPosition = position.y;
	}
}
