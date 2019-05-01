﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

 
    public GameObject MainMenu;
    public Button PlayButton;
    public Button ContinueButton;

    public GameObject OptionsMenu;
    public AudioMixer Mixer;

    public GameObject ControlsMenu;
    public GameObject GameOverMenu;

    public GameObject TitleScreenElements;
    public GameObject PauseBackgroundPanel;

    public Image HitPointUI;
    public Image StompCDImage;
    public Image FireCDImage;

    public Image TextBackground; //need to have references to both since text has to be child of image because of the layout group
    public Text MessageDisplayText;
    public Queue<string> MessageQueue = new Queue<string>();

    public static UIManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoadedListener;
        }
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if(MessageQueue.Count > 0)
        {
            DisplayText(MessageQueue.Peek());

            if (Input.GetKeyDown(KeyCode.Return))
            {
                MessageQueue.Dequeue();
                if (MessageQueue.Count == 0)
                    FinishTextDisplay();
            }
        }
    }

    void OnSceneLoadedListener(Scene scene, LoadSceneMode mode)
    {
        int SceneIndex = SceneManager.GetActiveScene().buildIndex;

        switch (SceneIndex)
        {
            case (0):     
                MainMenu.SetActive(true);
                ContinueButton.gameObject.SetActive(false);
                PlayButton.gameObject.SetActive(true);
                GameOverMenu.SetActive(false);
                TitleScreenElements.SetActive(true);
                PauseBackgroundPanel.SetActive(false);
                HitPointUI.gameObject.SetActive(false);
                StompCDImage.gameObject.SetActive(false);
                FireCDImage.gameObject.SetActive(false);
                break;
            case (4):
                MainMenu.SetActive(false);
                GameOverMenu.SetActive(false);
                TitleScreenElements.SetActive(false);
                PauseBackgroundPanel.SetActive(false);
                HitPointUI.gameObject.SetActive(false);
                StompCDImage.gameObject.SetActive(false);
                FireCDImage.gameObject.SetActive(false);
                break;
            case (3):
                break;
            default:              
                MainMenu.SetActive(false);
                ContinueButton.gameObject.SetActive(true);
                PlayButton.gameObject.SetActive(false);
                GameOverMenu.SetActive(false);
                TitleScreenElements.SetActive(false);
                PauseBackgroundPanel.SetActive(false);
                HitPointUI.gameObject.SetActive(true);
                StompCDImage.gameObject.SetActive(true);
                FireCDImage.gameObject.SetActive(true);
                break;
        }
    }

    public void DisplayText(string message)
    {
        TextBackground.gameObject.SetActive(true);
        MessageDisplayText.text = message;
        GameManager.GM.SetState(GameManager.GameState.Dialogue);    
    }

    public void FinishTextDisplay()
    {
        TextBackground.gameObject.SetActive(false);
        MessageDisplayText.text = null;
        GameManager.GM.SetState(GameManager.GameState.Playing);
    }

    public void SetMainMenu(bool state)
    {
        MainMenu.SetActive(state);
    }

    public void SetOptionsMenu(bool state)
    {
        OptionsMenu.SetActive(state);
    }

    public void SetControlsMenu(bool state)
    {
        ControlsMenu.SetActive(state);
    }

    public void SetGameOverMenu(bool state)
    {
        GameOverMenu.SetActive(state);
    }

    public void SetVolume(float volume)
    {
        Mixer.SetFloat("Volume", volume);
    }
}
