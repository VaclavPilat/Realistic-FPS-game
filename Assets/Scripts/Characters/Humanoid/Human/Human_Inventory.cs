using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = System.Random;

// Inventory
public class Human_Inventory : Lockable_Script
{
    //##########################################################################################
    //######################################  COMPONENTS  ######################################
    //##########################################################################################

    private Human_Camera Human_Camera;
    private void Awake () => Human_Camera = GetComponent<Human_Camera>();

    public GameObject Tablet; // Tablet prefab
    private void Start () => Pickup(Instantiate(Tablet));

    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################

    private float Throw_Force = 600f; // Throw force


    //##########################################################################################
    //##################################  PUBLIC VARIABLES  ####################################
    //##########################################################################################

    [HideInInspector] public GameObject Current_Item = null; // Current selected item
    [HideInInspector] public GameObject Last_Item = null; // Last used item
    [HideInInspector] public List<GameObject> Inventory = new List<GameObject>(); // List of all items in inventory


    //##########################################################################################
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################

    // Sorting inventory
    private void Sort_Inventory () => Inventory = Inventory.OrderBy(item => item.GetComponent<Item>().Category).ToList();

    // Picking up an item
    public bool Pickup (GameObject item)
    {
        if(Current_Item == null || Current_Item.GetComponent<Item>().Switchable)
        {
            Inventory.Add(item);
            Sort_Inventory();
            Switch(item);
            Pickup_Effect(item);
            return true;
        }
        else
            return false;
    }

    // Pickup effects
    private void Pickup_Effect (GameObject item)
    {
        item.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        item.GetComponent<Collider>().enabled = false;
        item.GetComponent<Rigidbody>().isKinematic = true;
        item.transform.position = Human_Camera.Item_Position.position;
        item.transform.parent = Human_Camera.Item_Position;
        item.transform.localEulerAngles = item.GetComponent<Item>().Default_Rotation;
    }

    // Dropping an item from inventory
    public bool Drop (float multiplier = 0f)
    {
        if(Current_Item != null) 
        {
            GameObject item = Current_Item;
            GameObject new_item = Previous();
            Inventory.Remove(Current_Item);
            if(Inventory.Count > 0)
                Switch(new_item, true);
            else
                Switch(null, true);
            Drop_Effect(item, multiplier);
            return true;
        }
        else
            return false;
    }

    // Effects for throwing the item out of inventory
    private void Drop_Effect (GameObject item, float multiplier)
    {
        if(multiplier > 1f)
            multiplier = 1f;
        item.GetComponent<Rigidbody>().isKinematic = false;
        item.GetComponent<Collider>().enabled = true;
        item.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        item.transform.parent = null;
        item.GetComponent<Rigidbody>().AddForce(Human_Camera.Camera.transform.forward * multiplier * Throw_Force); // Throwing the item with a force in the direction Human_Camera.Camera is looking at
        Random rand = new Random();
        item.transform.localRotation *= Quaternion.Euler(rand.Next(-10, 10), rand.Next(-10, 10), rand.Next(-10, 10)); // Giving the object random rotation
    }
    
    // Gets previous item in inventory
    public GameObject Previous ()
    {
        if(Inventory.Count < 2)
            return null;
        else
        {
            int index = Inventory.IndexOf(Current_Item);
            if(index == 0)
                return Inventory[Inventory.Count -1];
            else
                return Inventory[index -1];
        }
    }
    
    // Gets next item in inventory
    public GameObject Next ()
    {
        if(Inventory.Count < 2)
            return null;
        else
        {
            int index = Inventory.IndexOf(Current_Item);
            if(index == Inventory.Count -1)
                return Inventory[0];
            else
                return Inventory[index +1];
        }
    }


    // Switching to another item
    public bool Switch (GameObject item, bool throwing = false)
    {
        //Console.Log(this, (Current_Item != null ? Current_Item.transform.name : "null") + " -> " + (item != null ? item.transform.name : "null"));
        if(item == Current_Item)
            return false;
        if(Current_Item != null && !Current_Item.GetComponent<Item>().Switchable && !throwing)
            return false;
        // Setting current item
        var tmp = Current_Item;
        if(Current_Item != null && !throwing) // Disabling 'old' weapon
            Current_Item.SetActive(false);
        Current_Item = item;
        if(Current_Item != null) // Enabling 'new' weapon
            Current_Item.SetActive(true);
        // Setting last item
        if(Inventory.Count == 0)
            Last_Item = null;
        else if(Inventory.Count == 1)
            Last_Item = Current_Item;
        else if(Inventory.Contains(tmp))
            Last_Item = tmp;
        else
            Last_Item = Previous();
        return true;
    }

    // Dropping all items from inventory at once
    public bool Dump ()
    {
        while(true)
            if(!Drop())
                return true;
        return false;
    }


    //##########################################################################################
    //##################################  DEBUGGING TOSTRING  ##################################
    //##########################################################################################

    public override string ToString ()
    {
        string output = "";
        output += "Max throw force: " + Throw_Force.ToString() + "\n";
        output += "Inventory (" + Inventory.Count.ToString() + "): ";
        if(Inventory.Count > 0)
            foreach(GameObject item in Inventory)
                output += item.name + ", ";
        else
            output += "null";
        output += "\n";
        output += "Current item: " + ((Current_Item != null) ? Current_Item.name : "null") + "\n";
        output += "Last item: " + ((Last_Item != null) ? Last_Item.name : "null") + "\n";
        // Printing out information about current item
        if(Current_Item != null)
            output += Current_Item.GetComponent<Item>().ToString();
        return output;
    }

}
