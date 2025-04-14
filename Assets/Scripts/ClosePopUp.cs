using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ClosePopUp : MonoBehaviour
{
    [SerializeField] GameObject popUp;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            popUp.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
