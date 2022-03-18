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
        foreach(Sound s in Sounds)
        {
            s.Source = transform.gameObject.AddComponent<AudioSource>();
            s.Source.clip = s.Clip;
            Set_Volume(s);
            s.Source.pitch = s.Pitch;
            s.Source.loop = s.Loop;
            s.Source.spatialBlend = s.Spatial;
            s.Source.playOnAwake = s.Autoplay;
            if(s.Autoplay)
                s.Source.Play();
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

    // Stop sound
    public void Stop (string name)
    {
        Sound s = Get_Sound(name);
        if(s != null)
            s.Source.Stop();
    }

    // Setting current volume
    private void LateUpdate ()
    {
        foreach(Sound sound in Sounds)
            Set_Volume(sound);
    }

    // Setting volume
    private void Set_Volume (Sound sound)
    {
        Setting setting = Config_Loader.Get("Audio", (Is_Menu ? "Menu_Sound" : "Game_Sound"));
        sound.Source.volume = float.Parse(setting.Value) * sound.Volume;
    }

}
