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

    public void Enable () => Set_Enable(true);
    public void Disable () => Set_Enable(false);

    // Enabling or disabling current character ragdoll physics
    private void Set_Enable (bool enable)
    {
        foreach(Rigidbody rb in transform.GetComponentsInChildren<Rigidbody>())
            if(rb.gameObject != gameObject)
                rb.isKinematic = !enable;
    }

}