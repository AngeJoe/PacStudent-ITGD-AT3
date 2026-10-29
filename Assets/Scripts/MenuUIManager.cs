using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuUIManager : MonoBehaviour
{
    public TMP_Text highscoreText;
    public TMP_Text level2Text;

    void Start()
    {
        int score = PlayerPrefs.GetInt("Level1BestScore", 0);
        float time = PlayerPrefs.GetFloat("Level1BestTime", 0);
        highscoreText.text = "Level 1 Best:\n" + score.ToString("D6") + "\n" + GameManager.FormatTime(time);
        level2Text.text = "Level 2 Best:\n000000\n00:00:00";
    }

    public void LoadFirstLevel()
    {
        SceneManager.LoadSceneAsync(1);
    }
}