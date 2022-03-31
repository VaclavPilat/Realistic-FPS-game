using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

// Guns
public class Item_Firearm : Item
{
    //##########################################################################################
    //######################################  COMPONENTS  ######################################
    //##########################################################################################

    private Sound_Manager Sound_Manager; // Sound manager
    public Transform Bullet_Spawn; // Place where a bullet spawns
    private Transform Bullet_Casing_Spawn; // Place where a bullet spawns
    
    private void Awake () 
    {
        Sound_Manager = GetComponent<Sound_Manager>();
        Bullet_Spawn = transform.GetChild(0);
        Bullet_Casing_Spawn = transform.GetChild(1);
    }

    public GameObject Bullet; // Bullet prefab
    public GameObject Bullet_Casing; // Bullet casing prefab


    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################

    private bool Ammo_In_Chamber = false; // Bullet in chamber?
    private bool Casing_In_Chamber = false; // Is an empty casing still in chanber?

    private bool Ammo_Failed = false; // Did the ammunition fail to shoot?
    private bool Casing_Stuck = false; // Is the bullet case stuck in the chamber, making the gun unable to shoot?

    private bool Trigger_Reset = true; // Is the trigger reset?
    private float Trigger_Pressed = 0f; // Time since the last trigger press


    //##########################################################################################
    //##################################  PUBLIC VARIABLES  ####################################
    //##########################################################################################

    public Firearm_Mode Mode; // Firearm firing mode
    public float Bleeding = 20f; // Bleeding caused by a shot
    public float Gun_Failure = 0.01f; // Chance that an empty bullet shell causes the gun to stop working
    public float Ammo_Failure = 0.01f; // Chance that ammunition fails
    public float Bullet_Speed = 100f;


    //##########################################################################################
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################

    // Resetting trigger for not-full-auto firearms
    private void FixedUpdate ()
    {
        if(transform.parent != null && Mode != Firearm_Mode.Auto)
        {
            // Chacking if there is a delay after the last time the trigger was pulled
            if(Time.fixedTime > (Trigger_Pressed + 2*Time.fixedDeltaTime))
                Trigger_Reset = true;
            else
                Trigger_Reset = false;
        }
    }

    // Shooting
    public override bool Primary ()
    {
        Trigger_Pressed = Time.fixedTime;
        bool value = Shoot();
        if(value)
        {
            if(Mode == Firearm_Mode.Semi || Mode == Firearm_Mode.Auto)
                Reload();
            if(Mode == Firearm_Mode.Auto)
                Trigger_Reset = true;
        }
        return value;
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

    // Reloading the weapon - getting new ammo into the chamber
    public override bool Reload () 
    {
        GameObject casing = null;
        if(Casing_In_Chamber || Ammo_In_Chamber)
        {
            if(Bullet_Casing_Spawn.childCount == 0)
                casing = Instantiate(Bullet_Casing, Bullet_Casing_Spawn);
            else
                casing = Bullet_Casing_Spawn.GetChild(0).gameObject;
        }
        // Fixing issue with stuck ammo casing
        if(Casing_Stuck)
        {
            if(Random.Range(0f, 1f) <= 0.8f)
            {
                if(Casing_In_Chamber || Ammo_In_Chamber)
                    Move_Casing(casing);
                Console.Log(this, "Ammo casing unstuck");
                Casing_Stuck = false;
                Casing_In_Chamber = false;
                Ammo_In_Chamber = false;
            }
            else
            {
                Console.Log(this, "Ammo casing still stuck");
                return false;
            }
        }
        // Getting ammo casing out and adding new ammo to chamber
        if(!Casing_Stuck)
        {
            if((Casing_In_Chamber || Ammo_In_Chamber) && Random.Range(0f, 1f) <= Gun_Failure) // Ammo casing stuck
            {
                Console.Log(this, "Ammo casing stuck");
                Casing_Stuck = true;
                return false;
            }
            else // Old casing out, new ammo in
            {
                if(Casing_In_Chamber || Ammo_In_Chamber)
                    Move_Casing(casing);
                Console.Log(this, "New ammo loaded");
                Casing_In_Chamber = false;
                Ammo_In_Chamber = true;
                Ammo_Failed = false;
            }
        }
        return true;
    }

    // Setting the bullet casing in motion
    private void Move_Casing (GameObject casing)
    {
        casing.transform.SetParent(null);
        casing.GetComponent<Rigidbody>().isKinematic = false;
        casing.GetComponent<Rigidbody>().useGravity = true;
        casing.GetComponent<Rigidbody>().AddForce( transform.up * 100f + transform.forward * 100f );
        Destroy(casing, 5f);
    }

    // Getting new magazine or stashing it
    public override bool Stash () 
    {
        return true;
    }

    // Shooting
    private bool Shoot ()
    {
        // Ammo casing is stuck
        if(Casing_Stuck)
        {
            Console.Log(this, "Ammo is stuck, cannot fire");
            return false;
        }
        // Ammo failed to fire
        if(Ammo_Failed)
        {
            Console.Log(this, "Ammo already failed");
            return false;
        }
        // Ammo not in chamber
        if(!Ammo_In_Chamber)
        {
            Console.Log(this, "No ammo in chamber");
            return false;
        }
        // Ammo not in chamber
        if(!Trigger_Reset)
        {
            Console.Log(this, "Trigger is not reset");
            return false;
        }
        // Ammo failure chance
        if(Random.Range(0f, 1f) <= Ammo_Failure)
        {
            Console.Log(this, "Ammo failed to fire");
            Ammo_Failed = true;
            return false;
        }
        else
        {
            Console.Log(this, "Ammo fired");
            Sound_Manager.Play("Fire");
            GameObject bullet = Instantiate(Bullet, Bullet_Spawn);
            bullet.transform.SetParent(null);
            bullet.GetComponent<Rigidbody>().velocity = Bullet_Spawn.TransformDirection(new Vector3(-Bullet_Speed, 0f, 0f));
            //bullet.GetComponent<Projectile_Bullet>().Damage = Damage;
            Ammo_In_Chamber = false;
            Casing_In_Chamber = true;
            Trigger_Reset = false;
            return true;
        }
    }


    //##########################################################################################
    //##################################  DEBUGGING TOSTRING  ##################################
    //##########################################################################################

    public override string SpecificToString ()
    {
        string output = "";
        output += "Bleeding caused by item: " + Bleeding.ToString() + " ml/s\n";
        output += "Gun failure chance: " + Math.Round(Gun_Failure*100, 2).ToString() + "%" + (Casing_Stuck ? " - CASING STUCK" : null) + "\n";
        output += "Ammo failure chance: " + Math.Round(Ammo_Failure*100, 2).ToString() + "%" + (Ammo_Failed ? " - AMMO FAILED" : null) + "\n";
        output += "Bullet in chamber: " + Ammo_In_Chamber.ToString() + "\n";
        return output;
    }

}
