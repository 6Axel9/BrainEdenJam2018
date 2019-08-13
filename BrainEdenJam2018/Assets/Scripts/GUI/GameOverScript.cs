using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour {

    public Text Score;
    public Text HighScore;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;

        HighScore.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        Score.text = PlayerPrefs.GetInt("CurrentScore", 0).ToString();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
