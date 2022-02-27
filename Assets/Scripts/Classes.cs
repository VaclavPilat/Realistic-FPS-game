using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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