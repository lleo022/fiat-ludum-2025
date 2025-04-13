using UnityEngine;

public class teleportFromButton : MonoBehaviour
{
    [SerializeField] Transform targetNewLocation;
    [SerializeField] GameObject popUp;
    bool playerInRange = false;
    GameObject target;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = true;
            popUp.SetActive(true);
            target = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            popUp.SetActive(false);
            target = null; 
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        popUp.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                target.transform.position = targetNewLocation.position;
                popUp.SetActive(false);
                playerInRange = false;
            }
        }
    }
}
