using System.Collections;
using UnityEngine;
using System;
using System.Linq;

// Generating a map from a QR code
public class Fortress_Generator : MonoBehaviour
{
    //##########################################################################################
    //######################################  COMPONENTS  ######################################
    //##########################################################################################

    // Types of tiles in this map
    private enum Tile
    {
        Empty, // Nothing here, not yet used
        Filled, // Something here, not yet used
        Tower, // Guard tower
        Fountain, // A dried out fountain
        Rubbish, // Small 1x1 building or an obstackle
        Wall, // Procedurally generated outer wall
        Building, // Procedurally generated building
        Container, // Shipping container
        Vehicle // Vehicle or its residue
    }


    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################

    private Tile[,] Tiles = null; // 2D array of tiles that will be used for map generation
    private int Tile_Size = 3; // Size of a single tile (in meters)

    private GameObject[] Prefabs; // Array of loaded setting prefabs

    private Material[] Roofing_Materials; // Array of materials

    private GameObject[,] Instances; // 2D array of instances
    private int?[,] Flood_Indexes; // 2D array of indexes from a flood fill algorithm

    // Attempts to find a setting prefab by name
    private GameObject Find_Prefab (string name)
    {
        return Array.Find(Prefabs, g => g.name == name);
    }


    //##########################################################################################
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################

    // Generating map
    private void Awake()
    {
        // Loading resources
        Prefabs = Resources.LoadAll<GameObject>("Maps/Fortress/"); // Loading all tile prefabs
        Roofing_Materials = Resources.LoadAll<Material>("Maps/Fortress/Roofing"); // Loading all materials
        // Manipulating tiles
        Load_Tiles();
        Add_Outer_Walls();
        Replace_Patterns();
        Log_Tiles();
        // Generating map
        Instances = new GameObject[Tiles.GetLength(0), Tiles.GetLength(1)];
        Instantiate_Tiles();
        // Flood filling the tiles
        Flood_Indexes = new int?[Tiles.GetLength(0), Tiles.GetLength(1)];
        Flood_Fill_Instances();
        Log_Flood_Indexes();
        Change_Building_Materials();
    }

    // Printing out tiles into console
    private void Log_Tiles()
    {
        string output = "\n";
        int size = Tiles.GetLength(0);
        for(int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
                output += (int)Tiles[i, j];
            output += "\n";
        }
        Console.Log(this, output);
    }

