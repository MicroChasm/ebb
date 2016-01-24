using UnityEngine;
using System.Collections;

public class Boss1Controller : MonoBehaviour 
{
  public enum BossState
  {
    BOSS_STATE_IDLE,
    BOSS_STATE_SWIPE,
    BOSS_STATE_SLAM
  }

  public BossState bossState = BossState.BOSS_STATE_IDLE;
  private Animator anim;

	void Start () 
  {
    anim = GetComponent<Animator>();
	}
	
	void Update () 
  {
    switch (bossState)
    {
      case BossState.BOSS_STATE_IDLE:
        break;

      case BossState.BOSS_STATE_SWIPE:
        break;

      case BossState.BOSS_STATE_SLAM:
        break;
    }
    anim.SetInteger("BossState", (int)bossState);
	}
}
