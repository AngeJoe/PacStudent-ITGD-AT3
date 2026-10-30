using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;

    public LevelGenerator levelGenerator;
    public PacStudentController player;
    public AudioPlayer audioPlayer;
    public GhostController[] ghosts;
    public ParticleSystem deathParticles;

    public TMP_Text scoreText;
    public TMP_Text gameTimerText;
    public TMP_Text scaredTimerText;
    public TMP_Text countdownText;
    public TMP_Text gameOverText;
    public Image[] lifeImages;
    public GameObject blockingPanel;
    public Button exitButton;

    public int lives = 3;
    public float deathTime = 2f;
    public bool isPlaying = false;
    public bool isDying = false;

    int score = 0;
    int pelletsLeft = 0;
    float gameTime = 0;
    public float scaredTime = 0;
    bool gameOver = false;
    Vector3 playerStart = Vector3.zero;

    void Awake()
    {
        inst = this;
    }

    IEnumerator Start()
    {
        playerStart = player.transform.position;
        blockingPanel.SetActive(true);
        countdownText.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(false);

        // The level is generated in LevelGenerator.Start().
        yield return null;
        pelletsLeft = levelGenerator.pelletCount;
        UpdateHUD();

        audioPlayer.PlayIntro();

        countdownText.text = "3";
        yield return new WaitForSeconds(1f);
        countdownText.text = "2";
        yield return new WaitForSeconds(1f);
        countdownText.text = "1";
        yield return new WaitForSeconds(1f);
        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);

        countdownText.gameObject.SetActive(false);
        blockingPanel.SetActive(false);
        isPlaying = true;
        UpdateGhostStates();
        UpdateMusic();
    }

    void Update()
    {
        if (!isPlaying || gameOver)
            return;

        gameTime += Time.deltaTime;

        if (scaredTime > 0)
        {
            string oldState = GetGhostState();
            scaredTime -= Time.deltaTime;
            if (scaredTime < 0)
                scaredTime = 0;

            if (oldState != GetGhostState())
            {
                UpdateGhostStates();
                UpdateMusic();
            }
        }

        UpdateHUD();
    }

    public static string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int ms = Mathf.FloorToInt((time * 100) % 100f);
        string timeString = minutes.ToString("D2") + ":" + seconds.ToString("D2") + ":" + ms.ToString("D2");
        return timeString;
    }

    void UpdateHUD()
    {
        scoreText.text = score.ToString("D6");
        gameTimerText.text = FormatTime(gameTime);
        scaredTimerText.text = Mathf.CeilToInt(scaredTime).ToString();
        scaredTimerText.enabled = scaredTime > 0;

        for (int i = 0; i < lifeImages.Length; i++)
            lifeImages[i].enabled = i < lives;
    }

    public void AddScore(int amount)
    {
        if (!isPlaying)
            return;

        score += amount;
        UpdateHUD();
    }

    public void PelletCollected()
    {
        if (!isPlaying)
            return;

        pelletsLeft--;

        if (pelletsLeft <= 0)
            EndGame();
    }

    public string GetGhostState()
    {
        if (scaredTime <= 0)
            return "";
        else if (scaredTime <= 3)
            return "Recovering";
        else
            return "Scared";
    }

    public void StartScaredTimer()
    {
        if (!isPlaying)
            return;

        scaredTime = 10;
        UpdateGhostStates();
        UpdateMusic();
        UpdateHUD();
    }

    void UpdateGhostStates()
    {
        foreach (GhostController ghost in ghosts)
            if (ghost.state != "Dead")
                ghost.ChangeState(GetGhostState());
    }

    public void GhostEaten(GhostController ghost)
    {
        if (!isPlaying || ghost.state == "Dead")
            return;

        AddScore(300);
        ghost.ChangeState("Dead");
        audioPlayer.PlaySound(audioPlayer.ghostEatSound);
        UpdateMusic();
    }

    public void UpdateMusic()
    {
        if (gameOver || !isPlaying)
            return;

        foreach (GhostController ghost in ghosts)
        {
            if (ghost.state == "Dead")
            {
                audioPlayer.PlayDead();
                return;
            }
        }

        if (scaredTime > 0)
            audioPlayer.PlayScared();
        else
            audioPlayer.PlayNormal();
    }

    public void PlayerHit()
    {
        if (isPlaying && !isDying && !gameOver)
            StartCoroutine(PlayerDeath());
    }

    IEnumerator PlayerDeath()
    {
        isPlaying = false;
        isDying = true;
        lives--;
        UpdateHUD();

        player.animator.speed = 1;
        player.animator.SetBool("isDead", true);
        deathParticles.transform.position = player.transform.position;
        deathParticles.Play();
        audioPlayer.StopMusic();
        audioPlayer.PlaySound(audioPlayer.deathSound);

        yield return new WaitForSeconds(deathTime);
        isDying = false;

        if (lives <= 0)
        {
            EndGame();
            yield break;
        }

        scaredTime = 0;
        player.transform.position = playerStart;
        // Reset movement?

        foreach (GhostController ghost in ghosts)
            ghost.ResetGhost();

        isPlaying = true;
        UpdateHUD();
        UpdateMusic();
    }

    public void EndGame()
    {
        if (gameOver)
            return;

        isPlaying = false;
        gameOver = true;
        exitButton.interactable = false;
        audioPlayer.StopMusic();
        SaveHighScore();
        StartCoroutine(ShowGameOver());
    }

    void SaveHighScore()
    {
        int oldScore = PlayerPrefs.GetInt("Level1BestScore", 0);
        float oldTime = PlayerPrefs.GetFloat("Level1BestTime", 0);

        if (!PlayerPrefs.HasKey("Level1BestScore") || score > oldScore ||
            (score == oldScore && gameTime < oldTime))
        {
            PlayerPrefs.SetInt("Level1BestScore", score);
            PlayerPrefs.SetFloat("Level1BestTime", gameTime);
            PlayerPrefs.Save();
        }
    }

    IEnumerator ShowGameOver()
    {
        blockingPanel.SetActive(true);
        countdownText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(true);
        gameOverText.text = "Game Over";

        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("StartScene");
    }

    public void ExitLevel()
    {
        if (!gameOver)
            SceneManager.LoadScene("StartScene");
    }
}