using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

// Allows showing debugging info from each script on the current player
public class Player_Debug : MonoBehaviour
{
    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################

    private GUIStyle Style = null; // GUI styling
    private bool Visible = false; // GUI visibility
    private List<int> FPS = new List<int>(); // Arraylist for storing past FPS
    private int FPS_average; // Average FPS
    private HashSet<KeyCode> Keycodes = new HashSet<KeyCode>(); // List for recently used keycodes

    private void Start()
    {
        StartCoroutine(Show_FPS(0.3f));
        StartCoroutine(Clear_Keycodes(1f));
    }

    private void Update () => Get_FPS(); // Updating FPS 


    //##########################################################################################
    //###############################  PUBLIC BINDABLE METHODS  ################################
    //##########################################################################################

    [Bindable("Toggles debugging GUI visibility")] 
    public bool Toggle ()
    {
        Visible = !Visible;
        return true;
    }

    [Bindable("Show debugging GUI")] 
    public bool Show ()
    {
        if (Visible)
            return false;
        else
            Visible = true;
        return true;
    }

    [Bindable("Hiding debugging GUI")]
    public bool Hide ()
    {
        if (!Visible)
            return false;
        else
            Visible = false;
        return true;
    }


    //##########################################################################################
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################

    // Regularly clearing used keycodes
    private IEnumerator Clear_Keycodes (float interval)
    {
        while(true)
        {
            yield return new WaitForSeconds(interval);
            Keycodes.Clear();
        }
    }

    // Getting FPS
    private void Get_FPS () {
        FPS.Add((int)(1f / Time.deltaTime));
        if(FPS.Count > 15)
            FPS.RemoveAt(0);
    }

    // Showing FPS once in a while
    private IEnumerator Show_FPS (float interval)
    {
        while(true)
        {
            yield return new WaitForSeconds(interval);
            FPS_average = ((FPS.Count > 0) ? (int)Math.Round(FPS.Average()) : 0);
        }
    }

    // Showing all debugging info
    private void OnGUI()
    {
        // Setting up GUI styling
        if(Style == null)
        {
            Style = new GUIStyle(GUI.skin.box);
            Style.alignment = TextAnchor.UpperLeft;
        }
        if(Visible)
        {
            // Adding keycodes to hashset
            Event e = Event.current;
            if (e.isKey)
                Keycodes.Add(e.keyCode);
            else if(e.isMouse)
            {
                Console.Warning(this, e.button.ToString());
                KeyCode mouseButton;
                if (Enum.TryParse("Mouse" + e.button.ToString(), out mouseButton))
                    Keycodes.Add(mouseButton);
            }
            // Generating GUI text
            string debug = "";
            var scripts = transform.GetComponents<MonoBehaviour>(); // Loading all script components on the current character
            foreach(var script in scripts) // Looping through each script in array
                if(script.GetType().GetMethod("ToString").DeclaringType == script.GetType()) // Checking if script has overridden ToString method
                    debug += script.ToString();
            // Showing GUI
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), debug, Style); // Shows GUI if "visible" is true
        }
    }


    //##########################################################################################
    //##################################  DEBUGGING TOSTRING  ##################################
    //##########################################################################################

    // GETTING INFORMATION ABOUT CLIENT
    public override string ToString () 
    {
        string output = "";
        output += "Screen: ↔ " + Screen.width.ToString() + " x " + Screen.height.ToString() + " px\n";
        output += "Average FPS: " + FPS_average.ToString() + "\n";
        output += "Game time: " + Math.Round(Time.time).ToString() + " s\n";
        output += "Key events (" + Keycodes.Count.ToString() + "): ";
        foreach(KeyCode keycode in Keycodes)
            output += keycode.ToString() + " (" + ((int)keycode).ToString() + "), ";
        output += "\n";
        return output;
    }
    
}
