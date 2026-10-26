using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void LoadFirstLevel()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
