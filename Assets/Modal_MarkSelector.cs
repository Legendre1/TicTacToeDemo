using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Modal_MarkSelector : Modal_Base {

    public Text m_selecting_text;
    public Toggle[] m_toggles;
    public GameObject m_completion_button_go;//"go" means "game object" here-not the button behavior itself

    private int m_player_selecting;
    private const string SELECTION_TEXT = "Player {0} -     Choose Your Mark";

    public void triggerModal(int player_to_select)
    {
        m_player_selecting = player_to_select;
        setForSelectingPlayer(m_player_selecting);

        initializeSelectionDisplay();
        m_animator.SetBool("active", true);
    }

    public void selectionMade(int selection)
    {
        Debug.Log("Chose " + selection);
        m_completion_button_go.SetActive(true);
    }

    public void selectionComplete()
    {
        m_animator.SetBool("active", false);
    }

    public void onExitAnimationCompleted()
    {
        TTTGameManager.GetInstance().playerSelectionComplete();
    }

    private void setForSelectingPlayer(int player_num)
    {
        m_selecting_text.text = string.Format(SELECTION_TEXT, player_num);
    }

    private void initializeSelectionDisplay()
    {
        resetToggles();
        m_completion_button_go.SetActive(false);
    }

    private void resetToggles()
    { 
        for(int n = 0; n < m_toggles.Length; n++)
        {
            m_toggles[n].isOn = false;
        }
    }

}
