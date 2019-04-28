﻿using System.Collections;
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
                case (0):
                    gameState = GameState.Menu;
                    break;
                default:
                    Player = FindObjectOfType<PlayerController>();
                    MainCamera = FindObjectOfType<CameraController>();
                    gameState = GameState.Playing;
                    break;
            }

            SceneManager.sceneLoaded += OnSceneLoadedListener;
        }
        else
            Destroy(gameObject);
    }


	void Update ()
    { 
        if(gameState == GameState.Playing)
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

    public void PlayGame()
    {
        UIManager.Instance.StompCDImage.gameObject.SetActive(true);
        UIManager.Instance.FireCDImage.gameObject.SetActive(true);
        UIManager.Instance.SetMainMenu(false);
        UIManager.Instance.PauseBackgroundPanel.SetActive(false);

        gameState = GameState.Playing;
    }

    public void PauseGame()
    {
        UIManager.Instance.StompCDImage.gameObject.SetActive(false);
        UIManager.Instance.FireCDImage.gameObject.SetActive(false);
        UIManager.Instance.SetMainMenu(true);
        UIManager.Instance.PauseBackgroundPanel.SetActive(true);

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
