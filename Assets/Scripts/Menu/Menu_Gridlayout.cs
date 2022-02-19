using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Manages behaviour of a grid layout 
public class Menu_Gridlayout : MonoBehaviour
{
    //##########################################################################################
    //###################################### SCRIPT FLOW #######################################
    //##########################################################################################
    
    private void Awake ()
    {
        // Setting sizes of children object based on parent size
        transform.GetComponent<GridLayoutGroup>().cellSize = new Vector2(
            (transform.GetComponent<RectTransform>().rect.width - (transform.GetComponent<GridLayoutGroup>().spacing.x * (transform.childCount -1))) / transform.childCount, 
            transform.GetComponent<RectTransform>().rect.height
        );
    }

}
