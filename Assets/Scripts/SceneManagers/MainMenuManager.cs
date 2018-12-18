using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour {

    //Play button leads to options menu for choosing grid size
	public void goToOptionsMenu()
    {
        ApplicationLifecycleManager.GoToOptionsMenu();
    }

}
