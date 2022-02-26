using UnityEngine;
using UnityEngine.UI;

// Showing game version
public class Menu_Version : MonoBehaviour
{
    //##########################################################################################
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################

    // Printing out current version of the game
    private void Awake ()
    {
        Text label = GetComponent<Text>();
        label.text = "Version " + Application.version;
    }

}
