using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameStates
{
    START,
    INGAME,
    PAUSE,
    GAMEOVER,
    RANKING
}

public class GameManager : MonoBehaviour
{
    private float score;
    public int maxScore;

    private float currentTimeToShowNewBestScore = 0;

    public float slowMotionSpeed;

    public float slowMotionTime;
    private float currentSlowMotionTime;

    private bool isSlowMotionTime;

    public Vector3 playerStartPosition;

    public Transform player;

    private PlayerMovimentController playerMoviment;
    private PlayerLifeController playerLife;
    private SpawnerController spawner;

    public GameObject deathParticle;

    public GameObject pauseHUD;
    public GameObject gameHUD;
    public GameObject gameOverHUD;

    public AudioSource gameOverSound;

    public Text gameScoreText;

    public Text maxScoreText;
    public Text currentScoreText;

    public Animator gameOverAnimator;

    private GameStates currentState = GameStates.START;

    public float timeToFade;

    public AudioSource buttonSound;

    string nomeDaCenaAtual;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("maxScore"))
        {
            maxScore = PlayerPrefs.GetInt("maxScore");
        }

        maxScoreText.text = $"Best Score: {maxScore}";
        currentScoreText.text = $"Score: {(int)score}";

        playerStartPosition = player.position;
        playerMoviment = FindObjectOfType<PlayerMovimentController>().GetComponent<PlayerMovimentController>();
        spawner = FindObjectOfType<SpawnerController>().GetComponent<SpawnerController>();
        playerLife = FindObjectOfType<PlayerLifeController>().GetComponent<PlayerLifeController>();

        nomeDaCenaAtual = SceneManager.GetActiveScene().name;


    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case GameStates.START:
                spawner.enabled = true;
                player.position = playerStartPosition;
                playerMoviment.canMove = true;
                currentState = GameStates.INGAME;
                break;
            case GameStates.INGAME:
                spawner.enabled = true;
                playerMoviment.canMove = true;
                break;
            case GameStates.PAUSE:
                playerMoviment.canMove = false;
                break;
            case GameStates.GAMEOVER:
                playerMoviment.canMove = false;
                VerifyScore();
                ShowScore();
                gameOverHUD.SetActive(true);
                Time.timeScale = 1;
                break;
            case GameStates.RANKING:
                playerMoviment.canMove = false;
                break;

        }

        if (isSlowMotionTime)
        {
            currentSlowMotionTime += Time.deltaTime;
            spawner.currentTimeToIncreaseSpeed = 0;
            spawner.powerUpsSpawnRate = 50f;
            spawner.powerUpsCurrentSpawnRate = 0f;
            spawner.obstaclesSpawnRate = 1f;
            spawner.obstaclesCurrentSpawnRate = 0f;
        }

        if (currentSlowMotionTime >= slowMotionTime)
        {
            spawner.moveSpeed = spawner.currentMoveSpeed;
            currentSlowMotionTime = 0;
            spawner.powerUpsSpawnRate = 15f;
            spawner.powerUpsCurrentSpawnRate = 0f;
            spawner.obstaclesSpawnRate = 1f;
            spawner.obstaclesCurrentSpawnRate = 0f;
            isSlowMotionTime = false;
        }

        if (playerMoviment.canMove)
        {
            score += Time.deltaTime * spawner.moveSpeed;
            UpdateScore((int)score);
        }
    }

    public void UpdateScore(int score)
    {
        gameScoreText.text = $"Score: {score}m";
    }

    public void CallGameOver()
    {
        player.gameObject.SetActive(false);
        Instantiate(deathParticle, player.position, player.rotation);
        gameHUD.SetActive(false);
        gameOverSound.Play();
        currentState = GameStates.GAMEOVER;
    }

    public GameStates GetGameState()
    {
        return currentState;
    }

    public void SetSlowMotion()
    {
        spawner.currentMoveSpeed = spawner.moveSpeed;
        spawner.moveSpeed = slowMotionSpeed;
        isSlowMotionTime = true;
    }

    public void GetExtraLife()
    {
        playerLife.lifeCount++;

        if (playerLife.lifeCount > 2)
        {
            playerLife.lifeCount = 2;
        }
    }

    public void PauseGame()
    {
        pauseHUD.SetActive(true);
        gameHUD.SetActive(false);
        spawner.GetComponent<SpawnerController>().gameplayMusicSound.Stop();
        buttonSound.Play();
        currentState = GameStates.PAUSE;

    }

    public void ResumeGame()
    {
        pauseHUD.SetActive(false);
        gameHUD.SetActive(true);
        spawner.GetComponent<SpawnerController>().gameplayMusicSound.Play();
        buttonSound.Play();
        currentState = GameStates.INGAME;
    }

    public void RestartGame()
    {
        buttonSound.Play();
        SceneManager.LoadScene(nomeDaCenaAtual);
        Time.timeScale = 1;
    }

    public void VerifyScore()
    {
        if (score > maxScore)
        {
            maxScore = (int)score;
            PlayerPrefs.SetInt("maxScore", maxScore);
        }
    }

    public void ShowScore()
    {
        if (PlayerPrefs.HasKey("maxScore"))
        {
            maxScore = PlayerPrefs.GetInt("maxScore");
        }

        maxScoreText.text = $"Best Score: {maxScore}";
        currentScoreText.text = $"Score: {(int)score}";

        if(score >= maxScore)
        {
            currentTimeToShowNewBestScore += Time.deltaTime;

            if(currentTimeToShowNewBestScore >= 0.9f) gameOverAnimator.SetBool("hasNewMaxScore", true);

        }


    }

    public void GoToMenu()
    {
        buttonSound.Play();

        SceneManager.LoadScene("Menu");

    }

}
