using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ApplicationLifecycleManager {

	
	public static void GoToOptionsMenu()
    { 
    
    }

    public static void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public static void GoToTTTGame()
    { 
    
    }
}