    // Loading initial 2D array of tiles
    private void Load_Tiles()
    {
        int [,] Tile_Indexes = {
            {1,1,1,1,1,1,1,0,1,0,1,0,1,0,1,1,1,0,1,1,1,1,1,1,1},
            {1,0,0,0,0,0,1,0,0,1,1,1,1,0,1,0,0,0,1,0,0,0,0,0,1},
            {1,0,1,1,1,0,1,0,1,1,1,0,0,1,1,0,0,0,1,0,1,1,1,0,1},
            {1,0,1,1,1,0,1,0,1,1,1,0,1,1,1,1,0,0,1,0,1,1,1,0,1},
            {1,0,1,1,1,0,1,0,1,0,0,1,1,0,0,1,1,0,1,0,1,1,1,0,1},
            {1,0,0,0,0,0,1,0,0,0,1,1,0,0,1,0,0,0,1,0,0,0,0,0,1},
            {1,1,1,1,1,1,1,0,1,0,1,0,1,0,1,0,1,0,1,1,1,1,1,1,1},
            {0,0,0,0,0,0,0,0,0,0,1,0,1,0,0,1,1,0,0,0,0,0,0,0,0},
            {1,1,1,1,0,0,1,0,1,0,1,1,0,0,0,1,1,1,0,0,1,1,1,0,1},
            {1,0,0,1,0,0,0,1,1,0,1,0,1,0,1,0,0,1,0,1,0,0,0,1,0},
            {0,0,1,0,1,0,1,1,1,1,1,1,1,0,0,0,1,0,0,1,0,0,0,0,0},
            {1,0,0,0,1,1,0,1,0,1,1,0,0,1,0,0,0,1,0,0,1,1,1,0,0},
            {1,1,0,1,1,0,1,0,1,1,1,0,0,1,1,1,0,1,1,0,1,0,1,1,1},
            {0,0,1,1,0,0,0,1,0,1,0,1,1,0,0,1,1,0,1,1,1,0,0,0,1},
            {0,1,0,0,1,0,1,0,1,0,0,0,1,0,1,0,1,1,0,0,0,0,1,1,0},
            {1,0,0,0,0,0,0,1,1,0,0,1,1,0,1,0,0,0,1,0,1,1,0,0,1},
            {0,0,0,1,0,1,1,0,0,0,1,1,0,1,1,0,1,1,1,1,1,1,1,1,1},
            {0,0,0,0,0,0,0,0,1,0,0,1,1,0,1,0,1,0,0,0,1,0,0,0,1},
            {1,1,1,1,1,1,1,0,0,1,0,0,1,0,0,0,1,0,1,0,1,1,1,1,1},
            {1,0,0,0,0,0,1,0,0,0,0,1,0,0,0,1,1,0,0,0,1,0,0,0,1},
            {1,0,1,1,1,0,1,0,0,1,0,1,1,1,1,0,1,1,1,1,1,0,0,0,1},
            {1,0,1,1,1,0,1,0,1,1,0,0,0,0,1,1,1,1,1,1,1,0,1,1,1},
            {1,0,1,1,1,0,1,0,1,0,1,1,1,1,0,1,1,0,1,1,1,1,1,1,0},
            {1,0,0,0,0,0,1,0,1,1,1,1,1,1,1,0,0,1,0,0,1,0,1,0,0},
            {1,1,1,1,1,1,1,0,1,0,1,0,1,1,1,1,0,1,1,1,1,1,1,1,1}
        };
        Tiles = (Tile[,])(object)Tile_Indexes;
    }

    // Adding an outer layer to the tileset
    private void Add_Outer_Layer(Tile tile)
    {
        // Creating new array of tiles
        int old_size = Tiles.GetLength(0);
        int new_size = old_size + 2;
        Tile [,] new_tiles = new Tile[new_size, new_size];
        // Filling the new array of tiles
        for(int i = 0; i < new_size; i++)
        {
            // Adding outer walls
            new_tiles[0, i] = tile;
            new_tiles[i, 0] = tile;
            new_tiles[new_size - 1, i] = tile;
            new_tiles[i, new_size - 1] = tile;
        }
        // Adding the original tiles
        for(int i = 0; i < old_size; i++)
            for(int j = 0; j < old_size; j++)
                new_tiles[i + 1, j + 1] = Tiles[i, j];
        // Setting the new tiles as the current ones
        Tiles = new_tiles;
    }

    // Adding outer walls and a spacing before it
    private void Add_Outer_Walls()
    {
        if(Tiles[0, 0] != Tile.Wall)
        {
            if(Tiles[0, 0] != Tile.Empty)
                Add_Outer_Layer(Tile.Empty);
            Add_Outer_Layer(Tile.Wall);
        }
    }

    // Attempting to replace a pattern with another one on a specific spot
    private void Replace_Tile(Tile?[,] pattern, Tile?[,] replacement, int i, int j)
    {
        // Checking if the patterns exists
        for(int k = 0; k < pattern.GetLength(0); k++)
            for(int l = 0; l < pattern.GetLength(1); l++)
                if(pattern[k, l] != null && Tiles[i + k, j + l] != pattern[k, l])
                    return;
        // Overwriting the occurence of the pattern
        for(int k = 0; k < pattern.GetLength(0); k++)
            for(int l = 0; l < pattern.GetLength(1); l++)
                if(replacement[k, l] != null)
                    Tiles[i + k, j + l] = (Tile)replacement[k, l];
    }

    // Looking for a pattern and replacing it with a new one
    private void Replace_All_Tiles(Tile?[,] pattern, Tile?[,] replacement)
    {
        if(pattern.GetLength(0) != replacement.GetLength(0) || pattern.GetLength(1) != replacement.GetLength(1))
        {
            Console.Warning(this, "Pattern and replacement sizes do not match!");
            return;
        }
        int size = Tiles.GetLength(0);
        for(int i = 0; i < size - pattern.GetLength(0) + 1; i++)
            for(int j = 0; j < size - pattern.GetLength(1) + 1; j++)
                Replace_Tile(pattern, replacement, i, j);
    }

