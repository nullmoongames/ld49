using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayMusic : MonoBehaviour
{
    private static GameplayMusic _Instance;

    public static GameplayMusic Instance { get { return _Instance; } }

    private void Awake()
    {
        GameplayMusic music = FindObjectOfType<GameplayMusic>();

        if (music != null && music != this)
        {
            Destroy(this.gameObject);
        } else
        {
            _Instance = this;
            DontDestroyOnLoad(this);
        }
    }
}
