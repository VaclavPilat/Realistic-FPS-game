using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Target_Counter : MonoBehaviour
{
    //##########################################################################################
    //######################################  COMPONENTS  ######################################
    //##########################################################################################

    [SerializeField] private Text Count;
    [SerializeField] private Text Timer;


    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################

    private int Total = -1; // Total number of paper targets
    private int Destroyed = -1; // Number of destroyed paper targets
    private float Time_Since = -1f; // Time since the start of the app
    private float Current_Time = -1f; // Time spent on this map



    //##########################################################################################
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################

    private void Awake () => StartCoroutine(Check_Targets());

    // Checking target count
    private IEnumerator Check_Targets()
    {
        while(true)
        {
            // Getting all paper targets in scene
            GameObject[] all_objects = FindObjectsOfType<GameObject>();
            List<GameObject> paper_targets = new List<GameObject>();
            foreach(GameObject gameobject in all_objects)
                if (gameobject.layer == 9)
                    paper_targets.Add(gameobject);
            // Setting total number of targets
            if(Total < 0)
                Total = paper_targets.Count;
            Destroyed = Total - paper_targets.Count;
            if(Destroyed != 0 && Time_Since < 0)
                Time_Since = Time.time;
            if(paper_targets.Count != 0 && Time_Since >= 0)
                Current_Time = Time.time - Time_Since;
            // Showing variables
            Count.text = Destroyed.ToString() + "/" + Total.ToString();
            Timer.text = (Current_Time < 0 ? "0.00" : Current_Time.ToString("0.00")) + "s";
            yield return new WaitForSeconds(0.1f);
        }
    }
}
