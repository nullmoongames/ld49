using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Cinemachine;
using THOR;
using Crest;

public class GameManager : MonoBehaviour
{
    [Header("Score")]
    public int distanceCover = 0;
    public TMP_Text scoreText;
    public TMP_Text highscoreText;
    private float highscore;

    [Header("UI Menu")]
    public CanvasGroup gameUI;
    public CanvasGroup mainMenuUI;
    public CanvasGroup deathMenuUI;
    public CinemachineVirtualCamera mainMenuCam;

    [Header("Chaos")]
    public float chaosPercent;
    public THOR_Thunderstorm thunder;
    public Light mainLight;
    private float _startingLightIntensity;
    public Color chaosLightColor;
    private Color _startingLightColor;
    public OceanWaveSpectrum waves;
    public float startWavesMultiplier = 1f;
    public float maxWavesMultiplier = 1.5f;

    //Reload Management
    private Transform _player;
    private Vector3 _playerStartingPos;
    private bool _gameIsPlaying;

    //Instance
    public static GameManager instance;

    private float _startingZPos;

    private void Awake()
    {
        instance = this;

        _startingLightColor = mainLight.color;
        _startingLightIntensity = mainLight.intensity;

        _player = FindObjectOfType<PlayerController>().transform;
        _startingZPos = _player.transform.position.z;
        _player.gameObject.SetActive(false);
        _playerStartingPos = _player.transform.position;

        _FadeToMainMenu();
        scoreText.text = "<b>" + 0 + "</b>m";
        _LoadHighscore();

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
        mainLight.intensity = (_startingLightIntensity - (chaosPercent / 100));
        mainLight.color = Color.Lerp(_startingLightColor, chaosLightColor, (chaosPercent / 100));
        waves._multiplier = Mathf.Lerp(startWavesMultiplier, maxWavesMultiplier, (chaosPercent / 100));
    }

    public void ReloadLevel()
    {
        LevelGenerationManager.instance.DestroyBoats();
        LevelGenerationManager.instance.StartLevelGeneration();
        _player.transform.position = _playerStartingPos;
        scoreText.text = "<b>" + 0 + "</b>m";
        waves._multiplier = startWavesMultiplier;
        mainLight.color = _startingLightColor;
        mainLight.intensity = _startingLightIntensity;
        thunder.probability = 0;
        chaosPercent = 0;
    }

    void _UpdatePlayerMeters() 
    {
        //To be sure that the player meter do not decrease
        if(distanceCover < ((int)_player.transform.position.z - (int)_startingZPos)) 
        {
            scoreText.text = "<b>" + distanceCover + "</b>m";
            distanceCover = (int)_player.transform.position.z - (int)_startingZPos;
        }
    }

    void _UpdateChaosPercent()
    {
        if (chaosPercent < 100)
            chaosPercent = (distanceCover / 25);
        else
            chaosPercent = 100;
    }

    public void Play() 
    {
        _player.gameObject.SetActive(true);
        _FadeToGame();
        _gameIsPlaying = true;
        GameplayMusic.Instance.SwitchToGameplayMusic();
    }

    public void DisplayDeathScreen()
    {
        _FadeToDeathScreen();
    }

    public void SaveHighscore()
    {
        highscore = distanceCover;
        Debug.Log("Highscore : " + highscore);

        if (PlayerPrefs.GetInt("Highscore") < highscore)
            PlayerPrefs.SetInt("Highscore", (int)highscore);

        distanceCover = 0;
    }

    void _LoadHighscore() 
    {
        highscoreText.text = "Highscore : " + PlayerPrefs.GetInt("Highscore");
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
