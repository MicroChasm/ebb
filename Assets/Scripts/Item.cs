using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	

	}

	void OnTriggerStay2D (Collider2D player){
		if (player.tag == "Player"){
			gameObject.GetComponent<SpriteRenderer>().enabled = false;
			Debug.Log("item collide");
		}
	}
}
