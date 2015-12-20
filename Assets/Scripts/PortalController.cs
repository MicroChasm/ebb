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

    private bool activated = false;
    public int endCounter = 0;

    private CameraShake cameraShake; 
    private RuneController runeController;

    void Awake()
    {
        player = GameObject.Find("Player Object");
        cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
    }

    void Start() 
    {
        runeController = GameObject.Find("RuneController").GetComponent<RuneController>();
        /*
        initialConfig.numParticles = 500f;
        initialConfig.life = 0.5f;
        initialConfig.circleRadius = new Vector2(0.3f, 0.3f);
        initialConfig.tendrilRadius = 2;
        initialConfig.numTendrils = 7;
        initialConfig.tendrilVelocity = 5;

        currentConfig = initialConfig;
        */
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector2 tendrilOffset;
        GameObject child;
        int childCount;
        float tendrilAngle = 0.0f;
        Vector2 tendrilVelocity = Vector2.zero;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (activated)
        {
          endCounter++;
          if (endCounter > 275)
          {
              Application.LoadLevel(0);
          }
        }

        if (runeController.CollectedAllRunes() && distanceToPlayer < activateDistance)
        {
            activated = true;
            cameraShake.StartScreenShake(10f, 1.0f, 1.01f);
        }

        if (distanceToPlayer < startupDistance)
        {
            if (runeController.CollectedAllRunes())
            {
                m_particlesArea.AddParticles(circleOffset, circleRadius, numParticles * Time.deltaTime);
                m_particlesArea.AddParticles(circleOffset, circleRadius * 2, (numParticles / 10) * Time.deltaTime);
            }
        }

        if (activated)
        {
            //move player towards center to keep them from getting away.
            player.transform.position = Vector2.Lerp(player.transform.position, transform.position, 10 * Time.deltaTime);

            //add more particles to help engulf the screen.
            numParticles += 10;

            //add particles in first circle
            m_particlesArea.AddParticles(circleOffset, activatedCircleRadius, numParticles * Time.deltaTime);

            //add particles in second (larger) circle
            m_particlesArea.AddParticles(circleOffset, activatedCircleRadius * 2, (numParticles / 2) * Time.deltaTime);

            for (int tendrilIndex = 0; tendrilIndex < tendrils; tendrilIndex++)
            {
                //tendril angle along 2*pi radians .
                tendrilAngle = tendrilIndex * (2 * Mathf.PI / tendrils);

                //offset is given by angle plus small offset from center.
                tendrilOffset = new Vector2(offset.x + 0.1f * Mathf.Cos(tendrilAngle), offset.y + 0.1f * Mathf.Sin(tendrilAngle));

                //velocity is a rotation of an outwards vector by a certain number of degrees.
                m_fluid.AddVelocity(tendrilOffset, velocity * (Quaternion.AngleAxis((360 / tendrils) * tendrilIndex, Vector3.forward) * Vector3.right), radius);
            }
        }

    }
    void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(transform.position, startupDistance);
    }


}

