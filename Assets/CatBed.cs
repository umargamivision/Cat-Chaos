using System.Collections;
using System.Collections.Generic;
using Ommy.SaveData;
using UnityEngine;

public class CatBed : MonoBehaviour
{
    public Material bedMat;
    public Color[] bedColor;
    private void Start() 
    {
        SetupBed();
    }
    public void SetupBed()
    {
        var SBType = SaveData.Instance.shopItemDatas.Find(f => f.hasSelected == true);
        bedMat.color = bedColor[(int)SBType.shopItemType];
    }
}
