using System.Collections;
using UnityEngine;
using System;

// Types of tiles in this map
public enum Fortress_Tile
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

// Generating a map from a QR code
public class Fortress_Generator : MonoBehaviour
{
    //##########################################################################################
    //######################################  COMPONENTS  ######################################
    //##########################################################################################


    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################

    private Fortress_Tile[,] Tiles = null; // 2D array of tiles that will be used for map generation
    private void Awake() => Generate_Map(); // Loading initial tiles and performing operations on them


    //##########################################################################################
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################
    
    // Generating map
    private void Generate_Map()
    {
        Load_Tiles();
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
        Tiles = (Fortress_Tile[,])(object)Tile_Indexes;
    }

    // Adding an outer layer to the tileset
    private void Add_Outer_Layer(Fortress_Tile tile)
    {
        // Creating new array of tiles
        int old_size = Tiles.GetLength(0);
        int new_size = old_size + 2;
        Fortress_Tile [,] new_tiles = new Fortress_Tile[new_size, new_size];
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
        if(Tiles[0, 0] != Fortress_Tile.Wall)
        {
            if(Tiles[0, 0] != Fortress_Tile.Empty)
                Add_Outer_Layer(Fortress_Tile.Unused);
            Add_Outer_Layer(Fortress_Tile.Wall);
        }
    }

    // Looking for a pattern and replacing it with a new one
    private void Replace_Tiles(Fortress_Tile[,] pattern, Fortress_Tile[,] replacement)
    {

    }

}
