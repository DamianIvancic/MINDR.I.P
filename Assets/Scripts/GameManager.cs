using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public PlayerController Player;
    public CameraController MainCamera;
    public AudioSource MainAudio;

    public enum GameState
    {
        Menu,
        Playing,
        Dialogue,
        Paused,
        Cutscene,
        Gameover,
        Finished
    }

    [HideInInspector]
    public GameState gameState;
    [HideInInspector]
    public static GameManager GM;


    void Awake()
    {
        if (GM == null)
        {
            GM = this;
            DontDestroyOnLoad(gameObject);

            int sceneIndex = SceneManager.GetActiveScene().buildIndex;

            switch(sceneIndex)
            {
               /* case (0):
                    gameState = GameState.Menu;
                    break;*/
                default:
                    Player = FindObjectOfType<PlayerController>();
                    MainCamera = FindObjectOfType<CameraController>();
                    gameState = GameState.Playing;
                    break;
            }
        }
        else
            Destroy(gameObject);
    }


	void Update () {
		
	}

    void OnSceneLoadedListener(Scene scene, LoadSceneMode mode) //listener for SceneManager.sceneLoaded
    {
        int SceneIndex = SceneManager.GetActiveScene().buildIndex;

        switch (SceneIndex)
        {
            case (0):
                gameState = GameState.Menu;
                break;
            default:
                Player = FindObjectOfType<PlayerController>();
                MainCamera = FindObjectOfType<CameraController>();
                gameState = GameState.Playing;
                break;
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadScene(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);

    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame()
    {
        //UIManager.Instance.SetMenu(false);
       // UIManager.Instance.HealthDisplay.SetActive(true);

        gameState = GameState.Playing;
    }

    public void PauseGame()
    {
        //UIManager.Instance.SetMenu(true);
        //UIManager.Instance.HealthDisplay.SetActive(false);

        gameState = GameState.Paused;
    }

    public void QuitGame()
    {
        int SceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (SceneIndex == 0)
            Application.Quit();
        else
            SceneManager.LoadScene(0);
    }
}
