using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public event Action onComplete;
    public event Action onReset;
    public event Action onFail;

    public PlayerController playerController;
    public GrannyController grannyController;
    public LevelsManager levelsManager;
    public UIManager uIManager;
    public void Start()
    {

    }
    public void ResetData()
    {
        uIManager.ResetLevel();
        playerController.Respawn();
        levelsManager.ResetLevel();
        Debug.Log("data reseted");
    }
    public void LevelFail()
    {
        uIManager.LevelFail();
        Debug.Log("game fail");
    }
    public void LevelComplete()
    {
        uIManager.LevelComplete();
        Debug.Log("game complete");
    }
}
