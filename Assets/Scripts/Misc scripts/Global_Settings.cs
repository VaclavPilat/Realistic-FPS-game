using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

// Script for updating global settings once in a while
public class Global_Settings : MonoBehaviour
{
    //##########################################################################################
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################
    
    // Loading necessary settings
    private void Start()
    {
        Config_Loader.Load("Graphics");
        // Starting a coroutine that periodically applies the global settings
        StartCoroutine(Repeat(0.5f));
    }

    // Applying global settings
    private IEnumerator Repeat (float period)
    {
        while(true)
        {
            // Waiting for a specified amount of time
            yield return new WaitForSecondsRealtime(period);
            Apply_Settings();
        }
    }

    // Applying settings
    private void Apply_Settings ()
    {
        Setting setting;
        // Setting graphics quality
        setting = Config_Loader.Get("Graphics", "Quality");
        if(setting != null)
        {
            int index = int.Parse(setting.Value);
            if(QualitySettings.GetQualityLevel() != index)
            {
                Console.Log(this, "Setting new graphics quality to " + index.ToString() + " = " + setting.Check.Split('|')[index]);
                QualitySettings.SetQualityLevel(index, true);
            }
        }
        else
            Console.Warning(this, "Graphics quality setting cannot be found");
        // Setting graphics quality
        setting = Config_Loader.Get("Graphics", "Fullscreen");
        if(setting != null)
        {
            bool value = bool.Parse(setting.Value);
            if(Screen.fullScreen != value)
            {
                Console.Log(this, "Setting fullscreen mode to " + value.ToString());
                Screen.fullScreen = value;
            }
        }
        else
            Console.Warning(this, "Fullscreen mode setting cannot be found");
        // Setting master volume
        setting = Config_Loader.Get("Audio", "Master");
        if(setting != null)
        {
            float value = float.Parse(setting.Value);
            if(AudioListener.volume != value)
            {
                Console.Log(this, "Setting master volume to " + value.ToString());
                AudioListener.volume = value;
            }
        }
        else
            Console.Warning(this, "Master volume setting cannot be found");
    }

}
