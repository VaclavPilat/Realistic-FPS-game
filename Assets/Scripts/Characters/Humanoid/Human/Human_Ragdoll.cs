using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// Class for controlling Humanoid characters with ragdoll body
public class Human_Ragdoll : Lockable_Script
{
    //##########################################################################################
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################

    private void Awake () => Disable();

    // Disable ragdoll behaviour on all bodyparts
    private void Disable ()
    {
        foreach(Rigidbody rb in transform.GetComponentsInChildren<Rigidbody>())
            if(rb.gameObject != gameObject)
                rb.isKinematic = true;
    }

    // Enabling character ragdoll physics
    public void Enable ()
    {
        foreach(Rigidbody rb in transform.GetComponentsInChildren<Rigidbody>())
            if(rb.gameObject != gameObject)
                rb.isKinematic = false;
    }

    // Enabling ragdoll behaviour only on some bodyparts
    public void Enable_Some (Collider[] colliders)
    {
        foreach(Collider c in colliders)
        {
            Rigidbody rb = c.transform.GetComponent<Rigidbody>();
            if(rb)
                rb.isKinematic = false;
        }
    }

}