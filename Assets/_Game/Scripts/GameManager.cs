using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int distanceCover = 0; //Score

    private float highscore;

    private Transform _player;
    private Vector3 _playerStartingPos;

    private bool _gameIsPlaying;

    public TMP_Text scoreText;

    public GameObject gameUI;

    private void Awake()
    {
        instance = this;

        _player = FindObjectOfType<PlayerController>().transform;
        _player.gameObject.SetActive(false);
        _playerStartingPos = _player.transform.position;
    }

    private void Update()
    {
        if (!_gameIsPlaying)
            return;

        _UpdatePlayerMeters();
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

    public void Play() 
    {
        _player.gameObject.SetActive(true);
        gameUI.SetActive(true);
    }

    void _SaveHighscore()
    {

    }

    void _LoadHighscore() 
    {

    }
}
