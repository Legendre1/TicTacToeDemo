using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Modal_MarkSelector : Modal_Base {

    public Text m_selecting_text;
    public Toggle[] m_toggles;
    public GameObject m_completion_button_go;//"go" means "game object" here-not the button behavior itself

    //these must be set in the editor in indices which match up to the indices of int args passed to "selectionMade"
    public Sprite[] m_mark_sprites;

    private int m_player_selecting;
    private int m_selected_sprite_index;
    private const string SELECTION_TEXT = "Player {0} -     Choose Your Mark";

    public void triggerModal(int player_to_select, Sprite dissallowed_sprite = null)
    {
        m_player_selecting = player_to_select;
        setForSelectingPlayer(m_player_selecting);

        initializeSelectionDisplay();
        if(dissallowed_sprite != null)
        {
            int dissallowed_index = getIndexOfSprite(dissallowed_sprite);
            Debug.Log("Disabling mark " + dissallowed_index);
            disableMarkChoice(dissallowed_index);
        }
        m_animator.SetBool("active", true);

        //Set a listener for the toggle switches
        for(int n = 0; n < m_toggles.Length; n++)
        {
            Toggle t = m_toggles[n];
            int index = n;
            t.onValueChanged.AddListener((value) =>
            {
                if (value)
                {
                    selectionMade(index);
                }
            }
            );
        }
    }



    public void selectionMade(int selection)
    {
        Debug.Log("Selection Made " + selection);
        m_selected_sprite_index = selection;
        m_completion_button_go.SetActive(true);
    }

    public void selectionComplete()
    {
        m_animator.SetBool("active", false);
    }

    public void onExitAnimationCompleted()
    {
        TTTGameManager.GetInstance().reportSelectedMark(m_mark_sprites[m_selected_sprite_index]);
    }

    #region Internal Utility

    private void disableMarkChoice(int choice_index)
    {
        m_toggles[choice_index].gameObject.SetActive(false);
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
            m_toggles[n].gameObject.SetActive(true);//in case they were disabled in a previous game
            m_toggles[n].isOn = false;
        }
    }

    private int getIndexOfSprite(Sprite s)
    {
        for (int n = 0; n < m_mark_sprites.Length; n++)
        {
            if (s == m_mark_sprites[n])
            {
                return n;
            }
        }

        Debug.LogError("Sprite index not found");
        return -1;
    }

    #endregion

}
