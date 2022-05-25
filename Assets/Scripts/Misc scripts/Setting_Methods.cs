using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

// Class for storing methods called in settings
public static class Setting_Methods 
{

    // Onchange action for audio master volume setting
    public static void Master_Onchange (Setting setting) => AudioListener.volume = float.Parse(setting.Value);

    // Check action for graphics quality setting
    public static string Quality_Check ()
    {
        return string.Join("|", QualitySettings.names);
    }

    // Onchange action for graphics quality setting
    public static void Quality_Onchange (Setting setting) => QualitySettings.SetQualityLevel(int.Parse(setting.Value), true);

    // Onchange action for fullscreen setting
    public static void Fullscreen_Onchange (Setting setting) => Screen.fullScreen = bool.Parse(setting.Value);

    // Check action for graphics resolution setting
    public static string Resolution_Check () 
    {
        return string.Join("|", Screen.resolutions.Select(x => x.ToString()).ToArray());
    }

    // Onchange action for graphics resolution setting
    public static void Resolution_Onchange (Setting setting)
    {
        string[] resolution = setting.Check.Split('|')[int.Parse(setting.Value)].Replace(" ", string.Empty).Replace("Hz", string.Empty).Split(new Char[]{'x', '@'});
        Screen.SetResolution(int.Parse(resolution[0]), int.Parse(resolution[1]), bool.Parse(Config_Loader.Get("Graphics", "Fullscreen").Value), int.Parse(resolution[2]));
    }

}