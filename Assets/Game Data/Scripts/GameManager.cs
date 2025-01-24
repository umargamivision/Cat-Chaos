using System;
using System.Collections;
using System.Collections.Generic;
using Ommy.SaveData;
using Ommy.Singleton;
using UnityEngine;
using Cursor = UnityEngine.Cursor;

public class GameManager : Singleton<GameManager>
{
    private new void Awake()
    {
        base.Awake(); // Ensure Singleton initialization if needed.
        Application.targetFrameRate = 30;
    }
    [InspectorButton]
    public void SetFishs(int value)
    {
        Fishs = value;
    }
    [InspectorButton]
    public void SetKeys(int value)
    {
        Keys = value;
    }
    public int Fishs
    {
        get
        {
            return SaveData.Instance.Fishs;
        }
        set
        {
            SaveData.Instance.Fishs = value;
        }
    }
    public int Keys
    {
        get
        {
            return SaveData.Instance.Keys;
        }
        set
        {
            SaveData.Instance.Keys = value;
        }
    }
}
