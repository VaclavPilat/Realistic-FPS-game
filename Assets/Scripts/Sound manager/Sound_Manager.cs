using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

// https://www.youtube.com/watch?v=6OT43pvUyfY
// Sound manager, used for sounds
public class Sound_Manager : Sound_List
{
    //##########################################################################################
    //###################################### SCRIPT FLOW #######################################
    //##########################################################################################

    // Applying sound properties to newly created AudioSource components
    private void Start ()
    {
        foreach(Sound sound in Sounds)
        {
            sound.Source = transform.gameObject.AddComponent<AudioSource>();
            sound.Source.clip = sound.Clip;
            sound.Source.pitch = sound.Pitch;
            sound.Source.loop = sound.Loop;
            sound.Source.spatialBlend = sound.Spatial;
            //sound.Source.playOnAwake = sound.Autoplay;
            if(sound.Autoplay)
                Play(sound.Name);
        }
    }

    // Play sound
    public void Play (string name)
    {
        Sound sound = Get_Sound(name);
        if(sound != null)
        {
            Setting setting = Config_Loader.Get("Audio", (Is_Menu ? "Menu_Sound" : "Game_Sound"));
            sound.Source.PlayOneShot(sound.Clip, float.Parse(setting.Value) * sound.Volume);
        }
    }

    // Finding sound by name
    private Sound Get_Sound (string name)
    {
        Sound sound = Array.Find(Sounds, sound => sound.Name == name);
		if (sound == null)
			Console.Warning(this, "Sound '" + name + "' not found!");
        return sound;
    }

}
