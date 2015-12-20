using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PortalPedistalController : MonoBehaviour {
  private RuneController runeController;
  private Dictionary<string, SpriteRenderer> runeSpriteMap;
  private int prevNumCollectedRunes = 0;

  void Start () 
  {
    runeSpriteMap = new Dictionary<string, SpriteRenderer>();
    foreach (SpriteRenderer runeSprite in GetComponentsInChildren<SpriteRenderer>())
    {
      runeSpriteMap.Add(runeSprite.gameObject.name, runeSprite);
    }

    runeController = GameObject.Find("RuneController").GetComponent<RuneController>();
    StartCoroutine(UpdateRuneState());
  }

  void Update () 
  {

  }

  IEnumerator UpdateRuneState()
  {
    HashSet<string> runes;

    yield return new WaitForSeconds(1.0f);

    while (!runeController.CollectedAllRunes())
    {
      yield return new WaitForSeconds(1.0f);
      runes = runeController.CollectedRunes();
      if (runes.Count != prevNumCollectedRunes)
      {
        prevNumCollectedRunes = runes.Count;
        foreach (string runeName in runes)
        {
          runeSpriteMap[runeName.Replace("Collectable", "")].enabled = true;
          Debug.Log("enabled sprite rendered");
        }
      }
    }
  }
}
