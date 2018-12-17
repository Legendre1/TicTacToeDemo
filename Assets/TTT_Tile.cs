using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TTT_Tile : MonoBehaviour {

    #region Classes and Enums

    public enum TileState
    { 
        NONE = -1,
        PLAYER_1 = 0,//chosen so that they map to the index of m_player_mark_sprites
        PLAYER_2 = 1
    }

    #endregion

    #region Member Vars

    public Image m_mark_image;
    public PulseButtonScale m_scale_pulser;

    private TileState m_input_state;
    private TileState m_tilemark_state;
    private Sprite[] m_player_mark_sprites;

    private TTTGameManager m_game_manager;

    #endregion

    private void Start()
    {
        //cache a ref for later use
        m_game_manager = TTTGameManager.GetInstance();
    }

    #region Public Access

    public TileState getTileMarkState()
    {
        return m_tilemark_state;
    }

    public void assignPlayerMarks(Sprite player_1_mark, Sprite player_2_mark)
    {
        m_player_mark_sprites = new Sprite[2];
        m_player_mark_sprites[0] = player_1_mark;
        m_player_mark_sprites[1] = player_2_mark;

        initializeTile();
    }

    public void setInputState(TTTGameManager.GameState game_state)
    { 
        if(game_state == TTTGameManager.GameState.Player1_Turn)
        {
            m_input_state = TileState.PLAYER_1;
        }
        else if (game_state == TTTGameManager.GameState.Player2_Turn)
        {
            m_input_state = TileState.PLAYER_2;
        }
        else if (game_state == TTTGameManager.GameState.GameOver)
        {
            //the game has ended, do not accept further input
            m_input_state = TileState.NONE;
        }
        else
        {
            Debug.LogError("Improper game state signaled to game tile: " + game_state);
        }
    }

    #endregion

    #region Button Press Method

    public void onTileButtonPressed()
    {
        if(m_tilemark_state != TileState.NONE)
        {
            Debug.LogError("Cannot choose this tile, it has already been chosen.");
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
                Sprite marking_sprite = m_player_mark_sprites[(int)m_input_state];

                setMarkImageSprite(marking_sprite);
                m_tilemark_state = m_input_state;

                //inform the game that the user made input and marked his tile
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