    // Listing all patterns and their replacement
    private void Replace_Patterns()
    {
        // Replacing towers
        Tile? [,] tower_pattern = {
            {Tile.Filled,Tile.Filled,Tile.Filled,Tile.Filled,Tile.Filled,Tile.Filled,Tile.Filled},
            {Tile.Filled,Tile.Empty,Tile.Empty,Tile.Empty,Tile.Empty,Tile.Empty,Tile.Filled},
            {Tile.Filled,Tile.Empty,Tile.Filled,Tile.Filled,Tile.Filled,Tile.Empty,Tile.Filled},
            {Tile.Filled,Tile.Empty,Tile.Filled,Tile.Filled,Tile.Filled,Tile.Empty,Tile.Filled},
            {Tile.Filled,Tile.Empty,Tile.Filled,Tile.Filled,Tile.Filled,Tile.Empty,Tile.Filled},
            {Tile.Filled,Tile.Empty,Tile.Empty,Tile.Empty,Tile.Empty,Tile.Empty,Tile.Filled},
            {Tile.Filled,Tile.Filled,Tile.Filled,Tile.Filled,Tile.Filled,Tile.Filled,Tile.Filled}
        };
        Tile? [,] tower_replacement = {
            {null,null,null,null,null,null,null},
            {null,null,null,null,null,null,null},
            {null,null,Tile.Tower,Tile.Empty,Tile.Empty,null,null},
            {null,null,Tile.Empty,Tile.Empty,Tile.Empty,null,null},
            {null,null,Tile.Empty,Tile.Empty,Tile.Empty,null,null},
            {null,null,null,null,null,null,null},
            {null,null,null,null,null,null,null}
        };
        Replace_All_Tiles(tower_pattern, tower_replacement);
        // Replacing fountains
        /*Tile? [,] fountain_pattern = {
            {Tile.Filled,Tile.Filled,Tile.Filled,Tile.Filled,Tile.Filled},
            {Tile.Filled,Tile.Empty,Tile.Empty,Tile.Empty,Tile.Filled},
            {Tile.Filled,Tile.Empty,Tile.Filled,Tile.Empty,Tile.Filled},
            {Tile.Filled,Tile.Empty,Tile.Empty,Tile.Empty,Tile.Filled},
            {Tile.Filled,Tile.Filled,Tile.Filled,Tile.Filled,Tile.Filled}
        };
        Tile? [,] fountain_replacement = {
            {null,null,null,null,null},
            {null,null,null,null,null},
            {null,null,Tile.Fountain,null,null},
            {null,null,null,null,null},
            {null,null,null,null,null}
        };
        Replace_All_Tiles(fountain_pattern, fountain_replacement);
        // Replacing 1x1 buildings / rubbish
        Tile? [,] rubbish_pattern = {
            {null,Tile.Empty,null},
            {Tile.Empty,Tile.Filled,Tile.Empty},
            {null,Tile.Empty,null}
        };
        Tile? [,] rubbish_replacement = {
            {null,null,null},
            {null,Tile.Rubbish,null},
            {null,null,null}
        };
        Replace_All_Tiles(rubbish_pattern, rubbish_replacement);
        // Replacing a shipping container
        Tile? [,] container_pattern = {
            {null,Tile.Empty,null},
            {Tile.Empty,Tile.Filled,Tile.Empty},
            {Tile.Empty,Tile.Filled,Tile.Empty},
            {null,Tile.Empty,null}
        };
        Tile? [,] container_replacement = {
            {null,null,null},
            {null,Tile.Container,null},
            {null,Tile.Empty,null},
            {null,null,null}
        };
        Replace_All_Tiles(container_pattern, container_replacement);
        // Replacing vehicle
        Tile? [,] vehicle_pattern = {
            {null,Tile.Empty,Tile.Empty,null},
            {Tile.Empty,Tile.Filled,Tile.Filled,Tile.Empty},
            {null,Tile.Empty,Tile.Empty,null}
        };
        Tile? [,] vehicle_replacement = {
            {null,null,null,null},
            {null,Tile.Vehicle,Tile.Empty,null},
            {null,null,null,null}
        };
        Replace_All_Tiles(vehicle_pattern, vehicle_replacement);*/
        // Replacing buildings
        Tile? [,] building_pattern = {
            {Tile.Filled}
        };
        Tile? [,] building_replacement = {
            {Tile.Building}
        };
        Replace_All_Tiles(building_pattern, building_replacement);
    }

