using UnityEngine;
using UnityEngine.UI;
using System;

// Getting basic element properties
public class Menu_Button : MonoBehaviour
{
    //##########################################################################################
    //######################################  COMPONENTS  ######################################
    //##########################################################################################

    private Button Button = null; // Button component
    private Sound_Manager Sound_Manager; // Sound manager


    //##########################################################################################
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################

    public void Awake ()
    {
        Button = GetComponent<Button>(); // Getting Button component
        Sound_Manager = GetComponent<Sound_Manager>();
        Button.onClick.AddListener(Onclick_Sound);
    }

    // Making sound on click
    protected void Onclick_Sound () => Sound_Manager.Play("Click");

}
