using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int distanceCover = 0; //Score

    private float highscore;

    private Transform _player;
    private Vector3 _playerStartingPos;

    private bool _gameIsPlaying;

    private void Awake()
    {
        instance = this;

        _player = FindObjectOfType<PlayerController>().transform;
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
    }

    void _UpdatePlayerMeters() 
    {
        //To be sure that the player meter do not decrease
        if(distanceCover < (int)_player.transform.position.z)
            distanceCover = (int)_player.transform.position.z;
    }

    void _SaveHighscore()
    {

    }

    void _LoadHighscore() 
    {

    }
}
