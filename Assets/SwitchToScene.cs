using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchToScene : MonoBehaviour
{

    public int Scene;

    private void OnTriggerEnter2D(Collider2D col)
    {
        SceneManager.LoadSceneAsync(Scene);
    }
}
