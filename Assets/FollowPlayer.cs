using System;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public Vector2 offset;
    public float zoom = 3.28f;
    public Vector3 central_point = new Vector3(0,0,-1000);

    private Camera cam;
    
    private void Start()
    {
        cam = GetComponent<Camera>();
    }
    void Update()
    {
        if (player != null && transform != null)
        {
            float timing = .05f;
            //transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, -10); // Camera follows
            Vector3 initial_velocity = Vector3.zero;
            Vector3 targetCamPosition = new Vector3(player.position.x + offset.x, player.position.y + offset.y, -10);
            if (central_point != new Vector3(0,0,-1000))
            {
                targetCamPosition = new Vector3(central_point.x + offset.x, central_point.y + offset.y, -10);
            }
            transform.position = Vector3.SmoothDamp(transform.position, targetCamPosition, ref initial_velocity, timing);

            //Debug.Log("Changing zoom: " + cam.orthographicSize + " | " + zoom);
            float initial_velocity2 = 0f;
            float newSize = Mathf.SmoothDamp(cam.orthographicSize, zoom, ref initial_velocity2, timing);
            cam.orthographicSize = newSize;
        }
        
    }
}
