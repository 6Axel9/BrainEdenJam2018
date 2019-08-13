using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGamePauseMenu : MonoBehaviour {

    public GameObject menu;
    public Text Score;

    public GameObject PlayerGun;

	void Start () {

        menu.SetActive(false);
        Score.text = "0";
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.P)) {
            var mouse = GameObject.FindGameObjectWithTag("Player").GetComponent<SmoothMouseLook>();
            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<Humanoid>();
            if (menu.activeSelf)
            {
                PlayerGun.SetActive(true);
                menu.SetActive(false);
                mouse.enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                PlayerGun.SetActive(false);
                menu.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                mouse.enabled = false;
                Score.text = "Score " + player.Score;
            }
        }
	}

	public void BackToPlay(){
        PlayerGun.SetActive(true);
        menu.SetActive(false);
        GameObject.FindGameObjectWithTag ("Player").GetComponent<SmoothMouseLook> ().enabled = true;
		Cursor.lockState = CursorLockMode.Locked;
	}

	public void GoToMainMenu(){
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex - 1);
	}
		
	public void QuitGame(){
		Application.Quit ();
	}
}
