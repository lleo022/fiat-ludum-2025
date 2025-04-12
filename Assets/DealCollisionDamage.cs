using System.Xml.Serialization;
using UnityEngine;

public class DealCollisionDamage : MonoBehaviour
{
    private GameObject GameLogic;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameLogic = GameObject.Find("GameLogic");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Collided with death cube");
        GameLogic.GetComponent<GameLogic>().hurtPlayer(1);
    }
}
