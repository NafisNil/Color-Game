using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public static LevelManager LM;
	public GameObject winPanel;
	public GameObject gameOverPanel;
	public GameObject startPanel;
	public bool isStarted;

	public int levelNumber;
	// Use this for initialization
	void Start () {
		LevelManager.LM = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartGame(){
		isStarted = true;
  
		startPanel.SetActive (false);
	}

	public void TryAgain(){
		SceneManager.LoadScene(levelNumber);
	}

	public void NextLevel(){
		levelNumber++;
		SceneManager.LoadScene(levelNumber);
	}

    public void GameStart()
    {
        SceneManager.LoadScene("Level01");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
