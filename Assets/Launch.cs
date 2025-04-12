using UnityEngine;

public class Launch : MonoBehaviour
{
    public Vector2 faceDirection = new Vector2(0,0);
    public GameObject GameLogic; //should be set by spawner

    public int launchVelocity = 5;

    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Legal document start");
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(faceDirection.x*launchVelocity, launchVelocity);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Legal document updates");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up);
        if (hit)
        {
            float distance = Mathf.Abs(hit.point.y - transform.position.y);
            if (distance < .5f) //on the ground
            {
                //self-destruct
                Debug.Log("destroying: " + distance);
                Destroy(gameObject); // "gameObject" refers to itself
            }

        }

    }
}
