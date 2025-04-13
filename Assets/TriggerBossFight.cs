using UnityEngine;

public class TriggerBossFight : MonoBehaviour
{
    private GameObject GameLogic;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameLogic = GameObject.Find("GameLogic");
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 9) //if hit player
        {
            GameLogic.GetComponent<GameLogic>().StartBossFight();
        }
    }
}
