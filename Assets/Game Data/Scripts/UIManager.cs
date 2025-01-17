using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ommy.Singleton;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
public class UIManager : Singleton<UIManager>
{
    [Header("Panels")]
    public GameObject gamePlay;

    [Header("Texts")]
    public TMP_Text levelNoTxt;

    [Header("Images")]
    public Image levelFillImg;

    [Header("Buttons")]
    public GameObject grabButton;
    public void UpdateLevelBar(int levelNo, float fillAmount)
    {
        levelNoTxt.text = $"Level "+levelNo;
        levelFillImg.DOFillAmount(fillAmount,0.2f);
    }
}
