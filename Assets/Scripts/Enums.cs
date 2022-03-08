using System;

// List of item categories 
// (with a hint on what alpha keys they are binded to)
public enum Item_Category
{
    Tablet      = 0,
    Primary     = 1,
    Secondary   = 2,
    Melee       = 3,
    Throwable   = 4,
    Consumable  = 5
}

// Item usage - is the item one handed or two-handed?
// (ith a hint on minimum hands required)
public enum Item_Usage
{
    One         = 0,
    PreferTwo   = 1,
    Two         = 2
}

// Character pose
// (with a hint on character controller height [cm])
public enum Character_Pose
{
    Standing    = 180,
    Sitting     = 130,
    Crouching   = 90,
    Lying       = 50
}

// Gamemodes
public enum Game_Mode
{
    SOLO, // Solo matches
    CTF, // Cpture the flag
    DM, // Deathmatch
    TDM, // Team deathmatch
    KOTH // King of the hill
}

// Fire modes for firearms
public enum Firearm_Mode
{
    Action, // User has to manually reload the weapon between shots
    Semi, // Semi-automatic fire - one click, one shot, waiting for trigger reset
    Auto // Full-automatic fire
}