using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootSceneManager : MonoBehaviour {

    //Show the splash screen for two seconds then go to main menu
	void Start () {
        Invoke("goToMainMenu", 2.0f);
	}
	
	private void goToMainMenu()
    {
        ApplicationLifecycleManager.GoToMainMenu();
    }
}
