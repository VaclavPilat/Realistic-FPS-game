using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

// Keybind
[Serializable]
public class Keybind
{
    public string Name; // Name of the keybind
    public KeyCode KeyCode; // KeyCode of used key
    public string KeyDown; // GetKeyDown actions
    public string Key; // GetKey actions
    public string KeyUp; // GetKeyUp actions

    public Setting ToSetting ()
    {
        Setting s = new Setting();
        s.Name = ((int) KeyCode).ToString();
        s.Description = Name;
        s.Type = "keybind";
        s.Check = null;
        s.Value = KeyDown + "|" + Key + "|" + KeyUp;
        return s;
    }
}

// Information about maps
public class Map_Info
{
    public string Description; // Map decription
    public string Size; // Map size
    public Game_Mode[] Modes; // Game modes compatible with this map
}

// Sound
[Serializable]
public class Sound
{
    public string Name; // Sound name
    public AudioClip Clip; // Audio clip
    public float Volume = 1f; // Default volume
    public float Pitch = 1f; // Default pitch
    public bool Loop = false; // Enable looping?
    public float Spatial = 1f; // 0=2D; 1=3D
    public bool Autoplay = false; // Play automatically (on Awake) ?
    [HideInInspector] public AudioSource Source; // Audio source component
}

// Setting
[Serializable]
public class Setting
{
    public string Name; // Setting name, used internally
    public string Description; // Setting description, shown to user
    public string Type; // Setting type
    public string Check; // Setting rules for checking validity
    public string Value; // Setting value, stored as an object

    public Keybind ToKeybind ()
    {
        Keybind k = new Keybind();
        k.Name = Description;
        k.KeyCode = (KeyCode) (int.Parse(Name));
        string[] actions = Value.Split('|');
        k.KeyDown = actions[0];
        k.Key = actions[1];
        k.KeyUp = actions[2];
        return k;
    }
}