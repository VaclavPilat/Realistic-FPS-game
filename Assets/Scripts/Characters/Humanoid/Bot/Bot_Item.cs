using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controlling the current item
public class Bot_Item : Lockable_Script
{
    //##########################################################################################
    //######################################  COMPONENTS  ######################################
    //##########################################################################################

    private Human_Inventory Human_Inventory; // Human inventory script
    private LayerMask Character_Layer; // Character layer
    private Human_Health Human_Health; // Human health

    private void Awake () 
    {
        Human_Inventory = transform.GetComponent<Human_Inventory>();
        Character_Layer = LayerMask.GetMask("Character");
        Human_Health = GetComponent<Human_Health>();
    }


    //##########################################################################################
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################

    private void Start () => StartCoroutine(Shoot_At_Enemy());

    // Shooting at an enemy from a firearm
    private IEnumerator Shoot_At_Enemy ()
    {
        while(Human_Health.Alive)
        {
            if(Human_Inventory.Current_Item != null)
            {
                // Using a firearm
                Item_Firearm firearm = Human_Inventory.Current_Item.GetComponent<Item_Firearm>();
                if(firearm)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(firearm.Bullet_Spawn.position, firearm.Bullet_Spawn.TransformDirection(new Vector3(-1, 0f, 0f)), out hit, 10f, Character_Layer))
                    {
                        Console.Log(this, "Target in front of sights");
                        if(!firearm.Primary())
                            firearm.Reload();
                    }
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

}
