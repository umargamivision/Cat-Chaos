using UnityEngine;
public static class VibrationManager
{
#if UNITY_ANDROID && !UNITY_EDITOR
    private static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    private static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    private static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
    private static AndroidJavaClass vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");
#else
    private static AndroidJavaClass unityPlayer;
    private static AndroidJavaObject currentActivity;
    private static AndroidJavaObject vibrator;
    private static AndroidJavaClass vibrationEffectClass;
#endif

    public static void Vibrate()
    {
        if (isAndroid())
        {
            if (IsVibrationEffectSupported())
            {
                AndroidJavaObject vibrationEffect = vibrationEffectClass.CallStatic<AndroidJavaObject>("createOneShot", 100, VibrationEffectDefaultAmplitude());
                vibrator.Call("vibrate", vibrationEffect);
            }
            else
            {
                vibrator.Call("vibrate", 100); // Fallback for older devices
            }
        }
        else
        {
            Handheld.Vibrate();
        }
    }

    public static void Vibrate(long milliseconds)
    {
        if (isAndroid())
        {
            if (IsVibrationEffectSupported())
            {
                AndroidJavaObject vibrationEffect = vibrationEffectClass.CallStatic<AndroidJavaObject>("createOneShot", milliseconds, VibrationEffectDefaultAmplitude());
                vibrator.Call("vibrate", vibrationEffect);
            }
            else
            {
                vibrator.Call("vibrate", milliseconds); // Fallback for older devices
            }
        }
        else
        {
            Handheld.Vibrate();
        }
    }

    public static void Vibrate(long[] pattern, int repeat)
    {
        if (isAndroid())
        {
            if (IsVibrationEffectSupported())
            {
                AndroidJavaObject vibrationEffect = vibrationEffectClass.CallStatic<AndroidJavaObject>("createWaveform", pattern, repeat);
                vibrator.Call("vibrate", vibrationEffect);
            }
            else
            {
                vibrator.Call("vibrate", pattern, repeat); // Fallback for older devices
            }
        }
        else
        {
            Handheld.Vibrate();
        }
    }

    public static void Cancel()
    {
        if (isAndroid())
        {
            vibrator.Call("cancel");
        }
    }

    private static bool isAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return true;
#else
        return false;
#endif
    }

    private static bool IsVibrationEffectSupported()
    {
        return vibrationEffectClass != null && AndroidDeviceAPIVersion() >= 26;
    }

    private static int AndroidDeviceAPIVersion()
    {
        using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
        {
            return version.GetStatic<int>("SDK_INT");
        }
    }

    private static int VibrationEffectDefaultAmplitude()
    {
        return vibrationEffectClass.GetStatic<int>("DEFAULT_AMPLITUDE");
    }
}