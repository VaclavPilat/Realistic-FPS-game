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
                {
                    Set_Focus_Appearance(input);
                    // Stopping waiting for keys
                    foreach(Transform keybind in transform.parent)
                        keybind.GetComponent<Menu_Keybind>().Waiting = false;
                }
                else
                    Unset_Focus_Appearance(input);
            }
        }
        // Button
        if(Key.text == "None" && !Waiting)
            Set_Error_Appearance(Key.transform.parent);
        else
        {
            if(Waiting)
                Set_Focus_Appearance(Key.transform.parent);
            else
                Unset_Focus_Appearance(Key.transform.parent);
        }
    }

    // Setting focus appearance
    private void Set_Focus_Appearance (Transform input) => Change_Background(input, new Color(1f, 1f, 1f, 0.5f));

    // Setting error appearance
    private void Set_Error_Appearance (Transform input) => Change_Background(input, new Color(1f, 0f, 0f, 0.5f));

    // Unsetting focus appearance
    private void Unset_Focus_Appearance (Transform input) => Change_Background(input, Color.clear);

    // Changing backgroun color (id not set already)
    private void Change_Background (Transform input, Color color)
    {
        Image image = input.GetComponent<Image>();
        if(image.color != color)
            image.color = color;
    }

    // Editing keycode
    public void Edit_Keycode () 
    {
        foreach(Transform keybind in transform.parent)
            if(keybind != transform)
                keybind.GetComponent<Menu_Keybind>().Waiting = false;
        Waiting = !Waiting;
    }

    // Detecting pressed keys
    private void OnGUI()
    {
        if(Waiting)
        {
            Event e = Event.current;
            // Catching keyboard event
            if (e.isKey)
            {
                Set_Keycode(e.keyCode);
                Waiting = false;
            }
            else if(e.isMouse)
            // Catching mouse event
            {
                KeyCode mouseButton;
                if (Enum.TryParse("Mouse" + e.button.ToString(), out mouseButton))
                {
                    Set_Keycode(mouseButton);
                    Waiting = false;
                }
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
    private void Save_Keybinds () => Menu_Variables.Save_Keybinds();

}
