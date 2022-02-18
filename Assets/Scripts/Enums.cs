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