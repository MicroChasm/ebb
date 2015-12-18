using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{

    //public Attack3Collision attack3;


    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform camTransform;

    // How long the object should shake for.
    public float shake = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.8f;
    public float decreaseFactor = 1.0f;
    public float dampen = 1.0f;

    public const float DEFAULT_SHAKE_AMOUNT = 0.8f;
    public const float DEFAULT_SHAKE_DAMPEN = 0.95f;

    Vector3 originalPos;

    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    void FixedUpdate()
    {
        if (shake > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            shakeAmount *= dampen;
            shake -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shake = 0f;
            camTransform.localPosition = originalPos;
        }

        //if (attack3.attack3Running)
        //{
        //    shake += 5;
        //}
        //else
        //{
        //    shake = 0;
        //}
    }

    public void StartScreenShake(float timeSeconds, float newShakeAmount = DEFAULT_SHAKE_AMOUNT, float newDampen = DEFAULT_SHAKE_DAMPEN)
    {
      shake = timeSeconds;
      shakeAmount = newShakeAmount;
      dampen = newDampen;
    }

    public void AddShake(float timeSeconds)
    {
      shake += timeSeconds;
    }
}
