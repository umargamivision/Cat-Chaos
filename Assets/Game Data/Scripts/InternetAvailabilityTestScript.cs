using UnityEngine;
using EasyMobile;

public class InternetAvailabilityTestScript : MonoBehaviour
{
    public static InternetAvailabilityTestScript instance;

    private NativeUI.AlertPopup alert;
    private bool isCheckingInternet = false; // Prevent multiple alerts

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object alive across scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(CheckConnection), 0f, 1f); // Check internet every 1 second
    }

    public void CheckConnection()
    {
        NetworkReachability reachability = Application.internetReachability;
        switch (reachability)
        {
            case NetworkReachability.NotReachable:
                Debug.Log("No internet connection");

                if (!isCheckingInternet) // Prevent multiple alerts
                {
                    isCheckingInternet = true;
                    ShowNativeAlert();
                }
                break;

            case NetworkReachability.ReachableViaCarrierDataNetwork:
            case NetworkReachability.ReachableViaLocalAreaNetwork:
                Debug.Log("Internet available");
                isCheckingInternet = false;
                break;
        }
    }

    private void ShowNativeAlert()
    {
        alert = NativeUI.Alert("No Internet Connection", "Check Your Internet Connection And Try Again.");
        if (alert != null)
            alert.OnComplete += OnAlertCompleteHandler;
    }

    private void OnAlertCompleteHandler(int obj)
    {
        if (alert != null)
            alert.OnComplete -= OnAlertCompleteHandler;

        isCheckingInternet = false; // Allow rechecking
    }
}