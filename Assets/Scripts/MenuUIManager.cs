using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIManager : MonoBehaviour
{
    public void LoadFirstLevel()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
