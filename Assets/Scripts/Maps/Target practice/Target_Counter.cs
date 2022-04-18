using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Target_Counter : MonoBehaviour
{
    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################

    private int Total = -1; // Total number of paper targets
    private int Destroyed = -1; // Number of destroyed paper targets


    //##########################################################################################
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################

    private void Start () => StartCoroutine(Check_Targets());

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
            // Showing variables
            Console.Warning(this, Destroyed.ToString() + "/" + Total.ToString());
            yield return new WaitForSeconds(0.25f);
        }
    }
}
