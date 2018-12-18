using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTT_GameManager : MonoBehaviour {

    #region Enums, Delegates, Classes

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

    public enum PlayerIndex
    {
        Player1 = 0,
        Player2 = 1
    }

    public delegate void VoidCallback();

    #endregion

    #region Singleton Access

    private static TTT_GameManager s_instance;

    public static TTT_GameManager GetInstance()
    {
        return s_instance;
    }

    #endregion

    #region Member Vars

    public UI_Manager m_ui_manager;

    public string[] m_player_names;//these are set to the strings "Player 1" and "Player 2" in the scene.

    private GameState m_gamestate;
    private TTT_Gameboard m_gameboard;

    private Sprite[] m_player_mark_sprites;

    //Move history
    private GameHistory m_game_history;

    //Constants
    private const string MAKE_MOVE_TEXT = "Make your move, {0}";

    private const string PLAY_AGAIN_TITLE = "Play Again!";
    private const string PLAY_AGAIN_DESCRIPTION = "Would you like to choose new marks for this game?";
    private const string YES = "Yes";
    private const string NO = "No";

    private const int NUMBER_OF_PLAYERS = 2;
    private const float WIN_GAME_OVER_DELAY = 1.5f;//postgame delay for final board evaluation before showing the game over menu
    private const float DRAW_GAME_OVER_DELAY = 0.5f;

    #endregion

    #region Mono Methods

    // Use this for initialization
    void Start()
    {
        //push instance to static
        s_instance = this;

        //get helper script references
        m_gameboard = TTT_Gameboard.GetInstance();

        //initialize the game history data
        m_game_history = new GameHistory();
        m_game_history.Initialize();

        //begin by selecting the marks each player will use
        m_player_mark_sprites = new Sprite[NUMBER_OF_PLAYERS];
        selectPlayerMarks();
    }

    #endregion



    #region Mark Selection 

    private void selectPlayerMarks()
    {
        //begin by having player 1 select a mark to use in this game
        selectMarkForPlayer(PlayerIndex.Player1);
    }



    private void selectMarkForPlayer(PlayerIndex player_index)
    {
        if(player_index == PlayerIndex.Player1)
        {
            m_gamestate = GameState.Player1_SelectMark;
            m_ui_manager.presentMarkSelectionModal(m_player_names[(int)player_index]);
        }
        else if(player_index == PlayerIndex.Player2)
        {
            m_gamestate = GameState.Player2_SelectMark;
            Sprite player_1_sprite = m_player_mark_sprites[(int)PlayerIndex.Player1];
            m_ui_manager.presentMarkSelectionModal(m_player_names[(int)player_index], player_1_sprite);
        }
    }

    #endregion

    #region Game Startup

    private void beginGame()
    {
        constructGameBoard();
        //begin the first turn for player 1
        getInputFromPlayer(PlayerIndex.Player1);
    }

    private void constructGameBoard()
    {
        int board_size = UserdataManager.GetGridSize();
        m_gameboard.constructGameBoard(board_size);
        m_gameboard.setPlayerMarkDefinitions( 
            m_player_mark_sprites[(int)PlayerIndex.Player1],
            m_player_mark_sprites[(int)PlayerIndex.Player2]);
    }

    #endregion

    #region Public State Methods
    //these methods respond to outside signals that the game state needs to change

    public void reportSelectedMark(Sprite selected_sprite)
    {
        //Modal reports that a players selection of marking sprite was made. Advance gamestate.
        if (m_gamestate == GameState.Player1_SelectMark)
        {
            m_player_mark_sprites[(int)PlayerIndex.Player1] = selected_sprite;
            //player 1 has selected, ask for player 2s choice
            selectMarkForPlayer(PlayerIndex.Player2);
        }
        else if (m_gamestate == GameState.Player2_SelectMark)
        {
            m_player_mark_sprites[(int)PlayerIndex.Player2] = selected_sprite;
            //player 2 has selected, begin the game
            beginGame();
        }
        else
        {
            Debug.LogError("Improper gamestate at time of mark selection");
        }
    }

    public void recordMoveInHistory(int player_index, int column_index, int row_index)
    {
        m_game_history.AppendMove(player_index, column_index, row_index);
    }

    public void playerTileChoiceComplete()
    {
        //A gameboard tile reports that a mark selection was made. Check to see if the game is over
        GameCompletion completion = m_gameboard.checkForGameCompletion();
        if (completion != GameCompletion.Incomplete)
        {
            gameOver(completion);
            return;
        }

        //Game is not yet over, so advance the gamestate to get input from the other player.
        if (m_gamestate == GameState.Player1_Turn)
        {
            getInputFromPlayer(PlayerIndex.Player2);
        }
        else if (m_gamestate == GameState.Player2_Turn)
        {
            getInputFromPlayer(PlayerIndex.Player1);
        }
        else
        {
            Debug.LogError("Improper gamestate at time of board mark choice");
        }
    }

    public void playAgain()
    {
        //user requested another game. show a binary choice prompt which allows 
        //the users to either play again using the same marks, or choose new marks

        VoidCallback play_with_same_marks = playAgainUsingSameMarks;
        VoidCallback play_with_new_marks = chooseNewMarksAndPlayAgain;

        m_ui_manager.presentBinaryChoiceModal(PLAY_AGAIN_TITLE, PLAY_AGAIN_DESCRIPTION, NO, YES, playAgainUsingSameMarks, chooseNewMarksAndPlayAgain);

        //clean up the game board
        m_gameboard.clearBoard();
    }

    #endregion

    #region Gamestate Lifecycle Utility

    private void getInputFromPlayer(PlayerIndex player_index)
    {
        if(player_index == PlayerIndex.Player1)
        {
            m_gamestate = GameState.Player1_Turn;
            PlayerIndex player_1_index = PlayerIndex.Player1;
            m_ui_manager.triggerAnimatedText((int)player_index, string.Format(MAKE_MOVE_TEXT,
                                                m_player_names[(int)player_1_index]), m_player_mark_sprites[(int)PlayerIndex.Player1]);
        }
        else if (player_index == PlayerIndex.Player2)
        {
            m_gamestate = GameState.Player2_Turn;
            PlayerIndex player_2_index = PlayerIndex.Player2;
            m_ui_manager.triggerAnimatedText((int)player_2_index, string.Format(MAKE_MOVE_TEXT, 
                                                m_player_names[(int)player_2_index]), m_player_mark_sprites[(int)PlayerIndex.Player2]);
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
            int winner_index = -1;
            if(completion == GameCompletion.Win_Player1)
            {
                winner_index = (int)PlayerIndex.Player1;
            }
            else 
            {
                winner_index = (int)PlayerIndex.Player2;
            }
            Sprite winners_sprite = m_player_mark_sprites[winner_index];

            m_ui_manager.presentGameOverModal(true, WIN_GAME_OVER_DELAY, m_player_names[winner_index], winners_sprite);
        }
        else if(completion == GameCompletion.Draw)
        {
            m_ui_manager.presentGameOverModal(false,DRAW_GAME_OVER_DELAY);
        }
        else
        {
            Debug.LogError("Invalid completion state at game over");
        }
    }

    private void playAgainUsingSameMarks()
    {
        beginGame();
    }

    private void chooseNewMarksAndPlayAgain()
    {
        selectPlayerMarks();
    }

    #endregion


#if UNITY_EDITOR
    #region Print Move History (Editor Only)
    public void printMoveHistory()
    {
        m_game_history.PrintWinHistoryToConsole();
    }
    #endregion
#endif

}
