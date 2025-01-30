using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GVRemoteConfig : MonoBehaviour
{

    public static bool Use_AppOpen_Ad  = true;

    public void Fetch_Use_AppOpen_Ad_Value(GVAnalysisManager.FirebaseRemoteData data)
    {
        Use_AppOpen_Ad = data.DefaultValue_Boolean;
    }
}
