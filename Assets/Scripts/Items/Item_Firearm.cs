using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Guns
public class Item_Firearm : Item
{
    //##########################################################################################
    //######################################  COMPONENTS  ######################################
    //##########################################################################################

    private Sound_Manager Sound_Manager; // Sound manager
    private GameObject Bullet_Spawn; // Place where a bullet spawns
    
    private void Awake () 
    {
        Sound_Manager = GetComponent<Sound_Manager>();
        Bullet_Spawn = transform.GetChild(0).gameObject;
    }


    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################

    private bool Bullet_In_Chamber; // Bullet in chamber?


    //##########################################################################################
    //##################################  PUBLIC VARIABLES  ####################################
    //##########################################################################################

    public GameObject Bullet; // Bullet prefab
    public float Bleeding; // Bleeding caused by a shot
    public float Gun_Failure; // Chance that an empty bullet shell causes the gun to stop working
    public float Ammo_Failure; // Chance that ammunition fails


    //##########################################################################################
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################

    // Shooting
    public override bool Primary ()
    {
        Sound_Manager.Play("Fire");
        return true;
    }

    // Aiming down sights
    public override bool Secondary () 
    {
        return true;
    }

    // Ejecting magazine from a gun and vice versa
    public override bool Eject () 
    {
        return true;
    }

    // 
    public override bool Reload () 
    {
        return true;
    }

    // Getting new magazine or stashing it
    public override bool Stash () 
    {
        return true;
    }


    //##########################################################################################
    //##################################  DEBUGGING TOSTRING  ##################################
    //##########################################################################################

    public override string SpecificToString ()
    {
        string output = "";
        output += "Bleeding caused by item: " + Bleeding.ToString() + " ml/s\n";
        output += "Gun failure chance: " + Math.Round(Gun_Failure*100, 2).ToString() + "%\n";
        output += "Ammo failure chance: " + Math.Round(Ammo_Failure*100, 2).ToString() + "%\n";
        output += "Bullet in chamber: " + Bullet_In_Chamber.ToString() + "\n";
        return output;
    }

}
