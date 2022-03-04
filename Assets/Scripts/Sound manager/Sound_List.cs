using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

// List of sounds
public class Sound_List : MonoBehaviour
{
    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################

    [SerializeField] protected Sound[] Sounds; // List of sounds


    //##########################################################################################
    //##################################  PUBLIC VARIABLES  ####################################
    //##########################################################################################

    public bool Is_Menu = false; // Does the sound belong to menu?


    //##########################################################################################
    //###################################### SCRIPT FLOW #######################################
    //##########################################################################################

    // Loading audio settings
    protected void Awake () => Config_Loader.Load("Audio");

    // Setting master volume
    protected void Update () 
    {
        AudioListener.volume = float.Parse(Array.Find(Config_Loader.Config["Audio"], s => s.Name == "Master").Value);
    }

}
