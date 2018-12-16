using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTTGameManager : MonoBehaviour {

    #region Enums and Classes

    public enum GameState
    { 
        Player1_SelectMark,
        Player2_SelectMark,
        Player1_Turn,
        Player2_Turn,
        GameOver
    }

    #endregion

    #region Singleton Access

    private static TTTGameManager s_game_manager;

    public static TTTGameManager GetInstance()
    {
        return s_game_manager;
    }

    #endregion

    public Modal_MarkSelector m_mark_selector_modal;



    private GameState m_gamestate;


    #region Public Methods 

    public void playerSelectionComplete()
    { 
        //Modal reports that a mark selection was made. Advance gamestate.
        if(m_gamestate == GameState.Player1_SelectMark)
        {
            selectMarkForPlayer(2);
        }
        else if(m_gamestate == GameState.Player2_SelectMark)
        {
            beginGame();
        }
    }

    #endregion

    #region Mono Methods

    // Use this for initialization
    void Start () {
        //push instance to static
        s_game_manager = this;
        //begin by selecting the marks each player will use
        selectPlayerMarks();
    }

    #endregion

    #region Mark Selection 

    private void selectPlayerMarks()
    {
        //begin by having player 1 select a mark to use in this game
        selectMarkForPlayer(1);
    }

    private void selectMarkForPlayer(int player_num)
    {
        if(player_num == 1)
        {
            m_gamestate = GameState.Player1_SelectMark;
        }
        else if(player_num == 2)
        {
            m_gamestate = GameState.Player2_SelectMark;
        }

        m_mark_selector_modal.triggerModal(player_num);
    }

    #endregion

    private void beginGame()
    {
        Debug.Log("BEGIN GAME");
    }

}
