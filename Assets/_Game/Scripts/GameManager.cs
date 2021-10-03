using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Cinemachine;
using THOR;

public class GameManager : MonoBehaviour
{
    [Header("Score")]
    public int distanceCover = 0;
    public TMP_Text scoreText;
    private float highscore;

    [Header("UI Menu")]
    public CanvasGroup gameUI;
    public CanvasGroup mainMenuUI;
    public CanvasGroup deathMenuUI;
    public CinemachineVirtualCamera mainMenuCam;

    [Header("Chaos")]
    public float chaosPercent;
    public THOR_Thunderstorm thunder;

    //Reload Management
    private Transform _player;
    private Vector3 _playerStartingPos;
    private bool _gameIsPlaying;

    //Instance
    public static GameManager instance;

    private void Awake()
    {
        instance = this;

        _player = FindObjectOfType<PlayerController>().transform;
        _player.gameObject.SetActive(false);
        _playerStartingPos = _player.transform.position;

        _FadeToMainMenu();
    }

    private void Update()
    {
        if (!_gameIsPlaying)
            return;

        _UpdatePlayerMeters();
        _UpdateChaosPercent();
        _UpdateChaos();
    }

    void _UpdateChaos() 
    {
        thunder.probability = (chaosPercent / 100);
    }

    public void ReloadLevel()
    {
        LevelGenerationManager.instance.DestroyBoats();
        LevelGenerationManager.instance.StartLevelGeneration();
        _player.transform.position = _playerStartingPos;
        distanceCover = 0;
        //Reset water
    }

    void _UpdatePlayerMeters() 
    {
        //To be sure that the player meter do not decrease
        if(distanceCover < (int)_player.transform.position.z) 
        {
            scoreText.text = "<b>" + distanceCover + "</b>m";
            distanceCover = (int)_player.transform.position.z;
        }
    }

    void _UpdateChaosPercent()
    {
        if (chaosPercent < 100)
            chaosPercent = (distanceCover / 50);
        else
            chaosPercent = 100;
    }

    public void Play() 
    {
        _player.gameObject.SetActive(true);
        _FadeToGame();
        _gameIsPlaying = true;
    }

    public void DisplayDeathScreen()
    {
        _FadeToDeathScreen();
    }

    void _SaveHighscore()
    {

    }

    void _LoadHighscore() 
    {

    }

    void _FadeToGame() 
    {
        mainMenuCam.Priority = 0;
        gameUI.DOFade(1, .5f);
        mainMenuUI.DOFade(0, .5f);
        //blurMainMenuUI.DOFade(0, .5f);
        deathMenuUI.DOFade(0, .5f);

    }

    void _FadeToMainMenu()
    {
        gameUI.DOFade(0, .5f);
        mainMenuUI.DOFade(1, .5f);
        //blurMainMenuUI.DOFade(1, .5f);
        deathMenuUI.DOFade(0, .5f);
        mainMenuCam.Priority = 3;
    }

    void _FadeToDeathScreen()
    {
        gameUI.DOFade(0, .5f);
        mainMenuUI.DOFade(0, .5f);
        //blurMainMenuUI.DOFade(0, .5f);
        deathMenuUI.DOFade(1, .5f);
        mainMenuCam.Priority = 0;
    }
}
