﻿using System;
using UnityEngine;
using System.Collections;

public enum AudioClipEnum
{
  AUDIO_CLIP_UNDEFINED,
  AUDIO_CLIP_COIN,
  AUDIO_CLIP_CROSSRUNE,
  AUDIO_CLIP_FISHRUNE,
  AUDIO_CLIP_PRUNE,
  AUDIO_CLIP_TREERUNE,
  AUDIO_CLIP_HOUSERUNE,
  AUDIO_CLIP_DAMAGE1,
  AUDIO_CLIP_DAMAGE2,
  AUDIO_CLIP_DAMAGE3,
  AUDIO_CLIP_GEM
}

public class PlayerAudioController : MonoBehaviour 
{
  AudioSource audioSource;

  public AudioClip landingAudioClip;
  public AudioClip coinAudioClip;
  public AudioClip attack1AudioClip1;
  public AudioClip attack1AudioClip2;
  public AudioClip treeRuneTone;
  public AudioClip fishRuneTone;
  public AudioClip pruneTone;
  public AudioClip houseRuneTone;
  public AudioClip crossRuneTone;
  public AudioClip damage1;
  public AudioClip damage2;
  public AudioClip damage3;
  public AudioClip gemAudioClip1;
  public AudioClip[] walkingAudioClips = new AudioClip[7];

  void Start () 
  {
    audioSource = GetComponent<AudioSource>();
  }

  void Update () 
  {

  }

  public void PlayAudioClip(AudioClipEnum audioClipEnum)
  {
    AudioClip audioClip = null;

    switch (audioClipEnum)
    {
      case AudioClipEnum.AUDIO_CLIP_COIN:
        audioClip = coinAudioClip;
        break;

      case AudioClipEnum.AUDIO_CLIP_CROSSRUNE:
        audioClip = crossRuneTone;
        break;

      case AudioClipEnum.AUDIO_CLIP_FISHRUNE:
        audioClip = fishRuneTone;
        break;

      case AudioClipEnum.AUDIO_CLIP_PRUNE:
        audioClip = pruneTone;
        break;

      case AudioClipEnum.AUDIO_CLIP_TREERUNE:
        audioClip = treeRuneTone;
        break;

      case AudioClipEnum.AUDIO_CLIP_HOUSERUNE:
        audioClip = houseRuneTone;
        break;

      case AudioClipEnum.AUDIO_CLIP_DAMAGE1:
        audioClip = damage1;
        break;

      case AudioClipEnum.AUDIO_CLIP_DAMAGE2:
        audioClip = damage2;
        break;

      case AudioClipEnum.AUDIO_CLIP_DAMAGE3:
        audioClip = damage3;
        break;

      case AudioClipEnum.AUDIO_CLIP_GEM:
        audioClip = gemAudioClip1;
        break;
    }

    if ((audioClipEnum == AudioClipEnum.AUDIO_CLIP_UNDEFINED) || (audioClip != null))
    {
      audioSource.PlayOneShot(audioClip);
    }
    else
    {
      Debug.LogError("Could not player audioClip " + audioClipEnum); 
    }
  }

  public void PlayRuneAudioClip(string runeName)
  {
    AudioClipEnum clipEnum = AudioClipEnum.AUDIO_CLIP_UNDEFINED;
    if (runeName.IndexOf("tree", StringComparison.OrdinalIgnoreCase) >= 0)
    {
      clipEnum = AudioClipEnum.AUDIO_CLIP_TREERUNE;
    }
    else if (runeName.IndexOf("cross", StringComparison.OrdinalIgnoreCase) >= 0)
    {
      clipEnum = AudioClipEnum.AUDIO_CLIP_CROSSRUNE;
    }
    else if (runeName.IndexOf("fish", StringComparison.OrdinalIgnoreCase) >= 0)
    {
      clipEnum = AudioClipEnum.AUDIO_CLIP_FISHRUNE;
    }
    else if (runeName.IndexOf("house", StringComparison.OrdinalIgnoreCase) >= 0)
    {
      clipEnum = AudioClipEnum.AUDIO_CLIP_HOUSERUNE;
    }
    else
    {
      clipEnum = AudioClipEnum.AUDIO_CLIP_PRUNE;
    }
  }
  
  public void PlayWalkingAudioClip(int walkingClipIndex)
  {
    if (walkingClipIndex < walkingAudioClips.Length)
    {
      if (!audioSource.isPlaying)
      {
        audioSource.PlayOneShot(walkingAudioClips[walkingClipIndex]);
      }
    }
    else
    {
      Debug.LogError("Could not play walking clip " + walkingClipIndex);
    }
  }
}