using UnityEngine;
using UnityEngine.UI;
using System;

// Binding sounds to input elements
public class Menu_Sound : MonoBehaviour
{
    //##########################################################################################
    //######################################  COMPONENTS  ######################################
    //##########################################################################################

    private Button Button = null; // Button component
    private InputField InputField = null; // InputField component
    private Sound_Manager Sound_Manager; // Sound manager


    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################

    private bool InputField_Enabled = false; // Enabling sounds for input fields 


    //##########################################################################################
    //##################################  PUBLIC VARIABLES  ####################################
    //##########################################################################################

    public string Sound; // Sound name


    //##########################################################################################
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################

    // Enabling sounds
    private void Awake ()
    {
        // Setting up sound manager
        Sound_Manager = transform.root.GetComponent<Sound_Manager>();
        // Getting Button component
        Button = GetComponent<Button>(); 
        if(Button != null)
            Button.onClick.AddListener(Onclick_Sound);
        // Getting InputField component
        InputField = GetComponent<InputField>();
    }

    // Enabling sounds for InputField
    private void Update ()
    {
        if(InputField != null)
        {
            if(!InputField_Enabled && InputField.isFocused)
                Onclick_Sound();
            InputField_Enabled = InputField.isFocused;
        }
    }

    // Making sound on click
    protected void Onclick_Sound () => Sound_Manager.Play(Sound);

}
