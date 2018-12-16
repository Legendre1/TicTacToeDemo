using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuManager : MonoBehaviour {

    public Toggle m_4x4_toggle;

    private void Start()
    {
        //If the grid is set to 4x4, switch the toggle
        if(UserdataManager.GetGridSize() == 4)
        {
            m_4x4_toggle.isOn = true;
        }
    }

    public void goToMainMenu()
    {
        ApplicationLifecycleManager.GoToMainMenu();
    }

    public void SetGridTo3X3(bool toggled)
    { 
        if(toggled)
        {
            setGameGridSize(3);
        }
    }

    public void SetGridTo4X4(bool toggled)
    {
        if (toggled)
        {
            setGameGridSize(4);
        }
    }

    private void setGameGridSize(int grid_size)
    {
        UserdataManager.SetGridSize(grid_size);
    }
}
