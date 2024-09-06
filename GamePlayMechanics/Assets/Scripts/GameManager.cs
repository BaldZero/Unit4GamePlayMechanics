using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private int playerCount;
    public bool gameOver;
    public RawImage gameOverScreen;
    public RawImage startScreen;
    public RawImage pauseScreen;
    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        gameOverScreen.gameObject.SetActive(false);
        Time.timeScale = 0f;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }

        GameOver();
        
    }


    public void GameOver()
    {
        if (gameOver)
        {
            Time.timeScale = 0f;
            gameOverScreen.gameObject.SetActive(true);
        }
    }

    public void RestartGame()
    {
        gameOver = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        startScreen.gameObject.SetActive(false);

    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseScreen.gameObject.SetActive(true);
        
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseScreen.gameObject.SetActive(false );

    }
}
