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
    
    private void Awake () => Resize();

    // Resizing element based on its children
    public void Resize ()
    {
        // Setting sizes of children object based on parent size
        var grid = transform.GetComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(
            (transform.parent.GetComponent<RectTransform>().rect.width 
                - (grid.spacing.x * (transform.childCount -1))
                - grid.padding.left
                - grid.padding.right
                ) / transform.childCount, 
            grid.cellSize.y
        );
    }

}
