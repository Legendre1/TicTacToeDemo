using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UserdataManager
{
    //This class acts as a wrapper around Unity's userdata storage system


    //this helps to prevent typos in player prefs string keys
    private static class StorageFields
    {
        public static string GridSize = "grid_size";
    }

    #region Public Userdata Access

    //Get/Set the grid size of the TTT game
    public static void SetGridSize(int grid_size)
    {
        if (grid_size != 3 && grid_size != 4)
        {
            Debug.LogError("Grid size must be either 3 or 4!");
        }

        Debug.Log("Setting to " + grid_size);

        PlayerPrefs.SetInt(StorageFields.GridSize, grid_size);
    }

    public static int GetGridSize()
    {
        int grid_size = PlayerPrefs.GetInt(StorageFields.GridSize);
        if (grid_size == 0)
        {
            //initialize the value if it is unset
            grid_size = 3;
            PlayerPrefs.SetInt(StorageFields.GridSize, grid_size);
        }

        return grid_size;
    }

    #endregion

}
