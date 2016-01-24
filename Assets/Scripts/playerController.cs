using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//TODO change attack to use animator information rather then counts to determine when to draw collision boxes
//     boss 1 animator causes it to be unmovable and revert its transform

public class playerController : MonoBehaviour
{
  public enum PlayerState
  {
    IDLE,
    WALKING,
    RUNNING,
    DIALOG,
    //ATTACKING,
    FALLING,
    WALL_JUMP,
    WALL_GRAB,
    JUMPING,
    DOUBLE_JUMP,
    LANDING,
    CROUCHING,
    ROLLING,
    GLIDING
  };

  public enum AttackState
  {
    NO_ATTACK    = 0,
    PUNCH        = 1,
    GROUND_POUND = 3
  };

  public enum Direction
  {
    LEFT,
    RIGHT,
    OPPOSITE
  }

  struct PlayerInput
  {
    public bool  run;
    public bool  moving;
    public bool  left;
    public bool  right;
    public int   movementDirection;
    public bool  jump;
    public bool  interact;
    public bool  up;
    public bool  down;
    public float jumpHeld;
    public bool  holdingJump;
    public bool  wing;
    public bool  punchCombo;
  };

  public PlayerState playerState = PlayerState.FALLING;
  private PlayerState playerStatePrev = PlayerState.FALLING;

  bool dead = false;
  public SpriteRenderer spriteDisplay;

  [Header("Jump Settings")]
  public float jumpBurst = 0.1f;
  public float jumpTrail = 0.1f;
  bool jumping;
  public float jumpLimit = .30f;
  public float maxJumpSpeed = 1;

  [Header("Walk Settings")]
  public float maxSpeedWalk = 1;
  public float walkSpeed = 0.5f;

  [Header("Run Settings")]
  public float maxSpeedRun = 2;
  public float runSpeed = 0.75f;

  [Header("Fall Settings")]
  public float fastFallLimit = -2f;
  public float gravity = 0.1f;
  public float maxSpeedFall = 1f;

  [Header("Roll Settings")]
  public float maxRollSpeed = 1;
  public float rollSpeed = 0.5f;
  public float rollBurst = 0.5f;
  public float maxRollCounter = 10;

  [Header("Wall Grab Settings")]
  public float maxSpeedWallGrab = 0.1f;

  [Header("Wall Jump Settings")]
  public float maxSpeedWallJump = 1f;
  public float wallBurst = 0.1f;
  public float wallKickSpeed = 3f;
  public float wallKickLimit = 0.4f;

  [Header("Wing Settings")]
  public bool hasWingItem = false;
  public float wingCounter = 0f;
  public float maxWingCounter = 0f;
  public float wingHeldCount = 0.1f;
  public bool  wingAvailable = true;
  public float wingVerticalSpeed;
  private WingController wingController;

  //[Header("Movement Limits")]
  //public float maxMovement = 120;
  //public float maxMovementThisFrame = 0;

  [Header("Player Movement")]
  public float horizontalSpeed = 0;
  public float verticalSpeed = 0;

  [Header("Misc")]
  public float damage = 0;

  float minSpeed = 2;
  [HideInInspector]
    public float currentSpeedx;
  [HideInInspector]
  public float currentSpeedy;
  float wallKickCounter = 0;
  [HideInInspector]
  float rollCounter = 0;
  float jumpCounter;
  [HideInInspector]
    public WallCollisionRight wallCollision;
  [HideInInspector]
    public GroundCollision grounded;
  bool facingRight = true;
  [HideInInspector]
    public bool landing = false;
  [HideInInspector]
    public float groundCounter = 0;
  public PortalController Portal;

  public bool fastFall = true;
  bool noJump = false;
  public bool inputEnabled = true;
  float deathCounter = 0;
  public AttackState attackState = AttackState.NO_ATTACK;

  public float attackCounter = 0;

  private bool doubleJumpFlag = false;

  public int attack1Counter = 0;
  [HideInInspector]
    public bool attack1CounterOn = false;
  [HideInInspector]
    public bool holdingAttack = false;
  [HideInInspector]
    public int crouchCounter = 0;
  private bool doubleJumpItem;
  public bool attack3Pound = false;

  public ParticleSystem WalkParticles;
  public ParticleSystem WallParticlesLeft;
  public ParticleSystem WallParticlesRight;
  public ParticleSystem doubleJumpParticles;
  public ParticleSystem BloodParticles;

  private PlayerDeathAnimation playerDeathAnimation;

  private PlayerAudioController playerAudioController;

  // private DeathEffect deathEffect;

  public int coinCount = 0;
  private RuneController runeController;
  private List<string> runeNames;

  public Collider2D attackGroundPoundHitbox;
  public Collider2D attackPunchHitbox;
  public Collider2D attackPunchSndHitbox;

  Animator anim;
  BoxCollider2D boxCollider;
  int groundLayerMask = 0;

  Dialog dialogComponent;

  Rigidbody2D player;

  private PlayerProbes playerProbes;
  private PlayerInput playerInput;
  public Direction playerDirection;

  private float wallKickDirection;
  private CameraShake cameraShake;

  private float lastTimeAttackPressed = 0;

  [Header("God Mode Settings")]
  public bool invincible = false;

  void Awake()
  {
    player = GetComponent<Rigidbody2D>();
    spriteDisplay = gameObject.GetComponent<SpriteRenderer>();
    anim = GetComponent<Animator>();
    boxCollider = GetComponent<BoxCollider2D>();
    playerAudioController = GetComponentInChildren<PlayerAudioController>();
    playerDirection = Direction.RIGHT;
    playerDeathAnimation = GetComponentInChildren<PlayerDeathAnimation>();
    wingController = GetComponentInChildren<WingController>();
    playerInput.holdingJump = false;
  }

