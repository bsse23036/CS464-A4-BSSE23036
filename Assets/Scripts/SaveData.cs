using UnityEngine;

public static class SaveData
{
    private const string UnlockedKey = "UnlockedLevel";
    private const string BestTimePrefix = "BestTime_Level_";

    public static int GetUnlockedLevel()
    {
        return PlayerPrefs.GetInt(UnlockedKey, 1);
    }

    public static void UnlockLevel(int levelBuildIndex)
    {
        int clamped = Mathf.Clamp(levelBuildIndex, 1, 5);
        if (clamped > GetUnlockedLevel())
        {
            PlayerPrefs.SetInt(UnlockedKey, clamped);
            PlayerPrefs.Save();
        }
    }

    public static float GetBestTime(int levelBuildIndex)
    {
        return PlayerPrefs.GetFloat(BestTimePrefix + levelBuildIndex, -1f);
    }

    public static bool RecordBestTime(int levelBuildIndex, float clearTime)
    {
        float previousBest = GetBestTime(levelBuildIndex);
        if (previousBest < 0f || clearTime < previousBest)
        {
            PlayerPrefs.SetFloat(BestTimePrefix + levelBuildIndex, clearTime);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }

    public static void ResetProgress()
    {
        PlayerPrefs.DeleteKey(UnlockedKey);
        for (int i = 1; i <= 5; i++)
        {
            PlayerPrefs.DeleteKey(BestTimePrefix + i);
        }
        PlayerPrefs.Save();
    }
}