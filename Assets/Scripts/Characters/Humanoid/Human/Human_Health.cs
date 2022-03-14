using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Tracks current health state
public class Human_Health : Lockable_Script
{
    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################

    private float Maximum_Blood = 5000f; // Maximum blood amount in milileters
    private float Blood = 0f; // Current amount of blood in milileters
    private float Bleeding = 0f; // Bleeding speed (ml/s)
    
    // List of colliders
    [SerializeField] private Collider[] Head; 
    [SerializeField] private Collider[] Left_Arm; 
    [SerializeField] private Collider[] Right_Arm; 
    [SerializeField] private Collider[] Chest;
    [SerializeField] private Collider[] Left_Leg;
    [SerializeField] private Collider[] Right_Leg;


    //##########################################################################################
    //##################################  PUBLIC VARIABLES  ####################################
    //##########################################################################################
    
    public bool Alive = true; // Is character alive?


    //##########################################################################################
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################

    private void Awake () 
    {
        // Setting blood volume
        Blood = Maximum_Blood;
    }

    // Updating health
    private void FixedUpdate () => Bleed();

    // Bleeding
    private void Bleed ()
    {
        if(Blood > 0f)
            Blood -= Bleeding * Time.fixedDeltaTime;
        else
        {
            Die();
            Blood = 0f;
            Bleeding = 0f;
        }
    }

    // Causing pain on a selected body part
    public void Cause_Pain (Collider collider, float amount)
    {

    }

    // Causing damage with pain on a selected body part
    public void Cause_Damage (Collider collider, float amount)
    {
        if(Array.IndexOf(Head, collider) > -1) // Hit in the head
        {
            Die();
        }
        Bleeding += amount;
    }

    // Dying
    private void Die ()
    {
        Alive = false;
        Console.Log(this, "DEAD");
        GameObject.Destroy(this);
    }


    //##########################################################################################
    //##################################  DEBUGGING TOSTRING  ##################################
    //##########################################################################################

    public override string ToString ()
    {
        string output = "";
        output += "Blood: " + Blood.ToString() + "/" + Maximum_Blood.ToString() + " ml (" + ((Blood / Maximum_Blood) * 100f).ToString() + "%)\n";
        output += "Bleeding: " + Bleeding.ToString() + " ml/s\n";
        return output;
    }

}
