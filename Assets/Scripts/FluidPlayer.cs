using UnityEngine;
using System.Collections;

public class FluidPlayer : MonoBehaviour 
{

  [Header("Recording and Playback")]
  public bool record = false;
  public bool recording = false;
  public bool playback = false;

  public int numFrames;
  public int framesPerSecond;

  private Gradient colorRamp;
  private Vector2 fluidSize;
  private float[][] particleBuffers;

  private FluidSimulator fluid;
  private ParticlesArea particlesArea;


	void Start () 
  {
    fluid = GetComponent<FluidSimulator>();
    particlesArea = GetComponent<ParticlesArea>();
	}
	
	void Update () 
  {
    if (!record)
    {
      record = false;
      StartCoroutine(Record());
    }
	}

  IEnumerator Record()
  {
    int m_nWidth  = 512;
    int m_nHeight = 512;

    recording = true;

    colorRamp = particlesArea.m_colourGradient;

    for (int frameIndex = 0; frameIndex < numFrames; frameIndex++)
    {
      //ParticlesArea.m_particlesBuffer[0].GetData();
      yield return new WaitForSeconds(1 / framesPerSecond);
    }
    recording = false;
  }
}
