using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

// https://www.youtube.com/watch?v=6OT43pvUyfY
// Sound manager
public class Sound_Manager : MonoBehaviour
{
    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################

    [SerializeField] private Sound[] Sounds; // List of sounds


    //##########################################################################################
    //###################################### SCRIPT FLOW #######################################
    //##########################################################################################

    // Applying sound properties to newly created AudioSource components
    private void Awake ()
    {
        foreach(Sound s in Sounds)
        {
            s.Source = transform.gameObject.AddComponent<AudioSource>();
            s.Source.clip = s.Clip;
            s.Source.volume = s.Volume;
            s.Source.pitch = s.Pitch;
            s.Source.loop = s.Loop;
            s.Source.spatialBlend = s.Spatial;
            s.Source.playOnAwake = s.Autoplay;
        }
    }

    // Finding sound by name
    private Sound Get_Sound (string name)
    {
        Sound s = Array.Find(Sounds, sound => sound.Name == name);
		if (s == null)
			Console.Warning(this, "Sound '" + name + "' not found!");
        return s;
    }

    // Play sound
    public void Play (string name)
    {
        Sound s = Get_Sound(name);
        if(s != null)
            s.Source.Play();
    }

}
