using UnityEngine;
using UnityEngine.UI;

// Shows slider value in a label
public class Menu_Slider : MonoBehaviour
{
    //##########################################################################################
    //###################################### SCRIPT FLOW #######################################
    //##########################################################################################
    
    // Showing slider value after edit
    private void Start () 
    {
        Show();
        GetComponent<Slider>().onValueChanged.AddListener(delegate { 
            Show();
        });
    }

    // Showing slider value
    public void Show () => transform.GetChild(3).GetComponent<Text>().text = GetComponent<Slider>().value.ToString("0.00");

}