    // Showing tiles on scene
    private void Instantiate_Tiles()
    {
        int size = Tiles.GetLength(0);
        float offset = ((size / 2.0f) * Tile_Size);
        // Instantiating outer ground
        GameObject outer_ground = Find_Prefab("Outer_Ground");
        Instantiate(outer_ground, new Vector3(Tile_Size - offset, 0, offset - Tile_Size), Quaternion.Euler(0, 0, 0));
        Instantiate(outer_ground, new Vector3(offset - Tile_Size, 0, offset - Tile_Size), Quaternion.Euler(0, 90, 0));
        Instantiate(outer_ground, new Vector3(offset - Tile_Size, 0, Tile_Size - offset), Quaternion.Euler(0, 180, 0));
        Instantiate(outer_ground, new Vector3(Tile_Size - offset, 0, Tile_Size - offset), Quaternion.Euler(0, 270, 0));
        // Instantiating inner prefabs
        GameObject ground = Find_Prefab("Ground");
        for(int i = 0; i < size; i++)
            for(int j = 0; j < size; j++)
            {
                Tile tile = Tiles[i, j];
                // Instantiating ground
                if(tile != Tile.Wall)
                    Instantiate(ground, new Vector3(j*Tile_Size - offset, 0, offset - (i+1)*Tile_Size), new Quaternion(0, 0, 0, 1));
                // Instantiating special prefabs
                switch(tile)
                {
                    case Tile.Fountain:
                    case Tile.Rubbish:
                    case Tile.Tower:
                    case Tile.Wall:
                    case Tile.Container:
                    case Tile.Vehicle:
                        var prefab = Find_Prefab(tile.ToString());
                        Instantiate(prefab, new Vector3(j*Tile_Size - offset, 0, offset - i*Tile_Size), new Quaternion(0, 0, 0, 1));
                        break;
                    case Tile.Building:
                        Prepare_Procedural_Building(i, j, offset);
                        break;
                    default:
                        break;
                }
            }
    }

    // Getting a string that represents the building
    private string Get_Building_String(int i, int j)
    {
        string value = "";
        value += (Tiles[i-1,j-1] == Tile.Building && Tiles[i-1,j] == Tile.Building && Tiles[i,j-1] == Tile.Building ? "1" : "0"); // Top left
        value += (Tiles[i-1,j] == Tile.Building ? "1" : "0"); // Top
        value += (Tiles[i-1,j+1] == Tile.Building && Tiles[i-1,j] == Tile.Building && Tiles[i,j+1] == Tile.Building ? "1" : "0"); // Top right
        value += (Tiles[i,j-1] == Tile.Building ? "1" : "0"); // Left
        value += "1"; // Middle
        value += (Tiles[i,j+1] == Tile.Building ? "1" : "0"); // Right
        value += (Tiles[i+1,j-1] == Tile.Building && Tiles[i+1,j] == Tile.Building && Tiles[i,j-1] == Tile.Building ? "1" : "0"); // Bottom left
        value += (Tiles[i+1,j] == Tile.Building ? "1" : "0"); // Bottom
        value += (Tiles[i+1,j+1] == Tile.Building && Tiles[i+1,j] == Tile.Building && Tiles[i,j+1] == Tile.Building ? "1" : "0"); // Bottom right
        return value;
    }

    // Rotating building by 90 degrees clockwise
    private string Rotate_Building_String(string building)
    {
        return building.Substring(2, 1) + building.Substring(5, 1) + building.Substring(8, 1) + building.Substring(1, 1) + building.Substring(4, 1) + building.Substring(7, 1) + building.Substring(0, 1) + building.Substring(3, 1) + building.Substring(6, 1);
    }

