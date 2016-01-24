using UnityEngine;
using System.Collections;

public class BirdController : MonoBehaviour
{
    public ParticleSystem blood;
    public float hp = 300;
    public float playerDamage = 1000;
    public BirdSpriteAnimator birdAnimator;

    public AudioSource CawAudio;

    float flyingSpeed = 10;
    float attackSpeed = 30;
    float pathDistance = 20;
    Vector2 pathStart;
    float pathDirection = 1;
    GameObject player;
    Rigidbody2D playerBody;
    float birdSightDistance = 25;
    float diveHeight = 10;
    float climbingSpeed = 8;
    float followThroughDistance = 10;
    Vector2 playerFollowThroughPosition;
    float followThroughReachedDistance = 2;
    BirdState birdState = BirdState.BirdFlying;
    public Transform birdSpriteTransform;
    Rigidbody2D body;

    enum BirdState
    {
        BirdFlying,
        BirdClimbing,
        BirdAttacking,
        BirdFollowThrough,
        BirdDeath
    };

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
        pathStart = body.position;
        player = GameObject.Find("Player Object");
        if (player == null)
        {
            Debug.LogError("BirdBehavior could not find player object");
        }
        else
        {
            playerBody = player.GetComponent<Rigidbody2D>();
        }
        if (birdSpriteTransform == null)
        {
            Debug.LogError("Must set up birdSpriteTransform");
        }
        blood.emissionRate = 0;
        birdAnimator = transform.parent.GetComponentInChildren<BirdSpriteAnimator>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector2 movementVector = Vector2.zero;

        switch (birdState)
        {
            case BirdState.BirdFlying:
                if (Vector2.Distance(playerBody.position, body.position) < birdSightDistance)
                {
                    //birdState = BirdState.BirdClimbing;
                    birdState = BirdState.BirdAttacking;
                }
                else
                {
                    //Check if the bird needs to switch directions
                    if (Vector2.Distance(pathStart, body.position) >= pathDistance)
                    {
                        pathDirection *= -1;
                    }
                    movementVector = flyingSpeed * Vector2.right * Time.fixedDeltaTime * pathDirection;
                    body.MovePosition(body.position + movementVector);
                }
                break;

            case BirdState.BirdClimbing:
                //Test if bird is high enough above the player to dive
                if (Mathf.Sign((playerBody.position - body.position).y + diveHeight) == -1)
                {
                    birdState = BirdState.BirdAttacking;
                }
                else
                {
                    //climb up for dive
                    movementVector = Vector2.up;
                    body.MovePosition(Vector2.MoveTowards(body.position, body.position + Vector2.up, climbingSpeed * Time.fixedDeltaTime));
                }
                playerFollowThroughPosition = playerBody.position + followThroughDistance * (playerBody.position - body.position).normalized;
                break;

            case BirdState.BirdAttacking:
                if (Vector2.Distance(playerBody.position, body.position) < followThroughDistance)
                {
                    playerFollowThroughPosition = playerBody.position + followThroughDistance * (playerBody.position - body.position).normalized;
                    birdState = BirdState.BirdFollowThrough;
                }
                else
                {
                    movementVector = (playerBody.position - body.position);
                    body.MovePosition(Vector2.MoveTowards(body.position, playerBody.position, attackSpeed * Time.fixedDeltaTime));
                }
                break;

            case BirdState.BirdFollowThrough:
                if (Vector2.Distance(body.position, playerFollowThroughPosition) < followThroughReachedDistance)
                {
                    birdState = BirdState.BirdFlying;
                }
                else
                {
                    movementVector = playerFollowThroughPosition - body.position;
                    body.MovePosition(Vector2.MoveTowards(body.position, playerFollowThroughPosition, attackSpeed * Time.fixedDeltaTime));
                }
                pathStart = body.position;//reset path in case bird loses sight of player
                break;

            case BirdState.BirdDeath:
                body.isKinematic = false;
                body.gameObject.GetComponent<CircleCollider2D>().isTrigger = false;
                birdAnimator.StopAnimation();
                gameObject.tag = "Player";
                break;

            default:
                Debug.LogError("Bird fell through state system to default case");
                break;
        }
        body.rotation = Mathf.Rad2Deg * Mathf.Atan2(movementVector.y, movementVector.x);
        birdSpriteTransform.position = body.position;
    }

    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            ApplyDamage();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerAttack" && birdState != BirdState.BirdDeath)
        {
            Debug.Log("bird got hit!");
            ApplyDamage();
            CawAudio.PlayOneShot(CawAudio.clip);
        }
    }

    void ApplyDamage()
    {
        hp -= playerDamage;

        StartCoroutine(MoveAway(player.transform.position));

        blood.emissionRate = 500;
        StartCoroutine(StopBlood());
        if (hp < 0)
        {
            birdState = BirdState.BirdDeath;
        }
    }

    IEnumerator StopBlood()
    {
        yield return new WaitForSeconds(1);
        blood.emissionRate = 0;
    }

    IEnumerator RemoveBird()
    {
        yield return new WaitForSeconds(5);
        transform.parent.GetComponentInChildren<SpriteRenderer>().enabled = false;
    }

    IEnumerator MoveAway(Vector2 position)
    {
        Vector2 awayPosition = position + 4*(body.position - position);
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(0.1f);
            body.MovePosition(Vector2.Lerp(body.position, awayPosition, 0.1f));
        }
    }
}
