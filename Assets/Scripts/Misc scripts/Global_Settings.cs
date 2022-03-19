using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
        StartCoroutine(Repeat(1f));
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
        // Setting graphics quality
        Setting setting = Config_Loader.Get("Graphics", "Quality");
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
    }

}
