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

    public enum GameCompletion
    { 
        Incomplete,
        Win,
        Draw
    }

    #endregion

    #region Singleton Access

    private static TTTGameManager s_instance;

    public static TTTGameManager GetInstance()
    {
        return s_instance;
    }

    #endregion

    #region Member Vars

    public Modal_MarkSelector m_mark_selector_modal;

    private GameState m_gamestate;
    private TTT_Gameboard m_gameboard;

    #endregion

    #region Mono Methods

    // Use this for initialization
    void Start()
    {
        //push instance to static
        s_instance = this;

        //get helper script references
        m_gameboard = TTT_Gameboard.GetInstance();

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

    #region Game Startup

    private void beginGame()
    {
        constructGameBoard();
        //begin the first turn for player 1
        getInputFromPlayer(1);
    }

    private void constructGameBoard()
    {
        int board_size = UserdataManager.GetGridSize();
        m_gameboard.constructGameBoard(board_size);
    }

    #endregion

    #region Public State Methods

    //these methods respond to outside signals that the game state needs to change
    public void playerSelectionComplete()
    {
        //Modal reports that a mark selection was made. Advance gamestate.
        if (m_gamestate == GameState.Player1_SelectMark)
        {
            selectMarkForPlayer(2);
        }
        else if (m_gamestate == GameState.Player2_SelectMark)
        {
            beginGame();
        }
        else
        {
            Debug.LogError("Improper gamestate at time of mark selection");
        }
    }

    public void playerTileChoiceComplete()
    {
        //A gameboard tile reports that a mark selection was made. Check to see if the game is over
        GameCompletion completion = m_gameboard.checkForGameCompletion();
        if (completion != GameCompletion.Incomplete)
        {
            Debug.Log("Game completed");
            gameOver(completion);
            return;
        }

        //Game is not yet over, so advance the gamestate to get input from the other player.
        if (m_gamestate == GameState.Player1_Turn)
        {
            getInputFromPlayer(2);
        }
        else if (m_gamestate == GameState.Player2_SelectMark)
        {
            getInputFromPlayer(1);
        }
        else
        {
            Debug.LogError("Improper gamestate at time of board mark choice");
        }
    }

    #endregion

    #region Gamestate Lifecycle Utility

    private void getInputFromPlayer(int player_num)
    {
        if (player_num == 1)
        {
            m_gamestate = GameState.Player1_Turn;
        }
        else if (player_num == 2)
        {
            m_gamestate = GameState.Player2_Turn;
        }

        m_gameboard.setTilesToAcceptInput(m_gamestate);
    }

    private void gameOver(GameCompletion completion)
    { 
        if(completion == GameCompletion.Win)
        {
            Debug.Log("WIN");
        }
        else if(completion == GameCompletion.Draw)
        {
            Debug.Log("DRAW");
        }
        else
        {
            Debug.LogError("Invalid completion state at game over");
        }
    }

    #endregion

}
