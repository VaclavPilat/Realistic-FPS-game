using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

// Editor for creating materials and texture manipulation
public class Material_Creator : Editor
{

    //##########################################################################################
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################

    // Getting a mututal string from a list of strings
    static string Get_Common_Path (List<string> filepaths)
    {
        int shortest_length = -1;
        foreach(string path in filepaths)
        {
            // Getting length
            int length = path.Length;
            if(shortest_length == -1 || length < shortest_length)
                shortest_length = length;
        }
        // Getting the common part of strings
        int i;
        for(i = 0; i < shortest_length; i++)
            foreach(string path in filepaths)
                if(path[i] != filepaths[0][i])
                    return filepaths[0].Substring(0, i);
        return filepaths[0];
    }

    [MenuItem("Tools/Material from selected AmbientCG textures")]
    // Creating a material asset from selected textures
    static void Create_Material_From_AmbientCG ()
    {
        try
        {
            AssetDatabase.StartAssetEditing();
            // Checking if any textures were selected at all
            var textures = Selection.GetFiltered(typeof(Texture), SelectionMode.Assets).Cast<Texture>();
            if(textures != null && textures.Any())
            {
                // Creating a material
                Material material = new Material(Shader.Find("Standard"));
                List<string> filepaths = new List<string>();
                // Checking textures
                foreach(var texture in textures)
                {
                    string path = AssetDatabase.GetAssetPath(texture);
                    filepaths.Add(path);
                    // Applying texture to material
                    string name = Path.GetFileName(path);
                    if(name.Contains("_Color")) // Albedo
                        material.SetTexture("_MainTex", texture);
                    else if(name.Contains("_NormalGL")) // OpenGL Core is default for Linux
                    {
                        material.EnableKeyword("_NORMALMAP");
                        material.SetTexture("_BumpMap", texture);
                    }
                    else if(name.Contains("_Displacement")) // Height
                    {
                        material.EnableKeyword("_PARALLAXMAP");
                        material.SetTexture("_ParallaxMap", texture);
                    }
                    else if(name.Contains("_Metallness")) // Metallic map
                    {
                        material.EnableKeyword("_METALLICGLOSSMAP");
                        material.SetTexture("_MetallicGlossMap", texture);
                    }
                    else if(name.Contains("_AmbientOcclusion")) // Ambient occlusion
                    {
                        material.SetTexture("_OcclusionMap", texture);
                    }
                    else
                        Console.Warning(null, "Texture was not used: " + name);
                }
                // Getting material name
                char[] trimmed_chars = { '_', ' ', '-'};
                string material_name = Get_Common_Path(filepaths).Trim(trimmed_chars) + ".mat";
                // Saving material
                if (AssetDatabase.LoadAssetAtPath(material_name, typeof(Material)) != null)
                    Console.Warning(null, "Material already exists: " + material_name);
                else
                {
                    AssetDatabase.CreateAsset(material, material_name);
                    Console.Log(null, "Material saved as: " + material_name);
                }
            }
            else
                Console.Warning(null, "No textures were selected!");
        }
        finally
        {
            AssetDatabase.StopAssetEditing();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

}