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
        Win_Player1,
        Win_Player2,
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

    public UI_Manager m_ui_manager;
    public Modal_MarkSelector m_mark_selector_modal;

    private GameState m_gamestate;
    private TTT_Gameboard m_gameboard;

    private Sprite[] m_player_mark_sprites;

    //Constants
    private const string MAKE_MOVE_TEXT = "Make your move, Player {0}";
    private const int NUMBER_OF_PLAYERS = 2;

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
        m_player_mark_sprites = new Sprite[NUMBER_OF_PLAYERS];
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
        Debug.Log("Selecting mark for player " + player_num);
        if(player_num == 1)
        {
            m_gamestate = GameState.Player1_SelectMark;
            m_mark_selector_modal.triggerModal(player_num);
        }
        else if(player_num == 2)
        {
            m_gamestate = GameState.Player2_SelectMark;
            Sprite player_1_sprite = m_player_mark_sprites[(int)UI_Manager.PlayerUIIndex.Player1];
            m_mark_selector_modal.triggerModal(player_num, player_1_sprite);
        }
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
        m_gameboard.setPlayerMarkDefinitions( 
            m_player_mark_sprites[(int)UI_Manager.PlayerUIIndex.Player1],
            m_player_mark_sprites[(int)UI_Manager.PlayerUIIndex.Player2]);
    }

    #endregion

    #region Public State Methods
    //these methods respond to outside signals that the game state needs to change

    public void reportSelectedMark(Sprite selected_sprite)
    {
        Debug.Log("Selected sprite " + selected_sprite);
        //Modal reports that a players selection of marking sprite was made. Advance gamestate.
        if (m_gamestate == GameState.Player1_SelectMark)
        {
            m_player_mark_sprites[(int)UI_Manager.PlayerUIIndex.Player1] = selected_sprite;
            //player 1 has selected, ask for player 2s choice
            selectMarkForPlayer(2);
        }
        else if (m_gamestate == GameState.Player2_SelectMark)
        {
            m_player_mark_sprites[(int)UI_Manager.PlayerUIIndex.Player2] = selected_sprite;
            //player 2 has selected, begin the game
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
        else if (m_gamestate == GameState.Player2_Turn)
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
            m_ui_manager.triggerAnimatedText(UI_Manager.PlayerUIIndex.Player1, string.Format(MAKE_MOVE_TEXT,
                                                player_num), m_player_mark_sprites[(int)UI_Manager.PlayerUIIndex.Player1]);
        }
        else if (player_num == 2)
        {
            m_gamestate = GameState.Player2_Turn;
            m_ui_manager.triggerAnimatedText(UI_Manager.PlayerUIIndex.Player2, string.Format(MAKE_MOVE_TEXT, 
                                                player_num), m_player_mark_sprites[(int)UI_Manager.PlayerUIIndex.Player2]);
        }

        m_gameboard.setTilesToAcceptInput(m_gamestate);
    }

    private void gameOver(GameCompletion completion)
    {
        //end the game, tell the tiles to stop accepting input
        m_gamestate = GameState.GameOver;
        m_gameboard.setTilesToAcceptInput(m_gamestate);


        if(completion == GameCompletion.Win_Player1 || completion == GameCompletion.Win_Player2)
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
