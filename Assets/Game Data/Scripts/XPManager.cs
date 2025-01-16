using UnityEngine;

public class XPManager : MonoBehaviour
{
    public static XPManager Instance;

    public int totalXP;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddXP(int xp)
    {
        totalXP += xp;
        Debug.Log($"Total XP: {totalXP}");
    }
}