using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchToScene : MonoBehaviour
{

    public int Scene;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 9)
        {
            //player went through
            SceneManager.LoadSceneAsync(Scene);
        }
        
    }
}
