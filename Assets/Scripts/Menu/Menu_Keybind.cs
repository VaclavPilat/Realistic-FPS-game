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

    private Transform Name;
    private Transform Key;
    private Transform Down;
    private Transform Hold;
    private Transform Up;

    private void Awake () 
    {
        Menu_Variables = transform.root.GetComponent<Menu_Variables>();
        // Setting text components
        Name = transform.GetChild(0);
        Key = transform.GetChild(1);
        Down = transform.GetChild(2);
        Hold = transform.GetChild(3);
        Up = transform.GetChild(4);
        // Adding onedit actions
        Name.GetComponent<InputField>().onEndEdit.AddListener(delegate{Update_Keybind();});
        Key.GetComponent<Button>().onClick.AddListener(Edit_Keycode);
        Down.GetComponent<InputField>().onEndEdit.AddListener(delegate{Update_Keybind();});
        Hold.GetComponent<InputField>().onEndEdit.AddListener(delegate{Update_Keybind();});
        Up.GetComponent<InputField>().onEndEdit.AddListener(delegate{Update_Keybind();});

    }


    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################

    private bool Waiting = false; // Waiting for key
    private Color Focus_Color = new Color(1f, 1f, 1f, 0.5f);
    private Color Error_Color = new Color(1f, 0f, 0f, 0.5f);
    private Color Default_Color = new Color(1f, 1f, 1f, 0.2f);


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
        if(Key.GetChild(0).GetComponent<Text>().text == "None" && !Waiting)
            Set_Error_Appearance(Key);
        else
        {
            if(Waiting)
                Set_Focus_Appearance(Key);
            else
                Unset_Focus_Appearance(Key);
        }
    }

    // Setting focus appearance
    private void Set_Focus_Appearance (Transform input) => Change_Background(input, Focus_Color);

    // Setting error appearance
    private void Set_Error_Appearance (Transform input) => Change_Background(input, Error_Color);

    // Unsetting focus appearance
    private void Unset_Focus_Appearance (Transform input) => Change_Background(input, Default_Color);

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
        Key.GetChild(0).GetComponent<Text>().text = key.ToString();
        Update_Keybind();
    }

    // Updating edited keybind
    public void Update_Keybind ()
    {
        if(Menu_Variables.Visible)
        {
            int index = transform.GetSiblingIndex();
            // Setting keybind values
            Keybind bind = new Keybind();
            bind.Name = Name.GetComponent<InputField>().text;
            bind.KeyCode = (KeyCode) System.Enum.Parse(typeof(KeyCode), Key.GetChild(0).GetComponent<Text>().text);
            bind.KeyDown = Down.GetComponent<InputField>().text;
            bind.Key = Hold.GetComponent<InputField>().text;
            bind.KeyUp = Up.GetComponent<InputField>().text;
            // Saving keybinds
            Config_Loader.Config["Keybinds"][index] = bind.ToSetting();
            Config_Loader.Save("Keybinds");
        }
    }

}
