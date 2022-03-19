using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Tracks current health state
public class Human_Health : Lockable_Script
{
    //##########################################################################################
    //######################################  COMPONENTS  ######################################
    //##########################################################################################

    private Human_Ragdoll Human_Ragdoll; // Human Ragdoll script

    
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
        Human_Ragdoll = GetComponent<Human_Ragdoll>();
        // Setting blood volume
        Blood = Maximum_Blood;
    }

    // Updating health
    private void FixedUpdate () => Bleed();

    // Bleeding
    private void Bleed ()
    {
        if(Bleeding <= 0f)
            return;
        // Subtracting certain amount of blood each fixed frame
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
        if(!Alive)
            return;
    }

    // Causing damage with pain on a selected body part
    public void Cause_Damage (Collider collider, float amount)
    {
        if(!Alive)
            return;
        // Hit in the head = instant death
        if(Array.IndexOf(Head, collider) > -1)
            Die();
        // Hit in the limbs equals to loss of the limb
        if(Array.IndexOf(Left_Arm, collider) > -1)
            Human_Ragdoll.Enable_Some(Left_Arm);
        if(Array.IndexOf(Right_Arm, collider) > -1)
            Human_Ragdoll.Enable_Some(Right_Arm);
        if(Array.IndexOf(Left_Leg, collider) > -1)
            Human_Ragdoll.Enable_Some(Left_Leg);
        if(Array.IndexOf(Right_Leg, collider) > -1)
            Human_Ragdoll.Enable_Some(Right_Leg);
        // Taking damage
        Bleeding += amount;
    }

    // Dying
    private void Die ()
    {
        Alive = false;
        Console.Log(this, "DEAD");
        //Destroy(gameObject);
        Human_Ragdoll.Enable();
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
