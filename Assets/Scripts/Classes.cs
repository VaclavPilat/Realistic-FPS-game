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
    public string Value; // Setting value, stored as an object
}