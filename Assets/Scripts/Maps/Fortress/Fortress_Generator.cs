using System.Collections;
using UnityEngine;
using System;

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
        Unused, // This tile will not be used
        Tower, // Guard tower
        Fountain, // A dried out fountain
        Rubbish, // Small 1x1 building or an obstackle
        Wall, // Procedurally generated outer wall
        Building // Procedurally generated building
    }


    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################

    private Tile[,] Tiles = null; // 2D array of tiles that will be used for map generation
    private void Awake() => Generate_Map(); // Loading initial tiles and performing operations on them


    //##########################################################################################
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################
    
    // Generating map
    private void Generate_Map()
    {
        Load_Tiles();
        Replace_Patterns();
        Log_Tiles();
        Add_Outer_Walls();
        Log_Tiles();
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
                Add_Outer_Layer(Tile.Unused);
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
            {Tile.Tower,Tile.Unused,Tile.Unused,Tile.Unused,Tile.Unused,Tile.Unused,Tile.Unused},
            {Tile.Unused,Tile.Unused,Tile.Unused,Tile.Unused,Tile.Unused,Tile.Unused,Tile.Unused},
            {Tile.Unused,Tile.Unused,Tile.Unused,Tile.Unused,Tile.Unused,Tile.Unused,Tile.Unused},
            {Tile.Unused,Tile.Unused,Tile.Unused,Tile.Unused,Tile.Unused,Tile.Unused,Tile.Unused},
            {Tile.Unused,Tile.Unused,Tile.Unused,Tile.Unused,Tile.Unused,Tile.Unused,Tile.Unused},
            {Tile.Unused,Tile.Unused,Tile.Unused,Tile.Unused,Tile.Unused,Tile.Unused,Tile.Unused},
            {Tile.Unused,Tile.Unused,Tile.Unused,Tile.Unused,Tile.Unused,Tile.Unused,Tile.Unused}
        };
        Replace_All_Tiles(tower_pattern, tower_replacement);
        // Replacing fountains
        Tile? [,] fountain_pattern = {
            {Tile.Empty,Tile.Empty,Tile.Empty},
            {Tile.Empty,Tile.Filled,Tile.Empty},
            {Tile.Empty,Tile.Empty,Tile.Empty}
        };
        Tile? [,] fountain_replacement = {
            {null,null,null},
            {null,Tile.Fountain,null},
            {null,null,null}
        };
        Replace_All_Tiles(fountain_pattern, fountain_replacement);
    }

}
