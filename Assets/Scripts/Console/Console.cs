using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

// Class for getting additional info about scene, object and script when calling debug messages
// Methods in this class are called like this: Console.Error(this, "message");
public class Console 
{
    //##########################################################################################
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################

    // Gets a prefix -- current scene name, object name and script name
    private static string Prefix (MonoBehaviour script) 
    {
        return SceneManager.GetActiveScene().name + " → " + script.transform.name + " → " + script.GetType().Name + " ::: ";
    }

    // Calling debug methods with added prefix
    public static void Log      (MonoBehaviour script, string message) => Debug.Log         (Prefix(script) + message);
    public static void Warning  (MonoBehaviour script, string message) => Debug.LogWarning  (Prefix(script) + message);
    public static void Error    (MonoBehaviour script, string message) => Debug.LogError    (Prefix(script) + message);
}
