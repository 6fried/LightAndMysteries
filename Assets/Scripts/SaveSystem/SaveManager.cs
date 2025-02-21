using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private const string KEY = "Level";

    public static void SaveLevel(int value)
    {
        PlayerPrefs.SetInt(KEY, value);
        PlayerPrefs.Save();
    }

    public static int LoadLevel()
    {
        return PlayerPrefs.GetInt(KEY, 0);
    }

    public static void ResetLevel()
    {
        PlayerPrefs.DeleteKey(KEY);
    }
}
