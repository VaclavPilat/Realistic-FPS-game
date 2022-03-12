using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

// Inventory
public class Player_Inventory : Lockable_Script
{
    //##########################################################################################
    //######################################  COMPONENTS  ######################################
    //##########################################################################################

    private Human_Inventory Human_Inventory; // Common inventory code
    private Player_Camera Player_Camera; // Player camera script
    
    private void Awake () 
    {
        Human_Inventory = transform.GetComponent<Human_Inventory>();
        Player_Camera = transform.GetComponent<Player_Camera>();
    }


    //##########################################################################################
    //##################################  PUBLIC VARIABLES  ####################################
    //##########################################################################################

    public int Item_Layer = 6; // Item layer number
    public float Distance = 2.5f; // Maximum distance for picking up items


    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################

    private float Charged = 0f; // Amount charged in seconds
    private bool Charging = false; // Is throw being charged?


    //##########################################################################################
    //###############################  PUBLIC BINDABLE METHODS  ################################
    //##########################################################################################

    [Bindable("Switching to last used item")]
    public bool Last () 
    {
        return Human_Inventory.Switch(Human_Inventory.Last_Item);
    }

    [Bindable("Switching to information overview")]
    public bool Tablet () 
    {
        return Switch_To_Category(Item_Category.Tablet);
    }

    [Bindable("Switching to primary weapon")]
    public bool Primary () 
    {
        return Switch_To_Category(Item_Category.Primary);
    }

    [Bindable("Switching to secondary weapon")]
    public bool Secondary () 
    {
        return Switch_To_Category(Item_Category.Secondary);
    }

    [Bindable("Switching to melee weapon")]
    public bool Melee () 
    {
        return Switch_To_Category(Item_Category.Melee);
    }

    [Bindable("Switching to consumable item")]
    public bool Consumable () 
    {
        return Switch_To_Category(Item_Category.Consumable);
    }

    [Bindable("Switching to throwable")]
    public bool Throwable () 
    {
        return Switch_To_Category(Item_Category.Throwable);
    }

    [Bindable("Picking up an item")]
    public bool Pickup () 
    {
        var item = Check_For_Item();
        if(item != null)
            return Human_Inventory.Pickup(item);
        else
            return false;
    }

    [Bindable("Charging throw force")]
    public bool Charge () 
    {
        if(Charged < 1f)
        {
            Charging = true;
            Charged += Time.fixedDeltaTime;
            return false;
        }
        else
        {
            Charging = false;
            Charged = 1f;
            return true;
        }
    }

    [Bindable("Throwing an item")]
    public bool Throw () 
    {
        bool response = Human_Inventory.Drop(Charged);
        Charged = 0f;
        Charging = false;
        return response;
    }

    [Bindable("Dropping all items from inventory")]
    public bool Dump ()
    {
        return Human_Inventory.Dump();
    }


    //##########################################################################################
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################

    // Getting an item from category
    private bool Switch_To_Category (Item_Category category)
    {
        GameObject item = Get_Next_In_Category(category);
        if(item != null)
            return Human_Inventory.Switch(item);
        else
            return false;
    }

    // Getting next item in the selected category
    private GameObject Get_Next_In_Category (Item_Category category)
    {
        List<GameObject> items = Human_Inventory.Inventory.Where(item => item.GetComponent<Item>().Category == category).ToList();
        if(items.Count == 0)
            return null;
        else if(items.Contains(Human_Inventory.Current_Item))
        {
            if(items.Count == 1)
                return items[0];
            else
            {
                int index = items.IndexOf(Human_Inventory.Current_Item);
                if(index < items.Count -1)
                    return items[index +1];
                else
                    return items[0];
            }
        }
        else
            return items[0];
    }

    // Checking an item for pickup
    private GameObject Check_For_Item ()
    {
        RaycastHit hit;
        if (Physics.Raycast(Player_Camera.Camera.transform.position, Player_Camera.Camera.transform.forward, out hit, Distance, 1 << Item_Layer))
            return hit.transform.gameObject;
        else 
            return null;
    }


    //##########################################################################################
    //##################################  DEBUGGING TOSTRING  ##################################
    //##########################################################################################

    public override string ToString ()
    {
        string output = "";
        output += "Throw force: " + Math.Round(Charged, 1).ToString() + (Charging ? ", charging" : null) + "\n";
        return output;
    }

}
