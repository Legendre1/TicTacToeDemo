using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TTT_Tile : MonoBehaviour {

    #region Classes and Enums

    public enum TileState
    { 
        NONE = -1,
        PLAYER_1 = 0,//indices chosen so that they match up to TTTGameManager.PlayerIndex
        PLAYER_2 = 1
    }

    #endregion

    #region Member Vars

    public Image m_mark_image;
    public PulseButtonScale m_scale_pulser;

    private TileState m_input_state;
    private TileState m_tilemark_state;
    private Sprite[] m_player_mark_sprites;

    private TTT_GameManager m_game_manager;

    private int m_row_index;
    private int m_column_index;

    #endregion

    private void Start()
    {
        //cache a ref for later use
        m_game_manager = TTT_GameManager.GetInstance();
    }

    #region Public Access

    public TileState getTileMarkState()
    {
        return m_tilemark_state;
    }

    public void assignGridIndices(int grid_x, int grid_y)
    {
        m_column_index = grid_x;
        m_row_index = grid_y;
    }

    public void assignPlayerMarks(Sprite player_1_mark, Sprite player_2_mark)
    {
        m_player_mark_sprites = new Sprite[2];
        m_player_mark_sprites[(int)TTT_GameManager.PlayerIndex.Player1] = player_1_mark;
        m_player_mark_sprites[(int)TTT_GameManager.PlayerIndex.Player2] = player_2_mark;

        initializeTile();
    }

    public void setInputState(TTT_GameManager.GameState game_state)
    { 
        if(game_state == TTT_GameManager.GameState.Player1_Turn)
        {
            m_input_state = TileState.PLAYER_1;
        }
        else if (game_state == TTT_GameManager.GameState.Player2_Turn)
        {
            m_input_state = TileState.PLAYER_2;
        }
        else if (game_state == TTT_GameManager.GameState.GameOver)
        {
            //the game has ended, do not accept further input
            m_input_state = TileState.NONE;
        }
        else
        {
            Debug.LogError("Improper game state signaled to game tile: " + game_state);
        }
    }

    public void clearTile()
    {
        initializeTile();
        clearMarkImageSprite();
    }

    #endregion

    #region Button Press Method

    public void onTileButtonPressed()
    {
        if(m_tilemark_state != TileState.NONE)
        {
            Debug.LogError("Cannot choose this tile, it has already been marked.");
        }
        else
        {
            if (m_input_state == TileState.NONE)
            {
                Debug.Log("This tile is not ready for input");
            }
            else
            {
                //input is valid, set the persistent tilemark_state to match the current players mark and input state
                Sprite marking_sprite = m_player_mark_sprites[(int)m_input_state];//[(int)m_input_state is equivalent to the index of the player who made this selection

                setMarkImageSprite(marking_sprite);
                m_tilemark_state = m_input_state;

                //inform the game that the user made input and marked his tile
                m_game_manager.recordMoveInHistory((int)m_input_state, m_column_index, m_row_index);
                m_game_manager.playerTileChoiceComplete();
            }
        }
    }

    #endregion

    #region Private Utility Methods

    private void initializeTile()
    {
        m_input_state = TileState.NONE;
        m_tilemark_state = TileState.NONE;
    }

    private void setMarkImageSprite(Sprite mark_sprite)
    {
        m_mark_image.color = Color.white;
        m_mark_image.sprite = mark_sprite;
    }

    private void clearMarkImageSprite()
    {
        m_mark_image.color = Color.clear;
    }

    #endregion
}
