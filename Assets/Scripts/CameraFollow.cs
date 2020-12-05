using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float timeOffset; //Easement Timining/ Speed of Camera Movement
    [SerializeField] Vector3 offsetPos;

    [SerializeField] Vector3 boundsMin;
    [SerializeField] Vector3 boundsMax;

    // Start is called before the first frame update
    private void LateUpdate() //camera function
    {
        if (player != null)
        {
            Vector3 startPos = transform.position; //where the camera current is, each frame is updated to the newly calculated camera position
            Vector3 targetPos = player.position;    //Where the player is This remains constant until the object being followed changes its position

            targetPos.x += offsetPos.x;
            targetPos.y += offsetPos.y;
            targetPos.z = transform.position.z; //our camera default at -10

            //Set up the boundary where the target goes
            targetPos.x = Mathf.Clamp(targetPos.x, boundsMin.x, boundsMax.x); //doesnt let the camera go beyond or less than the min and max
            targetPos.y = Mathf.Clamp(targetPos.y, boundsMin.y, boundsMax.y);

            float t = 1f - Mathf.Pow(1f - timeOffset, Time.deltaTime * 30); //30fps
            transform.position = Vector3.Lerp(startPos, targetPos, t);

        }
    }
}
