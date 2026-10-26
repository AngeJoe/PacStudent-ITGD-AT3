using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour
{
    public void LoadStartLevel()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
