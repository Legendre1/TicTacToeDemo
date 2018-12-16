using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour {

	public void goToOptionsMenu()
    {
        ApplicationLifecycleManager.GoToOptionsMenu();
    }

    public void goToGame()
    {
        ApplicationLifecycleManager.GoToTTTGame();
    }
}
