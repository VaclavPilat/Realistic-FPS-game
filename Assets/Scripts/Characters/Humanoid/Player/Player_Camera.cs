using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Camera : Lockable_Script
{
    //##########################################################################################
    //######################################  COMPONENTS  ######################################
    //##########################################################################################

    private Human_Camera Human_Camera; // Human Camera script
    private Menu_Variables Menu_Variables; // Menu variables (for checking if player is allowed to do anything)
    
    private void Awake () 
    {
        Human_Camera = GetComponent<Human_Camera>();
        Menu_Variables = GameObject.Find("/Menu").GetComponent<Menu_Variables>();
    }

    
    //##########################################################################################
    //##################################  PUBLIC VARIABLES  ####################################
    //##########################################################################################

    public float Sensitivity = 100f; // Mouse Sensitivity
    public float Vertical = 90f; // Vertical rotation limit
    public float Horizontal = -1f; // Horizontal rotation limit


    //##########################################################################################
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################

    private float vertical_rotation = 0f; // Vertical rotation
    private float horizontal_rotation = 0f; // Horizontal rotation

    void FixedUpdate()
    {
        if(!Menu_Variables.Visible) // Only perform stuff if menu is not visible
        {
            Rotate_Player();
            Rotate_Camera();
        }
    }

    // ROTATING PLAYER
    private void Rotate_Player ()
    {
        if(Horizontal >= 0) // Using different rotation method when horizontal limit is used
        {
            horizontal_rotation += Input.GetAxis("Mouse X") * Sensitivity * Time.fixedDeltaTime;
            horizontal_rotation = Mathf.Clamp(horizontal_rotation, -Horizontal, Horizontal); // Clamping rotation so the player will not be able to look behind
            transform.Rotate(0.0f, horizontal_rotation, 0.0f); // Applying horizontal rotation to the player
        }
        else
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Sensitivity * Time.fixedDeltaTime);
    }

    // ROTATING Human_Camera.Camera
    private void Rotate_Camera ()
    {
        vertical_rotation -= Input.GetAxis("Mouse Y") * Sensitivity * Time.fixedDeltaTime;
        vertical_rotation = Mathf.Clamp(vertical_rotation, -Vertical, Vertical); // Clamping rotation so the player will not be able to look behind
        Human_Camera.Camera.transform.localRotation = Quaternion.Euler(vertical_rotation, 0.0f, 0.0f); // Applying vertical rotation to the Camera
    }


    //##########################################################################################
    //##################################  DEBUGGING TOSTRING  ##################################
    //##########################################################################################

    public override string ToString ()
    {
        string output = "";
        output += "Sensitivity: " + Sensitivity.ToString() + "\n";
        output += "Rotation limit (±): ↕ " + Vertical.ToString() + "°; ↔ " + ((Horizontal < 0) ? "none" : Horizontal.ToString() + "°") + "\n";
        output += "Player rotation: " + transform.rotation.eulerAngles.ToString() + " deg\n";
        output += "Camera rotation: " + Human_Camera.Camera.transform.localRotation.eulerAngles.ToString() + " deg\n";
        return output;
    }

}