    // Finding building prefab and instantiating it with a specific rotation and scale
    private void Prepare_Procedural_Building(int i, int j, float offset)
    {
        // Getting building string
        string building = Get_Building_String(i, j);
        // Variables for representing current rotation
        int rotates = 0;
        GameObject prefab;
        // Trying every possible rotation
        for(rotates = 0; rotates < 4; rotates++)
        {
            prefab = Find_Prefab("Building_" + building);
            if(prefab != null)
            {
                Instantiate_Procedural_Building(i, j, offset, prefab, rotates);
                return;
            }
            building = Rotate_Building_String(building);
        }
        Console.Warning(this, i.ToString() + "-" + j.ToString() + " : Unable to find correct building prefab");
    }

    // Rotating and translating instantiated building
    private void Rotate_Instance(GameObject instance, int tiles_w, int tiles_h, int rotates)
    {
        instance.transform.RotateAround(instance.transform.position + new Vector3(tiles_w * Tile_Size / 2f, 0, -tiles_h * Tile_Size / 2f), Vector3.up, 90 * rotates);
    }

    // Finding building prefab and instantiating it with a specific rotation and scale
    private void Instantiate_Procedural_Building(int i, int j, float offset, GameObject prefab, int rotates)
    {
        // Instantiating prefab instance
        //Console.Log(this, i.ToString() + "-" + j.ToString() + " : " + prefab.name + ", " + rotates.ToString() + "x clockwise");
        var instance = Instantiate(prefab, new Vector3(j*Tile_Size - offset, 0, offset - i*Tile_Size), Quaternion.Euler(0, 0, 0));
        Rotate_Instance(instance, 1, 1, rotates);
        instance.name = "Building, " + i.ToString() + "-" + j.ToString() + ", " + rotates.ToString() + "x clockwise";
        // Adding instance to array
        Instances[i, j] = instance;
    }

    // Setting a material to all children with a set name
    private void Set_Children_Material (Transform instance, string name, Material material)
    {
        foreach (Transform element in instance.GetComponentsInChildren<Transform>().Where(t => t.name == name).ToArray())
            element.GetComponent<Renderer>().material = material;
    }

    // Flooding tiles
    private int Flood_Fill (int size, int i, int j, int index, int count = 0)
    {
        if(Instances[i, j] != null && Flood_Indexes[i, j] == null)
        {
            Flood_Indexes[i, j] = index;
            count++;
            if(j < size -1)
                count += Flood_Fill(size, i, j+1, index);
            if(i < size -1)
                count += Flood_Fill(size, i+1, j, index);
            if(i > 0)
                count += Flood_Fill(size, i-1, j, index);
            if(j > 0)
                count += Flood_Fill(size, i, j-1, index);
        }
        return count;
    }

    // Flood filling tiles
    private void Flood_Fill_Instances ()
    {
        int size = Flood_Indexes.GetLength(0);
        int index = 0;
        for(int i = 0; i < Instances.GetLength(0); i++)
            for(int j = 0; j < Instances.GetLength(0); j++)
                if(Flood_Fill(size, i, j, index) > 0)
                    index++;
    }

    // Logging flood indexes
    private void Log_Flood_Indexes ()
    {
        string output = "";
        for(int i = 0; i < Instances.GetLength(0); i++)
        {
            output += "\n";
            for(int j = 0; j < Instances.GetLength(0); j++)
                output += " " + (Flood_Indexes[i, j] != null ? Flood_Indexes[i, j].ToString() : "-");
        }
        Console.Log(this, output);
    }

    // Changing building materials
    private void Change_Building_Materials ()
    {
        // Setting roofing materials
        for(int i = 0; i < Instances.GetLength(0); i++)
            for(int j = 0; j < Instances.GetLength(0); j++)
                if(Instances[i, j] != null)
                    Set_Children_Material(Instances[i, j].transform, "Roofing", Roofing_Materials[ (int)Flood_Indexes[i, j] % Roofing_Materials.Length ]);
    }

}