using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGamePauseMenu : MonoBehaviour
{

    public GameObject menu;

    void Start()
    {

        menu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (menu.activeSelf)
            {
                menu.SetActive(false);
                GameObject.FindGameObjectWithTag("Player").GetComponent<SmoothMouseLook>().enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                menu.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                GameObject.FindGameObjectWithTag("Player").GetComponent<SmoothMouseLook>().enabled = false;
            }
        }
    }

    public void BackToPlay()
    {
        menu.SetActive(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<SmoothMouseLook>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
