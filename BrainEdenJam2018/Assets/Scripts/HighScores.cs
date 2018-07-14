using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScores : MonoBehaviour {

    private Humanoid PlayerScript;
    public Text HighScore;
    public Text Score;

	// Use this for initialization
	void Start () {

        PlayerScript = GetComponent<Humanoid>();
    }
	
	// Update is called once per frame
	void Update () {
		
        if(PlayerScript)
        {
            int current = PlayerScript.Score;

            Score.text = current.ToString();

            if(current > PlayerPrefs.GetInt("HighScore", 0))
            {
                PlayerPrefs.SetInt("HighScore", current);
            }
        }

        HighScore.text = PlayerPrefs.GetInt("HighScore", 0).ToString();

        PlayerPrefs.Save();
    }
}
