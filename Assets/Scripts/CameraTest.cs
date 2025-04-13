using UnityEngine;

public class CameraTest : MonoBehaviour
{
    public float FollowSpeed = 2f;
    public Transform target;
    public float yOffset = -1f;
    
    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y + yOffset, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
    }
}