  void Start()
  {
    groundLayerMask = LayerMask.GetMask("ground");
    runeController = GameObject.Find("RuneController").GetComponent<RuneController>();
    playerProbes = GetComponentInChildren<PlayerProbes>();
    cameraShake = GetComponentInChildren<CameraShake>();
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    int damageSound;

    if (other.tag == "Hazard")
    {
      if (!invincible) damage += 15;

      damageSound = UnityEngine.Random.RandomRange(0, 3);
      if (damageSound == 0)
      {
        //AttackSource.PlayOneShot(Damage1);
        playerAudioController.PlayAudioClip(AudioClipEnum.AUDIO_CLIP_DAMAGE1);
      }
      else if (damageSound == 1)
      {
        //AttackSource.PlayOneShot(Damage2);
        playerAudioController.PlayAudioClip(AudioClipEnum.AUDIO_CLIP_DAMAGE2);
      }
      else if (damageSound == 2)
      {
        //AttackSource.PlayOneShot(Damage3);
        playerAudioController.PlayAudioClip(AudioClipEnum.AUDIO_CLIP_DAMAGE3);
      }
    }
  }

  void OnTriggerStay2D(Collider2D other)
  {
    AudioSource audioSource;
    SpriteRenderer spriteRenderer;
    bool runeCollectedAlready;

    if (other.tag == "Hazard")
    {
      if (!invincible) damage += 1;
    }

    if (other.tag == "Collectable")
    {
      if (other.gameObject.name == "gem_1")
      {

        audioSource = other.gameObject.GetComponent<AudioSource>();
        spriteRenderer = other.gameObject.GetComponent<SpriteRenderer>();
        GameObject.Find(other.gameObject.name.Replace("Collectable", "")).GetComponent<SpriteRenderer>().enabled = true;
        //audioSource.PlayOneShot(audioSource.clip);
        playerAudioController.PlayAudioClip(AudioClipEnum.AUDIO_CLIP_GEM);
        spriteRenderer.enabled = false;
      }
      else if (other.gameObject.name == "doublejump")
      {
        doubleJumpItem = true;
        Destroy(GameObject.Find("doublejump"));
      }
      else if (other.gameObject.name == "coin")
      {
        coinCount++;
        playerAudioController.PlayAudioClip(AudioClipEnum.AUDIO_CLIP_COIN);
        Destroy(other.gameObject);
      }
      else if (other.gameObject.name.EndsWith("RuneCollectable"))
      {
        runeCollectedAlready = runeController.CollectRune(other.gameObject);
        if (!runeCollectedAlready)
        {
          playerAudioController.PlayRuneAudioClip(other.gameObject.name);
        }
      }
      else if (other.gameObject.name == "WingItem")
      {
        hasWingItem = true;
        Destroy(other.gameObject);
      }
    }
  }

