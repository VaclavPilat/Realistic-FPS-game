using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

// Sound queue for playing each sound in the list after one another
public class Sound_Loop : Sound_List
{
    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################

    private AudioSource Source; // A singular audio source for playing all sounds in the list
    private IEnumerator Coroutine; // Stored coroutine


    //##########################################################################################
    //##################################  PUBLIC VARIABLES  ####################################
    //##########################################################################################
    
    public bool Autoplay = true; // Should it start automatically?
    public bool Playing = false; // Is the loop playing?


    //##########################################################################################
    //###################################### SCRIPT FLOW #######################################
    //##########################################################################################

    // Adding a single AudioSource component
    private void Awake () 
    {
        Source = gameObject.AddComponent<AudioSource>();
        Coroutine = Loop();
    }

    // Playing on start
    private void Start () 
    {
        if(Autoplay)
            Play();
    }

    // Starting loop coroutine
    public void Play () 
    {
        Playing = true;
        Console.Log(this, "Starting a sound loop with " + Sounds.Length + " tracks");
        StartCoroutine(Coroutine);
    }

    // Stopping loop coroutine
    public void Stop ()
    {
        Playing = false;
        Console.Log(this, "Stopped a sound loop with " + Sounds.Length + " tracks");
        StopCoroutine(Coroutine);
        Source.Stop();
    }

    // Playing all sounds in loop
    private IEnumerator Loop ()
    {
        int i = 0;
        while(Sounds.Length > 0)
        {
            Console.Log(this, "Current sound track in loop: " + Sounds[i].Name);
            // Setting sound properties
            Source.clip = Sounds[i].Clip;
            Source.volume = Sounds[i].Volume;
            Source.pitch = Sounds[i].Pitch;
            Source.loop = false;
            Source.spatialBlend = Sounds[i].Spatial;
            Source.playOnAwake = false;
            // Playing the sound
            Source.Play();
            yield return new WaitForSeconds(Source.clip.length);
            if(i == Sounds.Length -1)
                i = 0;
            else
                i++;
        }
    }

}
