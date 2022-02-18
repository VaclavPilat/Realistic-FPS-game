using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_Scene : MonoBehaviour
{
    //##########################################################################################
    //####################################### COMPONENTS #######################################
    //##########################################################################################

    // Scene transition for loading scenes
    private Scene_Transition Scene_Transition = null; 
    void Awake () 
    {
        var o = GameObject.Find("Scene Transition");
        if(o != null)
            Scene_Transition = o.GetComponent<Scene_Transition>();
        else
            Console.Error(this, "Cannot find gameobject for scene transition");
        if(Scene_Transition == null)
            Console.Error(this, "\"Scene_Transition\" is null");
    }

    //##########################################################################################
    //###################################### SCRIPT FLOW #######################################
    //##########################################################################################

    // Loading menu after X seconds
    void Start () => StartCoroutine(Load_Menu());
    private IEnumerator Load_Menu ()
    {
        yield return new WaitForSeconds(2);
        Scene_Transition.Load("Main");
    }
}
