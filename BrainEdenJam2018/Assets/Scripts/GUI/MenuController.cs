using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    [SerializeField] private Slider LoadingBar;
    [SerializeField] private Text LoadingText;

	public void Start(){
		Cursor.lockState = CursorLockMode.None;
	}

	public void PlayGame(){
        StartCoroutine(LoadGameAsync());
	}

	public void QuitGame(){
		Application.Quit ();
	}

    private IEnumerator LoadGameAsync()
    {
        var async = SceneManager.LoadSceneAsync("Game");
        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            LoadingBar.value = async.progress;
            LoadingText.text = "Progress - " + (async.progress * 100f) + "%";

            if(async.progress >= 0.9f)
            {
                LoadingBar.value = 1f;
                LoadingText.text = "Progress - " + (100f) + "%";
                yield return new WaitForSeconds(1.0f);

                async.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
