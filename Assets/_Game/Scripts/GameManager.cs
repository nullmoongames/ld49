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
    public TMP_Text highscoreTextPause;
    private float highscore;

    [Header("UI Menu")]
    public CanvasGroup gameUI;
    public CanvasGroup mainMenuUI;
    public CanvasGroup deathMenuUI;
    public GameObject mainMenuGameObject;
    public GameObject creditsMenuGameObject;
    public CinemachineVirtualCamera mainMenuCam;
    AudioSource audioSource;

    [Header("Intro")]
    public CinemachineVirtualCamera IntroVCam1;
    public CinemachineVirtualCamera IntroVCam2;
    public GameObject pirateRunnerIntro;

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

    [Header("Reload")]
    //Reload Management
    private Transform _player;
    private Vector3 _playerStartingPos;
    public bool _gameIsPlaying;
    public Transform fire;
    private Vector3 _startFirePos;

    //Instance
    public static GameManager instance;

    private float _startingZPos;

    private void Awake()
    {
        instance = this;

        _startFirePos = fire.transform.position;

        _startingLightColor = mainLight.color;
        _startingLightIntensity = mainLight.intensity;

        _player = FindObjectOfType<PlayerController>().transform;
        _startingZPos = _player.transform.position.z;
        _player.parent.gameObject.SetActive(false);
        _playerStartingPos = _player.transform.position;

        audioSource = GetComponent<AudioSource>();

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
        fire.transform.position = _startFirePos;
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
        PlayIntro1();
        _FadeToGame();
        GameplayMusic.Instance.SwitchToGameplayMusic();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void PlayIntro1()
    {
        IntroVCam1.Priority = 100;
        Invoke("PlayIntro2", 4f);
    }

    void PlayIntro2()
    {
        pirateRunnerIntro.SetActive(true);
        IntroVCam1.Priority = 0;
        IntroVCam2.Priority = 100;
        Invoke("DoPlay", 4f);
    }

    void DoPlay()
    {
        IntroVCam2.Priority = 0;
        pirateRunnerIntro.SetActive(false);
        _player.parent.gameObject.SetActive(true);
        _gameIsPlaying = true;
    }

    public void DisplayDeathScreen()
    {
        _FadeToDeathScreen();
    }

    public void DisplayCredits(bool display)
    {
        creditsMenuGameObject.SetActive(display);
        mainMenuGameObject.SetActive(!display);
    }

    public void PlayBellSound()
    {
        audioSource.Play();
    }

    public void ExitGame()
    {
        Application.Quit();
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
        highscoreText.text = "Highscore: " + PlayerPrefs.GetInt("Highscore");
        highscoreTextPause.text = "Highscore: " + PlayerPrefs.GetInt("Highscore");
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
