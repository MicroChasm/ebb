using UnityEngine;
using System.Collections;

[AddComponentMenu("Cocuy/Mouse Manipulator")]
public class PortalController : MonoBehaviour {

    public struct PortalConfig
    {
        public float numParticles;
        public float life;
        public Vector2 circleRadius;
        public float tendrilRadius;
        public float numTendrils;
        public float tendrilVelocity;
    };

    public PortalConfig initialConfig = new PortalConfig();
    public PortalConfig activatedConfig;
    public PortalConfig endingConfig;

    public PortalConfig currentConfig;

    public bool portalActivated = false;

    public const float INITIAL_PARTICLES = 500;
    public const float FINAL_PARTICLE = 200;
    public const float INITIAL_LIFE = 0.5f;
    public const float MID_LIFE = 0.8f;

    public float circleRadius = 50f;
    public float activatedCircleRadius = 50f;
    public float radius = 2f;
    public float velocity = 5f;
    public float tendrils = 7f;
    public float numParticles = 500f;

    public float startupDistance = 20;
    public float activateDistance = 5;

    public FluidSimulator m_fluid;
    public ParticlesArea m_particlesArea;

    private const float offsetFloat = 0.1f;

    public Vector2 offset = new Vector2(offsetFloat, offsetFloat);
    public Vector2 circleOffset = new Vector2(offsetFloat, offsetFloat);

    public ObjectSensor playerSensor;
    public GameObject player;
    private GameObject portalPedistal;

    private bool runesCollected = false;
    private bool activated = false;
    public int endCounter = 0;

    // Use this for initialization
    void Start() {
        /*
        initialConfig.numParticles = 500f;
        initialConfig.life = 0.5f;
        initialConfig.circleRadius = new Vector2(0.3f, 0.3f);
        initialConfig.tendrilRadius = 2;
        initialConfig.numTendrils = 7;
        initialConfig.tendrilVelocity = 5;

        currentConfig = initialConfig;
        */
        player = GameObject.Find("Player Object");
        portalPedistal = GameObject.Find("portal_pedestal");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector2 tendrilOffset;
        GameObject child;
        int childCount;
  
        bool allRunesFound = true;
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (activated)
        {
            portalActivated = true;
            endCounter++;
        }

        if (endCounter > 275)
        {
            Application.LoadLevel(0);
        }

        if (activated)
        {
            player.transform.position = Vector2.Lerp(player.transform.position, transform.position, 50 * Time.deltaTime);
            m_particlesArea.AddParticles(circleOffset, activatedCircleRadius, numParticles * Time.deltaTime);
            m_particlesArea.AddParticles(circleOffset, activatedCircleRadius * 2, (numParticles / 2) * Time.deltaTime);

            for (int tendrilIndex = 0; tendrilIndex < tendrils; tendrilIndex++)
            {
                tendrilOffset = new Vector2(offset.x + 0.1f * Mathf.Cos(tendrilIndex * (2 * Mathf.PI / tendrils)), offset.y + 0.1f * Mathf.Sin(tendrilIndex * (2 * Mathf.PI / tendrils)));
                m_fluid.AddVelocity(tendrilOffset, velocity * (Quaternion.AngleAxis((360 / tendrils) * tendrilIndex, Vector3.forward) * Vector3.up), radius);
                //Debug.Log("index = " + tendrilIndex + ", offset = " + tendrilOffset);
            }
        }
        else if (runesCollected && distanceToPlayer < activateDistance)
        {
            activated = true;
        }
        else if (distanceToPlayer < startupDistance)
        {
            if (!runesCollected)
            {
                childCount = portalPedistal.transform.childCount;
                for (int childIndex = 0; childIndex < childCount; childIndex++)
                {
                    child = portalPedistal.transform.GetChild(childIndex).gameObject;
                    if (child.name.Contains("Rune"))
                    {
                        allRunesFound = allRunesFound && child.GetComponent<SpriteRenderer>().enabled;
                    }
                    if (!allRunesFound)
                    {
                        break;
                    }
                }
                runesCollected = allRunesFound;
            }
            if (runesCollected)
            {
                m_particlesArea.AddParticles(circleOffset, circleRadius, numParticles * Time.deltaTime);
                m_particlesArea.AddParticles(circleOffset, circleRadius * 2, (numParticles / 10) * Time.deltaTime);
            }
        }
    }
    void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(transform.position, startupDistance);
    }


}

