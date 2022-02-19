using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

// Loading maps
public class Menu_Maps : MonoBehaviour
{
    //##########################################################################################
    //######################################  COMPONENTS  ######################################
    //##########################################################################################

    private Scene_Transition Scene_Transition; // Scene transition object

    private GameObject Buttons; // Gameobject that holds secondary buttons
    private GameObject Contents; // Gameobject that holds teriary panels

    public GameObject Button; // Seconary button prefab
    public GameObject Content; // Tertirary panel prefab


    //##########################################################################################
    //##################################  PUBLIC VARIABLES  ####################################
    //##########################################################################################

    public string Folder; // Name of a folder that contains desired scenes


    //##########################################################################################
    //###################################### SCRIPT FLOW #######################################
    //##########################################################################################

    private void Awake () 
    {
        Scene_Transition = GameObject.Find("Scene Transition").GetComponent<Scene_Transition>();
        Buttons = transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).gameObject;
        Contents = transform.GetChild(1).gameObject;
        Show_Maps( Get_Maps() );
    }
    
    // Gets list of scenes in a selected folder
    public List<string> Get_Maps ()
    {
        List<string> maplist = new List<string>();
        for(int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            if(SceneUtility.GetScenePathByBuildIndex(i).Contains(Folder + "/"))
            {
                // Getting name from scene
                string name = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
                maplist.Add(name);
            }
        return maplist;
    }

    // Adding UI elements
    public void Show_Maps (List<string> maplist)
    {
        foreach(string map in maplist)
        {
            // Adding secondary button
            GameObject map_button = Instantiate(Button, Buttons.transform);
            map_button.transform.name = map;
            map_button.transform.GetChild(0).GetComponent<Text>().text = map;
            // Adding tertiary panel with map info and settings
            GameObject map_content = Instantiate(Content, Contents.transform);
            map_content.transform.name = map;
            // Adding an action to a start button
            map_content.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(() => Scene_Transition.Load(map));
            // Loading map information from file
            var file = Resources.Load<TextAsset>("Maps/" + Folder + "/" + map);
            if(file != null)
            {
                string description = file.text;
                map_content.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = description;
            }
        }
    }

}
