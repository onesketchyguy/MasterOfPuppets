using UnityEngine;

public class PlayerPrefsManager
{
    private const string SFX_Volume = "SFX_VOLUME";

    private const string Music_Volume = "MUSIC_VOLUME";

    private const string Master_Volume = "MASTER_VOLUME";

    private const string Health_Bars = "HEALTHBARS_ABOVEENEMIES";

    private const string Hard_Mode_Locked = "HARDMODE_LOCKED";

    private const string Hard_Mode_Active = "HARDMODE_ACTIVE";

    public static void ResetValues()
    {
        MasterVolume = 1;
        MusicVolume = 0.5f;
        SFXVolume = 1;
    }

    public static bool HardModeUnlocked
    {
        get
        {
            return PlayerPrefs.GetInt(Hard_Mode_Locked, 0) == 1;
        }
        set
        {
            PlayerPrefs.SetInt(Hard_Mode_Locked, value == true ? 1 : 0);
        }
    }

    public static bool HardModeActive
    {
        get
        {
            return PlayerPrefs.GetInt(Hard_Mode_Active, 0) == 1;
        }
        set
        {
            PlayerPrefs.SetInt(Hard_Mode_Active, value == true ? 1 : 0);
        }
    }

    public static int GoldCount
    {
        get
        {
            return PlayerPrefs.GetInt("GOLD_COUNT", 0);
        }
        set
        {
            PlayerPrefs.SetInt("GOLD_COUNT", value);
        }
    }

    /// <summary>
    /// Weather or not the user is using health bars above their enemies
    /// </summary>
    public static bool UsingHealthBars
    {
        get
        {
            return PlayerPrefs.GetInt(Health_Bars) == 0;
        }
        set
        {
            var val = (value == true ? 0 : 1);
            PlayerPrefs.SetInt(Health_Bars, val);
        }
    }

    public static float MasterVolume
    {
        get
        {
            return PlayerPrefs.GetFloat(Master_Volume);
        }
        set
        {
            PlayerPrefs.SetFloat(Master_Volume, value);
        }
    }

    public static float MusicVolume
    {
        get
        {
            return PlayerPrefs.GetFloat(Music_Volume) * MasterVolume;
        }
        set
        {
            PlayerPrefs.SetFloat(Music_Volume, value);
        }
    }

    public static float SFXVolume
    {
        get
        {
            return PlayerPrefs.GetFloat(SFX_Volume) * MasterVolume;
        }
        set
        {
            PlayerPrefs.SetFloat(SFX_Volume, value);
        }
    }

    public static bool FirstLaunch
    {
        get
        {
            return PlayerPrefs.GetInt("FIRST_LAUNCH") != 1;
        }
        set
        {
            if (value == false)
            {
                PlayerPrefs.SetInt("FIRST_LAUNCH", 1);
            }
            else
            {
                PlayerPrefs.SetInt("FIRST_LAUNCH", 0);
            }
        }
    }
}