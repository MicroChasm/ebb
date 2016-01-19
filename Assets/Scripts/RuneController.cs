using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RuneController : MonoBehaviour {
  private HashSet<string> runesCollected;
  private HashSet<string> runes;
  public bool allRunesCollected = false;

	void Awake () 
  {
	  runes = new HashSet<string>();
	  runesCollected = new HashSet<string>();

    //Find All Runes in the game
    
    GameObject[] collectables = GameObject.FindGameObjectsWithTag("Collectable");
    foreach (GameObject collectable in collectables)
    {
      if (collectable.name.EndsWith("RuneCollectable"))
      {
        runes.Add(collectable.name);
      }
    }
	}

  public bool CollectRune(GameObject gameObject)
  {
    bool alreadyCollected = false;
    if (runes.Contains(gameObject.name))
    {
      alreadyCollected = runesCollected.Contains(gameObject.name);
      if (!alreadyCollected)
      {
        runesCollected.Add(gameObject.name);
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        allRunesCollected  = CollectedAllRunes();
      }
    }
    return alreadyCollected;
  }

  public bool CollectedAllRunes()
  {
    return runesCollected.Count == runes.Count;
  }

  public HashSet<string> CollectedRunes()
  {
    return runesCollected;
  }
}
