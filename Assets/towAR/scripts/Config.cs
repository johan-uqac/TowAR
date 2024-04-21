using UnityEngine;

public class Config : MonoBehaviour
{
    // Version code of the app
    public static string version_code = "v1.0.0";

    // Number of days to lock certain features
    public static int lock_days = 7;

    // Record of the user's performance
    public static int record = 0;

    // Status of AR recognition
    public static AR_statu ar_statu = AR_statu.recognizing;

    // Struct for user attributes (currently empty)
    private struct UserAttributes { }

    // Struct for app attributes (currently empty)
    private struct AppAttributes { }

    void Awake()
    {
        // Set the record from player preferences
        if (PlayerPrefs.GetInt("record") == 0)
        {
            // If no record is found, set it to 0
            Config.record = 0;
        }
        else
        {
            // Otherwise, retrieve the record from player preferences
            Config.record = PlayerPrefs.GetInt("record");
        }
    }
}

// Enumeration for AR recognition status
public enum AR_statu
{
    recognizing,           // Recognizing mode
    object_is_placed      // Object is placed mode
}
