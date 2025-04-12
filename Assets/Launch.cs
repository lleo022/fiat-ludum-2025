using UnityEngine;

public class Launch : MonoBehaviour
{
    public Vector2 faceDirection;
    public GameObject GameLogic; //should be set by spawner

    public int launchVelocity = 10;

    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = faceDirection + new Vector2(0, launchVelocity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
