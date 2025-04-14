using UnityEngine;

public class StartBossFight : MonoBehaviour
{
    private GameObject GameLogic;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameLogic = GameObject.Find("GameLogic");
        GameLogic.GetComponent<GameLogic>().StartBossFight();
    }

}
