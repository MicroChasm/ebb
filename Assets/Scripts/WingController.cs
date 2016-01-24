using UnityEngine;
using System.Collections;

public class WingController : MonoBehaviour 
{
  public enum WingState
  {
    WING_STATE_UNWING,
    WING_STATE_UNFURL,
    WING_STATE_FLUTTER,
    WING_STATE_FURL
  };

  public WingState wingState = WingState.WING_STATE_UNWING;

  Animator anim;

	void Start () 
  {
    anim = GetComponent<Animator>();
	}
	
	void Update () 
  {
    AnimatorStateInfo animatorStateInfo = anim.GetCurrentAnimatorStateInfo(0);
    switch (wingState)
    {
      case WingState.WING_STATE_UNWING:
        break;

      case WingState.WING_STATE_UNFURL:
        if (animatorStateInfo.IsName("Unfurl") && animatorStateInfo.normalizedTime >= 1.0f)
        {
          wingState = WingState.WING_STATE_FLUTTER;
          Debug.Log("Unfurled");
        }
        break;

      case WingState.WING_STATE_FLUTTER:
        break;

      case WingState.WING_STATE_FURL:
        if (animatorStateInfo.IsName("Furl") && animatorStateInfo.normalizedTime >= 1.0f)
        {
          wingState = WingState.WING_STATE_UNWING;
          Debug.Log("Furled");
        }
        break;
    }
    anim.SetInteger("WingState", (int)wingState);
	}
  
  public void Gliding(bool gliding)
  {
    if (gliding)
    {
      if (wingState == WingState.WING_STATE_UNWING)
      {
        wingState = WingState.WING_STATE_UNFURL;
      }
    }
    else if (!gliding && wingState != WingState.WING_STATE_UNWING)
    {
      wingState = WingState.WING_STATE_FURL;
    }
  }

  public bool CheckUnfurled()
  {
    return wingState == WingState.WING_STATE_FLUTTER;
  }
}
