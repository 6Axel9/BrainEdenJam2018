using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScores : MonoBehaviour {

    private Humanoid PlayerScript;
    public Text CurrentScore;

	// Use this for initialization
	void Start () {

        PlayerScript = GetComponent<Humanoid>();
    }
	
	// Update is called once per frame
	void Update () {
		
        if(PlayerScript)
        {
            int current = PlayerScript.Score;

            CurrentScore.text = "Score " + current.ToString();

            if (!PlayerPrefs.HasKey("HighScore")) {
                PlayerPrefs.SetInt("HighScore", 0);
            }

            if(current > PlayerPrefs.GetInt("HighScore", 0)) {
                PlayerPrefs.SetInt("HighScore", current);
            }

            PlayerPrefs.SetInt("CurrentScore", current);
        }

        PlayerPrefs.Save();
    }
}
