using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Keybind
{
    public string Name; // Name of the keybind
    public KeyCode KeyCode; // KeyCode of used key
    public string KeyDown; // GetKeyDown actions
    public string Key; // GetKey actions
    public string KeyUp; // GetKeyUp actions

    // TOSTRING
    public override string ToString ()
    {
        return "Name: " + ((Name != null) ? Name : "null") + "; KeyCode: " + KeyCode.ToString() + "; KeyDown: " + ((KeyDown != null) ? KeyDown : "null") + "; Key: " + ((Key != null) ? Key : "null") + "; KeyUp: " + ((KeyUp != null) ? KeyUp : "null");
    }

}
