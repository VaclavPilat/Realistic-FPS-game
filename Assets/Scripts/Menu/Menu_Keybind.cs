using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

// Loading keybinds
public class Menu_Keybind : MonoBehaviour
{
    //##########################################################################################
    //######################################  COMPONENTS  ######################################
    //##########################################################################################

    private Menu_Variables Menu_Variables;
    private void Awake () => Menu_Variables = transform.root.GetComponent<Menu_Variables>();

    [SerializeField] private Text Name;
    [SerializeField] private Text Key;
    [SerializeField] private Text Down;
    [SerializeField] private Text Hold;
    [SerializeField] private Text Up;


    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################

    private bool Waiting = false; // Waiting for key


    //##########################################################################################
    //###################################### SCRIPT FLOW #######################################
    //##########################################################################################

    // Setting background color
    private void Update ()
    {
        // Input fields
        foreach(Transform input in transform)
        {
            InputField inputfield = input.GetComponent<InputField>();
            if(inputfield)
            {
                if(inputfield.isFocused)
                    Set_Focus_Appearance(input);
                else
                    Unset_Focus_Appearance(input);
            }
        }
        // Button
        if(Waiting)
            Set_Focus_Appearance(Key.transform.parent);
        else
            Unset_Focus_Appearance(Key.transform.parent);
    }

    // Setting focus appearance
    private void Set_Focus_Appearance (Transform input) 
    {
        Color color = new Color(1f, 1f, 1f, 0.5f);
        Image image = input.GetComponent<Image>();
        if(image.color != color)
            image.color = color;
    }

    // Unsetting focus appearance
    private void Unset_Focus_Appearance (Transform input) 
    {
        Color color = Color.clear;
        Image image = input.GetComponent<Image>();
        if(image.color != color)
            image.color = color;
    }

    // Editing keycode
    public void Edit_Keycode () => Waiting = !Waiting;

    // Detecting pressed keys
    private void OnGUI()
    {
        if(Waiting)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                Set_Keycode(e.keyCode);
                Waiting = false;
            }
        }
    }

    // Setting keycode
    private void Set_Keycode (KeyCode key)
    {
        Key.text = key.ToString();
        Update_Keybind();
    }

    // Updating edited keybind
    public void Update_Keybind ()
    {
        if(Menu_Variables.Visible)
        {
            int index = transform.GetSiblingIndex();
            // Setting keybind values
            Keybind bind = Menu_Variables.Keybinds[index];
            bind.Name = Name.text;
            bind.KeyCode = (KeyCode) System.Enum.Parse(typeof(KeyCode), Key.text);
            bind.KeyDown = Down.text;
            bind.Key = Hold.text;
            bind.KeyUp = Up.text;
            // Saving keybinds
            Save_Keybinds();
        }
    }

    // Saving keybinds
    private void Save_Keybinds ()
    {
        Console.Log(this, "Saving keybinds...");
    }

}
