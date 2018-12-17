using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TTT_Gameboard : MonoBehaviour {
    //this is a helper class for TTTGameManager. It handles construction of and top down interface with the game "board" of selectable tiles

    #region Singleton Access

    private static TTT_Gameboard s_instance;

    public static TTT_Gameboard GetInstance()
    {
        return s_instance;
    }

    #endregion

    #region Member Vars

    //public, defined in scene
    public GameObject m_tile_prefab;
    public float m_tile_spacing;
    public float m_outer_board_buffer_width;

    private List<TTT_Tile> m_board_tiles;
    private Image m_board_image;
    private RectTransform m_board_rectform;

    #endregion

    private void Start()
    {
        s_instance = this;
        m_board_image = GetComponent<Image>();
        m_board_rectform = GetComponent<RectTransform>();
    }

    public void constructGameBoard(int grid_size)
    {
        //Resize the gameboard image to fit all tiles
        float board_side_length = (m_tile_spacing * (grid_size)) + (2 * m_outer_board_buffer_width);
        m_board_rectform.sizeDelta = new Vector2(board_side_length, board_side_length);
        showBoard();

        //Fill the board with tiles
        float half_anchor_width = m_tile_spacing * grid_size / 2;
        m_board_tiles = new List<TTT_Tile>();
        for(int n = 0; n < grid_size; n++)
        {
            for (int m = 0; m < grid_size; m++)
            {
                //this will construct the grid left to right, top to bottom (indices increase like a page of text)
                Vector2 tile_position = new Vector2(-half_anchor_width + ((n + 0.5f) * m_tile_spacing), half_anchor_width - ((m + 0.5f) * m_tile_spacing));

                //create tile, set position within grid
                GameObject new_tile_go = Instantiate(m_tile_prefab, transform);
                RectTransform rectform = new_tile_go.GetComponent<RectTransform>();
                rectform.anchoredPosition = tile_position;

                //add ref to active list for further access
                TTT_Tile new_tile = new_tile_go.GetComponent<TTT_Tile>();
                m_board_tiles.Add(new_tile);
            }
        }
    }

    public void setPlayerMarkDefinitions(Sprite player_1_sprite, Sprite player_2_sprite)
    {
        for (int n = 0; n < m_board_tiles.Count; n++)
        {
            m_board_tiles[n].assignPlayerMarks(player_1_sprite, player_2_sprite);
        }
    }

    public void setTilesToAcceptInput(TTTGameManager.GameState current_state)
    { 
        for(int n = 0; n < m_board_tiles.Count; n++)
        {
            m_board_tiles[n].setInputState(current_state);
        }
    }

    public TTTGameManager.GameCompletion checkForGameCompletion()
    {
        //this method examines the gameboard and returns true if a win or draw is detected.
        //search for a win by either player


        //search for a draw (full board


        return TTTGameManager.GameCompletion.Incomplete;
    }

    #region Utility

    private void showBoard()
    {
        m_board_image.color = Color.white;
    }

    private void hideBoard()
    {
        m_board_image.color = Color.clear;
    }

    #endregion
}
