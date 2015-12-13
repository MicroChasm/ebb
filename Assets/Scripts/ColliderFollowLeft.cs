using UnityEngine;
using System.Collections;

public class ColliderFollowLeft : MonoBehaviour
{


    public playerController player;
    public float startPositionx = 0;
    public float startPositiony = 0;


    // Update is called once per frame
    void LateUpdate()
    {
        startPositionx = player.transform.position.x;
        startPositiony = player.transform.position.y;

        transform.position = new Vector3((startPositionx - .75f), startPositiony, 0);

    }
    
}
