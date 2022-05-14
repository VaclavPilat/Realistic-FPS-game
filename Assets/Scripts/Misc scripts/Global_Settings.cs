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
    }

}
