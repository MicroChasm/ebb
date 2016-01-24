using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour 
{
  //TODO Have a way to save and play animations during gameplay
  //file name, length, frames, player, record, repeat

  public string fileName = "";
  public bool startRecording = false;
  public bool replayRecording = false;
  public float recordingLengthSeconds = 5f;
  public float framesPerSecond = 10f;
  public bool playOnRepeat = false;

	void Start () 
  {
	
	}
	
	void Update () 
  {
	
	}
}
