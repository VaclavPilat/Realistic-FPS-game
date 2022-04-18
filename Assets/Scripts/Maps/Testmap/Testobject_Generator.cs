using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Testobject_Generator : MonoBehaviour
{
    //##########################################################################################
    //##################################  PUBLIC VARIABLES  ####################################
    //##########################################################################################

    public Testobject[] Objects; // List of generated prefabs
    public Transform Text; // Text object prefab


    //##########################################################################################
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################

    private void Awake()
    {
        // Looping through list of objects
        foreach (Testobject testObject in Objects)
            // Looping for object count
            for(int i = 0; i < testObject.Count; i++)
            {
                // Adding object itself
                var objectInstance = Instantiate(testObject.Prefab, testObject.Start + i * testObject.Position, testObject.Prefab.rotation);
                objectInstance.localScale += i * testObject.Scale;
                objectInstance.eulerAngles += i * testObject.Rotation;
                // Forcing update of transform variables
                Physics.SyncTransforms();
                // Adding text
                var textInstance = Instantiate(Text, testObject.Start + i * testObject.Position, Text.rotation);
                // Changing text
                string output = "";
                if (testObject.Scale != Vector3.zero)
                    output += objectInstance.GetComponent<Collider>().bounds.size.ToString() + " m\n";
                if (testObject.Rotation != Vector3.zero)
                    output += objectInstance.eulerAngles.ToString() + " °\n";
                textInstance.GetComponent<TextMesh>().text = output;
            }
    }
}
