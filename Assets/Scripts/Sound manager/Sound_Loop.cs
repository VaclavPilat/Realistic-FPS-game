using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

// Sound queue for playing each sound in the list after one another
public class Sound_Loop : Sound_Manager
{
    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################

    private AudioSource Source; // A singular audio source for playing all sounds in the list


    //##########################################################################################
    //###################################### SCRIPT FLOW #######################################
    //##########################################################################################

    // Adding a single AudioSource component
    private void Awake () => Source = gameObject.AddComponent<AudioSource>();

    // Playing on start
    private void Start () => StartCoroutine(Play());

    // Playing all sounds in loop
    private IEnumerator Play ()
    {
        int i = 0;
        while(Sounds.Length > 0)
        {
            Console.Log(this, Sounds[i].Name);
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
