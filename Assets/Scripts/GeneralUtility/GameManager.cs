using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public PlayerController Player;
    public CameraController MainCamera;
    public AudioSource MainAudio;


    public delegate void OnSetState(GameState state);
    public OnSetState OnSetStateCallback;

    public enum GameState
    {
        Menu,
        Playing,
        Dialogue,
        Paused,
        Loading,
        Cutscene,
        Gameover,
        Finished
    }

    public GameState CurrentSate
    {
        get { return _gameState; }
    }
     
    private GameState _gameState;
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
                case (0):
                    _gameState = GameState.Menu;
                    break;
                default:
                    Player = FindObjectOfType<PlayerController>();
                    MainCamera = FindObjectOfType<CameraController>();
                    _gameState = GameState.Playing;
                    break;
            }

            SceneManager.sceneLoaded += OnSceneLoadedListener;      
        }
        else
            Destroy(gameObject);
    }


	void Update ()
    { 
        if(_gameState == GameState.Playing)
        {
            if (Input.GetKey(KeyCode.Escape))
                PauseGame();
        }
	}

    void OnSceneLoadedListener(Scene scene, LoadSceneMode mode) //listener for SceneManager.sceneLoaded
    {
        int SceneIndex = SceneManager.GetActiveScene().buildIndex;

        switch (SceneIndex)
        {
            case (0):
                _gameState = GameState.Menu;
                break;
            case (3):            
                break;
            default:
                Player = FindObjectOfType<PlayerController>();
                MainCamera = FindObjectOfType<CameraController>();
                MainAudio = FindObjectOfType<AudioSource>();
                _gameState = GameState.Playing;
                break;
        }
    }

    public void LoadScene(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
       // LoadingManager.LoadScene(sceneNum);
    }

    public void RestartScene()
    {     
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //LoadingManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //LoadingManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SetState(GameManager.GameState state)
    {
        _gameState = state;
        if(OnSetStateCallback != null)
             OnSetStateCallback.Invoke(state);
    }

    public void PlayGame()
    {
        UIManager.Instance.StompCDImage.gameObject.SetActive(true);
        UIManager.Instance.FireCDImage.gameObject.SetActive(true);
        UIManager.Instance.SetMainMenu(false);
        UIManager.Instance.PauseBackgroundPanel.SetActive(false);

        _gameState = GameState.Playing;
    }

    public void PauseGame()
    {
        UIManager.Instance.StompCDImage.gameObject.SetActive(false);
        UIManager.Instance.FireCDImage.gameObject.SetActive(false);
        UIManager.Instance.SetMainMenu(true);
        UIManager.Instance.PauseBackgroundPanel.SetActive(true);

        _gameState = GameState.Paused;
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