  // Update is called once per frame 
  void Update()
  {
    float rayX;
    float rayY;
    float rayXEnd;
    float rayYEnd;
    AnimatorStateInfo animatorStateInfo = anim.GetCurrentAnimatorStateInfo(0);

    /* Restart with 'r' */
    if (Input.GetKey("r"))
    {
      TransitionState(PlayerState.FALLING);
      Application.LoadLevel(Application.loadedLevel);
    }

    /* Player Input */
    playerInput.run = Input.GetKey(KeyCode.LeftShift);
    playerInput.moving = Input.GetKey("a") || Input.GetKey("d");
    playerInput.right  = Input.GetKey("d");
    playerInput.left   = Input.GetKey("a");
    playerInput.down   = Input.GetKey("s");
    playerInput.jump   = Input.GetKeyDown("space");
    playerInput.up     = Input.GetKeyDown("w");

    if (anim.GetCurrentAnimatorStateInfo(0).IsName("Landing"))
    {
      playerInput.run = false;
      playerInput.moving = false;
      playerInput.right  = false;
      playerInput.left   = false;
      playerInput.down   = false;
      playerInput.jump   = false;
      playerInput.up     = false;
    }

    if ((Input.GetKeyDown("space") || Input.GetKeyDown("w")) && !GroundedState(playerState) && playerState != PlayerState.WALL_GRAB)
    {
      playerInput.jumpHeld = 0f;
      playerInput.holdingJump = true;
    }
    else if (Input.GetKey("space") || Input.GetKey("w"))
    {
      if (playerInput.holdingJump)playerInput.jumpHeld += Time.deltaTime;
      playerInput.wing = hasWingItem && playerInput.holdingJump&& playerInput.jumpHeld > wingHeldCount;
    }
    else if ((Input.GetKeyUp("space") || Input.GetKeyUp("w")))
    {
      //Debug.Log("Wing Set to True");
      playerInput.holdingJump = false;
    }
    else
    {
      playerInput.jumpHeld = -1f;
    }

    if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
    {
      lastTimeAttackPressed = Time.time;
    }

    if ((Time.time - lastTimeAttackPressed) < 0.2 ||
        Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow) ||
        ((animatorStateInfo.IsName("Punch") || animatorStateInfo.IsName("PunchCombo")) && animatorStateInfo.normalizedTime < 1f))
    {
      playerInput.left = false;
      playerInput.right = false;
      playerInput.moving = false;
      playerInput.movementDirection = 0;
      if (playerState == PlayerState.WALKING || playerState == PlayerState.RUNNING)
      {
        TransitionState(PlayerState.IDLE);
        horizontalSpeed = 0;
      }
      else if (playerState == PlayerState.JUMPING)
      {
        TransitionState(PlayerState.FALLING);
      }

      if (Input.GetKey(KeyCode.LeftArrow))
      {
          Flip(Direction.LEFT);
      }
      if (Input.GetKey(KeyCode.RightArrow))
      {
          Flip(Direction.RIGHT);
      }
    }
    else
    {
      if ((Input.GetKey("a")) && !Input.GetKeyDown(KeyCode.RightArrow) && !(playerState == PlayerState.WALL_JUMP || playerState == PlayerState.DIALOG))
      {
        //if (Input.GetKey(KeyCode.LeftArrow) && (playerState == PlayerState.WALKING || playerState == PlayerState.RUNNING))
        //{
        //  FlipSprite(Direction.LEFT);
        //}
        //else
        //{
          playerInput.movementDirection = -1;
          Flip(Direction.LEFT);
        //}
      }
      else if ((Input.GetKey("d")) && !(playerState == PlayerState.WALL_JUMP || playerState == PlayerState.DIALOG))
      {
        //if (Input.GetKey(KeyCode.RightArrow) && (playerState == PlayerState.WALKING || playerState == PlayerState.RUNNING))
        //{
        //  FlipSprite(Direction.RIGHT);
        //}
        //else
        //{
          playerInput.movementDirection = 1;
          Flip(Direction.RIGHT);
        //}
      }
      else 
      {
        playerInput.movementDirection = 0;
      }
      playerInput.interact = Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E);
    }

    /* Attack Input */
    if (attackState == AttackState.PUNCH && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow)))
    {
      playerInput.punchCombo = true;
    }
    else 
    {
      playerInput.punchCombo = false;
    }

    currentSpeedx = player.velocity.x;
    currentSpeedy = player.velocity.y;
    //maxMovementThisFrame = maxMovement;

    landing = false;

    if (damage > 0)
    {
      damage = Mathf.Max(0, damage - 0.5f);
    }

    /* Damage System */
    BloodParticles.emissionRate = (damage * 10);
    if (damage > 250 || Input.GetKeyDown("k"))
    {
      playerDeathAnimation.Play();
      dead = true;
      anim.SetBool("Dead", true);
      GetComponent<SpriteRenderer>().enabled = false;
    }
    //Debug.Log(damage);

    if (Mathf.Abs(horizontalSpeed) < 0.000001) horizontalSpeed = 0;

    switch (attackState)
    {
      case AttackState.GROUND_POUND:
        attackGroundPoundHitbox.enabled = true;
        break;

      case AttackState.PUNCH:
        if (!anim.GetBool("PunchCombo"))
        {
          attackPunchHitbox.enabled = true;
        }
        else
        {
          attackPunchSndHitbox.enabled = true;
        }
        break;

      case AttackState.NO_ATTACK:
        attackPunchSndHitbox.enabled = false;
        attackPunchHitbox.enabled = false;
        attackGroundPoundHitbox.enabled = false;
        attackPunchSndHitbox.enabled = false;
        break;
    }
        
    /* State Actions */
    switch (playerState)
    {
      case PlayerState.DIALOG:

        player.velocity = Vector2.zero;
        anim.SetBool("Dialog", true);
        inputEnabled = false;
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
          dialogComponent.IncrementMessage();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
          dialogComponent.DecrementMessage();
        }
        else if (playerInput.interact || playerInput.jump)
        {
          dialogComponent.SelectDialogOption();
        }
        horizontalSpeed = 0;
        verticalSpeed = 0;
        break;

      case PlayerState.IDLE:
        horizontalSpeed = Mathf.Lerp(horizontalSpeed, 0, 7 * Time.deltaTime);
        verticalSpeed = 0;
        inputEnabled = true;
        if (!playerProbes.CheckGrounded())
        {
          TransitionState(PlayerState.FALLING);
        }
        else if (playerInput.jump || playerInput.up)
        {
          TransitionState(PlayerState.JUMPING);
        }
        else if (playerInput.down && playerInput.moving)
        {
          TransitionState(PlayerState.ROLLING);
        }
        else if (playerInput.moving)
        {
          if (playerInput.run)
          {
            TransitionState(PlayerState.RUNNING);
          }
          else
          {
            TransitionState(PlayerState.WALKING);
          }
        }
        break;

      case PlayerState.JUMPING:
        doubleJumpFlag = true;
        wingAvailable = true;
        jumpCounter += Time.deltaTime;

        if (playerProbes.CheckWall() && (playerInput.up || playerInput.jump))
        {
          TransitionState(PlayerState.WALL_JUMP);
        }
        else if (playerInput.wing)
        {
          TransitionState(PlayerState.GLIDING);
        }
        else if (doubleJumpItem && doubleJumpFlag && playerInput.up)
        {
          TransitionState(PlayerState.DOUBLE_JUMP);
        }
        else if (jumpCounter > jumpLimit || verticalSpeed <= 0)
        {
          TransitionState(PlayerState.FALLING);
        }
        else
        {
          anim.SetBool("Up", true);
          verticalSpeed += jumpTrail * Time.deltaTime;
          verticalSpeed = Mathf.Clamp(verticalSpeed, -maxJumpSpeed, maxJumpSpeed);
        }
        break;

      case PlayerState.FALLING:
        playerProbes.forwardLow.Trigger();
        playerProbes.forwardHigh.Trigger();
        if (playerProbes.CheckWall() && (playerInput.up || playerInput.jump))
        {
          TransitionState(PlayerState.WALL_JUMP);
        }
        else if (playerProbes.CheckWallGrab() && playerInput.moving)
        {
          TransitionState(PlayerState.WALL_GRAB);
        }
        else if (playerProbes.CheckGroundNear() && !(playerProbes.CheckCliff() && playerProbes.CheckWallBehind()))
        {
          TransitionState(PlayerState.IDLE);
        }
        else if (playerInput.wing && wingAvailable)
        {
          TransitionState(PlayerState.GLIDING);
        }
        else if (doubleJumpItem && doubleJumpFlag && playerInput.up)
        {
          TransitionState(PlayerState.DOUBLE_JUMP);
        }
        else
        {
          anim.SetBool("Down", true);
          horizontalSpeed = Mathf.Lerp(horizontalSpeed, 0, Time.deltaTime * 0.5f);
          doubleJumpParticles.emissionRate = 0;
        }

        break;

      case PlayerState.DOUBLE_JUMP:
        doubleJumpFlag = false;
        jumpCounter += Time.deltaTime;

        if (jumpCounter > 0.1f)
        {
          doubleJumpParticles.emissionRate = 0;
        }

        if (playerProbes.CheckWall() && (playerInput.up || playerInput.jump))
        {
          TransitionState(PlayerState.WALL_JUMP);
        }
        else if (playerInput.wing && wingAvailable)
        {
          TransitionState(PlayerState.GLIDING);
        }
        else if ((jumpCounter > jumpLimit) || (!playerInput.jump && !playerInput.up))
        {
          jumpCounter = 0;
          jumping = false;
          TransitionState(PlayerState.FALLING);
        }
        else
        {
          verticalSpeed += jumpTrail * Time.deltaTime;
          verticalSpeed = Mathf.Clamp(verticalSpeed, -maxJumpSpeed, maxJumpSpeed);
        }


        break;

      case PlayerState.WALL_GRAB:
        doubleJumpFlag = true;

        verticalSpeed = (verticalSpeed / 2);

        if (!playerProbes.CheckWallGrab() || !playerInput.moving || PlayerDirection() != playerProbes.WallDirection())
        {
          //TODO this doesn't work now!
          //WallParticlesLeft.emissionRate = 250;

          // else if (wallCollision.wallRight)
          //{
          //   WallParticlesRight.emissionRate = 250;
          //Debug.Log("wall particles should be playing");
          //}
          TransitionState(PlayerState.FALLING);
          WallParticlesRight.emissionRate = 0;
          WallParticlesLeft.emissionRate = 0;
        }
        else
        {
          wallKickDirection = playerProbes.CheckWall() ? -1 : 1;
          if (playerProbes.CheckWall() && (!playerProbes.CheckGrounded()) && (playerInput.up || playerInput.jump))
          {
            TransitionState(PlayerState.WALL_JUMP);
            inputEnabled = false;
            horizontalSpeed = wallKickDirection * wallBurst * Time.deltaTime;
            horizontalSpeed = Mathf.Clamp(horizontalSpeed, -maxSpeedWallJump, maxSpeedWallJump);
            jumping = true;
            noJump = true;
            jumpCounter = 0;
            verticalSpeed = 0;

            StartCoroutine(wallCollision.Deactivate());
          }
        }

        break;

      case PlayerState.WALL_JUMP:
        doubleJumpFlag = true;
        wingAvailable = true;
        wallKickCounter += Time.deltaTime;
        horizontalSpeed = wallKickDirection * wallKickSpeed * Time.deltaTime;
        horizontalSpeed = Mathf.Clamp(horizontalSpeed, -maxSpeedWallJump, maxSpeedWallJump);

        horizontalSpeed += wallKickDirection * wallKickSpeed * Time.deltaTime;
        verticalSpeed += wallKickSpeed * Time.deltaTime;

        if (wallKickCounter < wallKickLimit/2)
        {
          playerInput.moving = false;
          playerInput.left   = false;
          playerInput.right  = false;
        }
        else if (playerInput.wing && wingAvailable)
        {
          TransitionState(PlayerState.GLIDING);
        }

        if (wallKickCounter >= wallKickLimit)
        {
          inputEnabled = true;
          wallKickCounter = 0;

          TransitionState(PlayerState.FALLING);
        }

        break;

      case PlayerState.RUNNING:
        if (!playerProbes.CheckGrounded())
        {
          TransitionState(PlayerState.FALLING);
          doubleJumpFlag = true;
        }
        else if (playerInput.jump || playerInput.up)
        {
          TransitionState(PlayerState.JUMPING);
        }
        else if (!playerInput.moving)
        {
          TransitionState(PlayerState.IDLE);
        }
        else if (!playerInput.run)
        {
          TransitionState(PlayerState.WALKING);
        }
        else if (playerInput.down)
        {
          TransitionState(PlayerState.ROLLING);
        }
        else
        {
          horizontalSpeed += playerInput.movementDirection * runSpeed * Time.deltaTime;
          horizontalSpeed = Mathf.Clamp(horizontalSpeed, -maxSpeedRun, maxSpeedRun);
        }
        break;

      case PlayerState.WALKING:
        if (!playerProbes.CheckGrounded())
        {
          TransitionState(PlayerState.FALLING);
          doubleJumpFlag = true;
        }
        else if (playerInput.jump || playerInput.up)
        {
          TransitionState(PlayerState.JUMPING);
        }
        else if (!playerInput.moving)
        {
          TransitionState(PlayerState.IDLE);
          horizontalSpeed = 0;
        }
        else if (playerInput.run)
        {
          TransitionState(PlayerState.RUNNING);
        }
        else if (playerInput.down)
        {
          TransitionState(PlayerState.ROLLING);
        }
        else
        {
          horizontalSpeed += playerInput.movementDirection * walkSpeed * Time.deltaTime;
          horizontalSpeed = Mathf.Clamp(horizontalSpeed, -maxSpeedWalk, maxSpeedWalk);
        }
        break;

      case PlayerState.ROLLING:
        if (!playerProbes.CheckGrounded())
        {
          TransitionState(PlayerState.FALLING);
          anim.SetBool("Roll", false);
        }
        else if (playerInput.up || playerInput.jump)
        {
          TransitionState(PlayerState.JUMPING);
          anim.SetBool("Roll", false);
        }
        else
        {
          horizontalSpeed += playerInput.movementDirection * rollSpeed * Time.deltaTime;
          horizontalSpeed = Mathf.Clamp(horizontalSpeed, -maxRollSpeed, maxRollSpeed);
        }
        break;

      case PlayerState.GLIDING:
        if (playerProbes.CheckGroundNear())
        {
          TransitionState(PlayerState.IDLE);
        }
        else if (wingCounter >= maxWingCounter)
        {
          TransitionState(PlayerState.FALLING);
        }
        else if (playerProbes.CheckWallGrab() && playerInput.moving)
        {
          TransitionState(PlayerState.WALL_GRAB);
        }
        else if (wingController.CheckUnfurled())
        {
          horizontalSpeed += playerInput.movementDirection * walkSpeed * Time.deltaTime;
          horizontalSpeed = Mathf.Clamp(horizontalSpeed, -maxSpeedWalk, maxSpeedWalk);
          
          verticalSpeed = Mathf.Lerp(verticalSpeed, wingVerticalSpeed, 3f * Time.deltaTime);
        }
        else
        {
          verticalSpeed = Mathf.Lerp(verticalSpeed, wingVerticalSpeed, Time.deltaTime);
        }
        break;

      default:
        break;
    }

    anim.SetBool("Grounded", GroundedState(playerState));

    if (AirState(playerState) && playerState != PlayerState.WALL_JUMP && playerInput.moving)
    {
      if (playerInput.run)
      {
        horizontalSpeed += playerInput.movementDirection * runSpeed * Time.deltaTime;
        horizontalSpeed = Mathf.Clamp(horizontalSpeed, -maxSpeedRun, maxSpeedRun);
      }
      else
      {
        horizontalSpeed += playerInput.movementDirection * walkSpeed * Time.deltaTime;
        horizontalSpeed = Mathf.Clamp(horizontalSpeed, -maxSpeedWalk, maxSpeedWalk);
      }
    }


    //TODO remove wall collision use
    if (grounded.landing && !wallCollision.wallLeft && !wallCollision.wallRight)
    {
      if (true /* !LandingAudioSource.isPlaying */)
      {
        if (fastFall)
        {
          //TODO replace with audio controller call
          // LandingAudioSource.PlayOneShot(fastFallLandingAudioClip);
          playerAudioController.PlayAudioClip(AudioClipEnum.AUDIO_CLIP_FAST_FALL_LAND);
        }
        else
        {
          //TODO replace with audio controller call
          //LandingAudioSource.PlayOneShot(landingAudioClip);
          playerAudioController.PlayAudioClip(AudioClipEnum.AUDIO_CLIP_LAND);
        }
      }
    }

    /* Apply Gravity */
    if (playerState == PlayerState.FALLING || playerState == PlayerState.WALL_GRAB)
    {
      verticalSpeed -= gravity * Time.deltaTime;
    }

    //TODO move to walking state, move rate = 0 to top of function
    if (playerProbes.CheckGrounded())
    {
      if (playerInput.moving)
      {
        WalkParticles.emissionRate = 1000;
        if (playerInput.left)
        {
          WalkParticles.transform.rotation = Quaternion.Euler(300, 0, 0);
        }
        else WalkParticles.transform.rotation = Quaternion.Euler(240, 0, 0);

        if (true /*!walkingAudioSource.isPlaying */)
        {
          //walkingAudioSource.PlayOneShot(walkingAudioClips[Random.Range(0, walkingAudioClips.Length)]);
          playerAudioController.PlayWalkingAudioClip();
        }
      }
      else WalkParticles.emissionRate = 0;
    }
    else WalkParticles.emissionRate = 0;

    //player can't move if in the landing state
    if ((groundCounter > 0) && (groundCounter < 45) && fastFall)
    {
      horizontalSpeed = 0;
      noJump = true;
    }
    else
    {
      noJump = false;
    }
    //Debug.Log("ground counter:" + groundCounter);


    //Check out in front.
    playerProbes.forwardLow.Trigger();
    playerProbes.forwardHigh.Trigger();
    float wallDistance = playerProbes.WallPoint().x - (transform.position.x + boxCollider.bounds.extents.x * PlayerDirection()) + 0.01f * playerInput.movementDirection;
    if (playerState == PlayerState.WALL_GRAB)
    {
      horizontalSpeed = wallDistance;
    }
    else if (AirState(playerState) && playerProbes.CheckWallAhead())//forwardHigh.Hit())
    {
      //Debug.Log("clamping towards wall. wallDistance = " + wallDistance + " horiz speed = " + horizontalSpeed);
      horizontalSpeed = AbsoluteMin(horizontalSpeed, wallDistance);// + 0.01f * playerInput.movementDirection;
      //Debug.Log("clamp result = " + horizontalSpeed);
      //Debug.Break();
    }

    //Check for head hitting ceiling
    if (playerProbes.CheckHeadHit())
    {
      if (AirState(playerState) && verticalSpeed > 0)
      {
        float headDistance = playerProbes.HeadPoint().y - (transform.position.y + boxCollider.bounds.extents.y);
        verticalSpeed = AbsoluteMin(verticalSpeed, headDistance);
        TransitionState(PlayerState.FALLING);
      }
      else if (playerState == PlayerState.WALKING || playerState == PlayerState.RUNNING)
      {
        if (playerProbes.headRight.Hit() && playerInput.moving && (playerProbes.headRight.Distance() / playerProbes.headRight.Length()) < 0.25)
        {
          //Debug.Log("dist/length = " + playerProbes.headRight.Distance() / playerProbes.headRight.Length());
          horizontalSpeed = 0;
        }
      }
    }

    //Check for ground before move
    if (verticalSpeed < 0)
    {
      if (playerProbes.CheckGroundFar())
      {
        float groundFarDistance = playerProbes.groundFar.Distance();
        verticalSpeed = Mathf.Max(verticalSpeed, -1 * groundFarDistance);
      }
    }

    if (attackState == AttackState.GROUND_POUND && playerProbes.CheckGrounded())
    {
      horizontalSpeed = 0;
    }

    /* Apply player movement */
    player.transform.position += new Vector3(horizontalSpeed, verticalSpeed, 0);

    //Adjust in case we moved into or over the ground
    playerProbes.groundLeft.Trigger();
    playerProbes.groundRight.Trigger();
    if (GroundedState(playerState) && playerProbes.CheckGrounded())
    {
      Vector2 newGroundPoint;
      Vector2 newPosition;
      if (!playerProbes.CheckCliff())
      {
        newGroundPoint = playerProbes.FindGroundPostMovement();
        newPosition = new Vector2(newGroundPoint.x, newGroundPoint.y + boxCollider.bounds.extents.y);
        //Debug.Log("no cliff");
      }
      else
      {
        newGroundPoint = playerProbes.GroundPoint();
        newPosition = new Vector2(transform.position.x, newGroundPoint.y + boxCollider.bounds.extents.y);
        //Debug.Log("cliff");
      }
      //Debug.Log("adjusting post movement + " + newPosition);
      player.transform.position = newPosition;
    }

    //TODO this may not be needed
    //if (attackState != AttackState.NO_ATTACK)
    //{
    //  horizontalSpeed = 0;
    //}

    anim.SetBool("Sliding", playerState == PlayerState.IDLE && horizontalSpeed != 0);
  }

  public bool AirState(PlayerState state)
  {
    return (state == PlayerState.JUMPING)     ||
           (state == PlayerState.DOUBLE_JUMP) ||
           (state == PlayerState.WALL_GRAB)   ||
           (state == PlayerState.WALL_JUMP)   ||
           (state == PlayerState.FALLING)     ||
           (state == PlayerState.GLIDING);
  }

  /* animation code-------------------------------------------------------------------------------------------------------------*/

  void FixedUpdate()
  {
    if (dead == true)
    {
      inputEnabled = false;
      deathCounter++;
      //deathEffect.Play();
    }

    if (deathCounter > 70)
    {
      Application.LoadLevel(0);
    }

    if (attackState != AttackState.NO_ATTACK)
    {
      attackCounter++;
      anim.SetFloat("AttackCounter", attackCounter);

      if ((!Input.GetKey(KeyCode.LeftArrow)) && (!Input.GetKey(KeyCode.RightArrow)) && (!Input.GetKey(KeyCode.DownArrow)))
      {
        holdingAttack = false;
      }
    }

    AnimatorStateInfo animatorStateInfo = anim.GetCurrentAnimatorStateInfo(0);
    switch (attackState)
    {
      case AttackState.NO_ATTACK:
        anim.SetBool("PunchCombo", false);
        break;

      case AttackState.PUNCH:
        if (animatorStateInfo.IsName("Punch") && animatorStateInfo.normalizedTime >= 1.0f)
        {
          if (playerInput.punchCombo)
          {
            //TODO add comboAttack
            anim.SetBool("PunchCombo", true);
          }
          else
          {
            attackState = AttackState.NO_ATTACK;
          }
        }
        else if (animatorStateInfo.IsName("PunchCombo") && animatorStateInfo.normalizedTime >= 1.0f)
        {
          attackState = AttackState.NO_ATTACK;
          anim.SetBool("PunchCombo", false);
        }
        break;

      //case (AttackState.PUNCH_SND):
      //  if (attackCounter > 169)
      //  {

      //    if (holdingAttack)
      //    {
      //      attackState = AttackState.PUNCH;
      //      attackCounter = 0;
      //    }
      //    else
      //    {
      //      attackState = AttackState.NO_ATTACK;
      //      inputEnabled = true;
      //      attackCounter = 0;
      //    }
      //  }
      //  break;

      case (AttackState.GROUND_POUND):
        if ((attackCounter > 12) && (anim.GetBool("Grounded") == true) && (anim.GetBool("Attack3 Pound") == false))
        {
          anim.SetBool("Attack3 Pound", true);
          attackCounter = 0;
          //AttackSource.PlayOneShot(Attack3Sound);
          playerAudioController.PlayAudioClip(AudioClipEnum.AUDIO_CLIP_DAMAGE1);
          attack3Pound = true;
          cameraShake.StartScreenShake(1.4f, 2.0f);
        }
        if (attackCounter > 75 && (anim.GetBool("Attack3 Pound")))
        {
          attackState = AttackState.NO_ATTACK;
          attackCounter = 0;
          attack3Pound = false;
          inputEnabled = true;
          anim.SetBool("Attack3 Pound", false);
        }
        break;
    }
    anim.SetInteger("Attack", (int)attackState);


    switch (playerState)
    {
      case PlayerState.DIALOG:
        break;

      case PlayerState.IDLE:
        if (playerProbes.CheckGrounded() && playerInput.down && !playerInput.moving)
        {
          TransitionState(PlayerState.CROUCHING);
        }

        break;

      case PlayerState.CROUCHING:
        horizontalSpeed = 0;

        if (!playerProbes.CheckGrounded())
        {
          TransitionState(PlayerState.FALLING);
        }
        else if (!playerInput.down)
        {
          TransitionState(PlayerState.IDLE);
        }
        else if (playerInput.moving)
        {
          TransitionState(PlayerState.ROLLING);
        }
        else if (playerInput.jump || playerInput.up)
        {
          TransitionState(PlayerState.JUMPING);
        }
        else
        {
          crouchCounter++;

          if (crouchCounter > 17)
          {
            anim.SetBool("Crouch Hold", true);
            anim.SetBool("Crouch", false);
          }

          if (crouchCounter >= 18)
          {
            crouchCounter = 18;
          }
        }
        break;

      case PlayerState.ROLLING:
        rollCounter++;
        if (rollCounter > maxRollCounter)
        {
          anim.SetBool("Roll", false);
          if (playerInput.moving)
          {
            if (playerInput.run)
            {
              TransitionState(PlayerState.RUNNING);
            }
            else
            {
              TransitionState(PlayerState.WALKING);
            }
          }
          else
          {
            TransitionState(PlayerState.IDLE);
          }
        }
        break;

      case PlayerState.GLIDING:
        wingCounter += Time.deltaTime;
        break;

      default:
        break;
    }

    //moving variable for animator, to make sure that the player is trying to move, not just sliding down a slope
    anim.SetBool("Moving", playerInput.moving);

    //VerticalSpeed is calculated using thresholds in the fall animation blend tree.
    anim.SetFloat("VerticalSpeed", 0.5f * (65 / maxSpeedFall) * verticalSpeed);

    if (playerProbes.CheckGrounded())
    {
      anim.SetBool("Landing", false);
    }
    else
    {
      groundCounter = 0;
    }

    //landing state
    if (playerProbes.CheckGrounded() && (groundCounter < 47))
    {
      groundCounter++;
    }

    //falling fast enough for a landing animation
    if (verticalSpeed < fastFallLimit)
    {
      fastFall = true;
    }

    if (groundCounter == 45)
    {
      fastFall = false;
    }

    if ((groundCounter > 0) && (groundCounter < 47))
    {
      landing = true;
    }

    if ((groundCounter == 0) || (groundCounter == 47))
    {
      landing = false;
    }

    anim.SetBool("Landing", landing);


    if ((Mathf.Abs(currentSpeedx) > 0) && (playerProbes.CheckGrounded() == true))
    {
      anim.SetFloat("Speed", Mathf.Abs(currentSpeedx));
    }

    if ((Mathf.Abs(currentSpeedx) <= 0) && (playerProbes.CheckGrounded() == true))
    {
      anim.SetFloat("Speed", Mathf.Abs(currentSpeedx));
    }

    anim.SetBool("FastFall", fastFall);

    if (attackState == AttackState.NO_ATTACK && playerState != PlayerState.DIALOG)
    {
      anim.SetBool("Attack3 Pound", false);
      if (Input.GetKeyDown(KeyCode.DownArrow))
      {
        attackState = AttackState.GROUND_POUND;
        holdingAttack = true;
      }
      else if ((Input.GetKeyDown(KeyCode.LeftArrow)) || (Input.GetKeyDown(KeyCode.RightArrow)))
      {
        attackState = AttackState.PUNCH;
        holdingAttack = true;
        //if (playerProbes.CheckGrounded())
        //{
        //  transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        //}
      }
    }

    //sliding code----------------------------------------------------------------------------------------
    //if ((Mathf.Abs(horizontalSpeed.x) > 0.5) && (moving == false) && (playerProbes.CheckGrounded() == true))
    //{
    // anim.SetBool("Sliding", true);
    //  Debug.Log("Sliding is true");
    // }
    // else anim.SetBool("Sliding", false);

  }

  public void Flip(Direction direction = Direction.OPPOSITE)
  {
    FlipSprite(direction);
    if (direction == Direction.OPPOSITE)
    {
      facingRight = !facingRight;
    }
    else if (direction == Direction.LEFT)
    {
      facingRight = false;
      playerDirection = Direction.LEFT;
    }
    else if (direction == Direction.RIGHT)
    {
      facingRight = true;
    }
    playerDirection = direction;
    playerProbes.SetDirection(PlayerDirection());
  }

  public void FlipSprite(Direction direction = Direction.OPPOSITE)
  {
    if (direction == Direction.OPPOSITE)
    {
      Vector3 theScale = transform.localScale;
      theScale.x *= -1;
      transform.localScale = theScale;
    }
    else if (direction == Direction.LEFT)
    {
      Vector3 theScale = transform.localScale;
      if (Mathf.Sign(theScale.x) != -1)
      {
        theScale.x *= -1;
        transform.localScale = theScale;
      }
    }
    else if (direction == Direction.RIGHT)
    {
      Vector3 theScale = transform.localScale;
      if (Mathf.Sign(theScale.x) != 1)
      {
        theScale.x *= -1;
        transform.localScale = theScale;
      }
    }
  }

  public Direction SignToDirection(float direction)
  {
    Direction result = Direction.OPPOSITE;
    float sign = Mathf.Sign(direction);
    if (direction == 0)
    {
      result = Direction.OPPOSITE;
    }
    else if (sign == -1f)
    {
      result = Direction.LEFT;
    }
    else if (direction == 1f)
    {
      result = Direction.RIGHT;
    }
    return result;
  }

  public float PlayerDirection()
  {
    float result = 1f;
    //return Mathf.Sign(transform.localScale.x);
    if (playerDirection == Direction.LEFT)
    {
      result = -1f;
    }
    return result;
  }

  public bool StartDialog(Dialog dialogComponent)
  {
    bool ready = false;
    if (GroundedState(playerState))
    {
      ready = true;
      TransitionState(PlayerState.DIALOG);
      this.dialogComponent = dialogComponent;
      Flip(SignToDirection(dialogComponent.transform.position.x - transform.position.x));
    }
    return ready;
  }

  public void EndDialog()
  {
    TransitionState(PlayerState.IDLE);
    this.dialogComponent = null;
    anim.SetBool("Dialog", false);
    inputEnabled = true;
  }

  void LateUpdate()
  {
    //clamp velocity
    //player.velocity = Vector2.ClampMagnitude(player.velocity, maxMovementThisFrame);

    if (playerState != PlayerState.WALL_GRAB)
    {
      WallParticlesLeft.emissionRate = 0;
      WallParticlesRight.emissionRate = 0;

    }
  }

  void TransitionState(PlayerState newPlayerState)
  {
    /* Exit current state */
    if (playerState == PlayerState.GLIDING)
    {
      anim.SetBool("Glide", false);
      wingController.Gliding(false);
    }
    else if (playerState == PlayerState.DOUBLE_JUMP)
    {
      doubleJumpParticles.emissionRate = 0;
    }
    else if (playerState == PlayerState.WALL_GRAB)
    {
      anim.SetBool("WallGrab", false);
    }
    else if (playerState == PlayerState.CROUCHING)
    {
      anim.SetBool("Crouch", false);
      anim.SetBool("Crouch Hold", false);
    }
    else if (playerState == PlayerState.FALLING)
    {
      anim.SetBool("Down", false);
    }
    else if (playerState == PlayerState.JUMPING)
    {
      anim.SetBool("Up", false);
    }
    else if (playerState == PlayerState.JUMPING)
    {
      invincible = false;
    }
    else if (playerState == PlayerState.RUNNING)
    {
      anim.SetBool("Run", false);
    }

    /* Enter new state */
    if (newPlayerState == PlayerState.JUMPING)
    {
      playerAudioController.PlayAudioClip(AudioClipEnum.AUDIO_CLIP_JUMP);
      jumping = true;
      jumpCounter = 0;
      anim.SetBool("Up", true);

      if (crouchCounter > 0)
      {
        crouchCounter = 0;
        verticalSpeed += jumpBurst*2;
      }
      else
      {
        verticalSpeed += jumpBurst;
      }
    }
    else if (newPlayerState == PlayerState.DOUBLE_JUMP)
    {
      jumping = true;
      jumpCounter = 0;
      verticalSpeed = jumpBurst*1.2f;
      doubleJumpParticles.emissionRate = 5000;
    }
    else if (newPlayerState == PlayerState.WALL_JUMP)
    {
      wallKickDirection = -1f * playerProbes.WallDirection();
      Flip(SignToDirection(-1 * playerProbes.WallDirection()));
      wallKickCounter = 0;
      verticalSpeed = 0;
    }
    else if (newPlayerState == PlayerState.WALL_GRAB)
    {
      anim.SetBool("WallGrab", true);
    }
    else if (newPlayerState == PlayerState.ROLLING)
    {
      invincible = true;
      rollCounter = 0;
      anim.SetBool("Roll", true);
      horizontalSpeed = rollBurst * PlayerDirection();
    }
    else if (newPlayerState == PlayerState.GLIDING)
    {
      anim.SetBool("Glide", true);
      wingController.Gliding(true);
      wingAvailable = false;
      wingCounter = 0f;
    }
    else if (newPlayerState == PlayerState.CROUCHING)
    {
      crouchCounter = 0;
      anim.SetBool("Crouch", true);
    }
    else if (newPlayerState == PlayerState.FALLING)
    {
      anim.SetBool("Down", true);
    }
    else if (newPlayerState == PlayerState.RUNNING)
    {
      anim.SetBool("Run", true);
    }

    playerStatePrev = playerState;
    playerState = newPlayerState;
  }

  bool GroundedState(PlayerState playerState)
  {
    return playerState == PlayerState.WALKING ||
           playerState == PlayerState.RUNNING ||
           playerState == PlayerState.IDLE    ||
           playerState == PlayerState.LANDING ||
           playerState == PlayerState.DIALOG  ||
           playerState == PlayerState.CROUCHING;
  }
  
  float AbsoluteMin(float first, float second)
  {
    float result = first;
    //if (Mathf.Sign(first) == Mathf.Sign(second) && Mathf.Abs(second) < Mathf.Abs(first))
    if (Mathf.Abs(second) < Mathf.Abs(first))
    {
      result = second;
    }
    return result;
  }
}
