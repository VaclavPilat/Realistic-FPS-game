using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Loading keybinds
public class Menu_Keybinds : MonoBehaviour
{
    //##########################################################################################
    //######################################  COMPONENTS  ######################################
    //##########################################################################################

    public GameObject Keybind; // Keybind UI prefab


    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################

    private Keybind[] Keybinds = null; // Array of keybinds
    private string Resource = "Config/Keybinds"; // Name of resource that stores keybinds


    //##########################################################################################
    //###################################### SCRIPT FLOW #######################################
    //##########################################################################################

    private void Awake () 
    {
        Load_Keybinds();
        Create_UI();
    }

    // Loading all keybinds
    private void Load_Keybinds ()
    {
        var file = Resources.Load<TextAsset>(Resource);
        if(file != null)
        {
            string json = file.text;
            Keybinds = JsonHelper.FromJson<Keybind>(json);
            Console.Log(this, "Found " + Keybinds.Length.ToString() + " keybinds in \"" + Resource + "\"");
        }
        else
            Console.Error(this, "Resource \"" + Resource + "\" doesn't exist");
    }

    // Generates UI for each keybind found
    private void Create_UI ()
    {
        if(Keybinds != null)
        {
            Vector2 size = new Vector2((transform.GetComponent<RectTransform>().rect.width - 60f) / Keybind.transform.childCount, Keybind.transform.GetComponent<RectTransform>().rect.height);
            foreach(Keybind keybind in Keybinds)
            {
                GameObject bind = Instantiate(Keybind, transform);
                // Setting UI
                bind.transform.GetChild(0).GetComponent<Text>().text = keybind.Name;
                bind.transform.GetChild(1).GetComponent<Text>().text = keybind.KeyCode.ToString();
                bind.transform.GetChild(2).GetComponent<Text>().text = keybind.KeyDown;
                bind.transform.GetChild(3).GetComponent<Text>().text = keybind.Key;
                bind.transform.GetChild(4).GetComponent<Text>().text = keybind.KeyUp;
                // Setting properties
                bind.GetComponent<GridLayoutGroup>().cellSize = size;
            }
        }
    }

}
