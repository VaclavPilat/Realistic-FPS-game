using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Filtering secondary buttons based on searched text
public class Menu_Search : MonoBehaviour
{
    //##########################################################################################
    //####################################### COMPONENTS #######################################
    //##########################################################################################

    private InputField InputField;
    private void Awake () => InputField = GetComponent<InputField>();


    //##########################################################################################
    //###################################### SCRIPT FLOW #######################################
    //##########################################################################################
    
    // Filtering buttons
    public void Filter_Buttons ()
    {
        string text = InputField.text.ToLower();
        // Filtering by text
        foreach(Transform button in transform.parent.parent.GetChild(1).GetChild(0).GetChild(0))
        {
            if(button.GetChild(0).GetComponent<Text>().text.ToLower().Contains(text))
                button.gameObject.SetActive(true);
            else
                button.gameObject.SetActive(false);
        }
    }


}
