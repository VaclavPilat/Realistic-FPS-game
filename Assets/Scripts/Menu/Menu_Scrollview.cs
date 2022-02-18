using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Manages behaviour of a scrollview 
public class Menu_Scrollview : MonoBehaviour
{
    //##########################################################################################
    //###################################### SCRIPT FLOW #######################################
    //##########################################################################################
    
    private void Awake ()
    {
        // Setting height based on contents
        float total = 0f;
        foreach(Transform child in transform)
        {
            total += child.GetComponent<RectTransform>().rect.height;
        }
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, total);
    }

}
