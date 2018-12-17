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

    //script object win conditions
    public WinConditions m_3x3_winconditions;
    public WinConditions m_4x4_winconditions;

    private WinConditions m_active_win_conditions;
    private List<TTT_Tile> m_board_tiles;
    private Image m_board_image;
    private RectTransform m_board_rectform;
    private TTT_Tile[] m_possible_winning_tiles;//array of tiles to be examined for a win and reused

    //Constants
    private const float WIN_PULSE_DELAY = 0.4f;//the timing between pulses of tiles showing a winning sequence

    #endregion

    #region Mono Methods

    private void Start()
    {
        s_instance = this;
        m_board_image = GetComponent<Image>();
        m_board_rectform = GetComponent<RectTransform>();
    }

    #endregion

    #region Public Access Methods

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

        //define the win conditions for this board
        //define win-examination array for reuse based on grid size
        m_possible_winning_tiles = new TTT_Tile[grid_size];
        if (grid_size == 3)
        {
            m_active_win_conditions = m_3x3_winconditions;
        }
        else if (grid_size == 4)
        {
            m_active_win_conditions = m_4x4_winconditions;
        }
        else 
        {
            Debug.LogError("No win conditions found for grid size " + grid_size);
        }

#if UNITY_EDITOR
        verifyWinConditions(grid_size);
#endif
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
        int winning_player = searchForWinningPlayer();
        if(winning_player >= 0)
        {
            highlightWinningSequence();

            if (winning_player ==(int)UI_Manager.PlayerUIIndex.Player1)
            {
                return TTTGameManager.GameCompletion.Win_Player1;
            }
            else if(winning_player == (int)UI_Manager.PlayerUIIndex.Player2)
            {
                return TTTGameManager.GameCompletion.Win_Player2;
            }
        }

        //search for a draw (full board)
        if(isBoardFull())
        {
            return TTTGameManager.GameCompletion.Draw;
        }

        return TTTGameManager.GameCompletion.Incomplete;
    }

    #endregion

    #region Win/Draw-Condition Checking

    private int searchForWinningPlayer()
    {
        //if a player has won, this method returns that players (0-based) index. 
        //Otherwise it returns -1
        foreach (WinConditions.TileSequence tile_sequence in m_active_win_conditions.WinningTileSequences)
        {
            int winning_player = searchSequenceForWin(tile_sequence);
            if(winning_player >= 0)
            {
                return winning_player;
            }
        }

        return -1;
    }

    private int searchSequenceForWin(WinConditions.TileSequence tile_sequence)
    { 
        //returns index of winning player (or -1)

        //build up the (temp, reused) potential winning tile sequence for examination
        for(int n = 0; n < tile_sequence.Tiles.Length; n++)
        {
            m_possible_winning_tiles[n] = m_board_tiles[tile_sequence.Tiles[n]];
        }

        //first, look for empty tiles. if any tiles are empty, there cannot be a win
        for (int n = 0; n < m_possible_winning_tiles.Length; n++)
        {
            if(m_possible_winning_tiles[n].getTileMarkState() == TTT_Tile.TileState.NONE)
            {
                return -1;
            }
        }

        //next, compare the first tile to all other tiles. if any fail to match, no winner
        TTT_Tile first_tile = m_possible_winning_tiles[0];
        for(int n = 1; n < m_possible_winning_tiles.Length; n++)
        {
            TTT_Tile compare_tile = m_possible_winning_tiles[n];
            if(first_tile.getTileMarkState() != compare_tile.getTileMarkState())
            {
                //not a match, no win possible here
                return -1;
            }
        }

        //all the tiles are marked and match one another. return the winners index
        if(first_tile.getTileMarkState() == TTT_Tile.TileState.PLAYER_1)
        {
            return (int)UI_Manager.PlayerUIIndex.Player1;
        }
        else if (first_tile.getTileMarkState() == TTT_Tile.TileState.PLAYER_2)
        {
            return (int)UI_Manager.PlayerUIIndex.Player2;
        }
        else
        {
            Debug.LogError("Winning tile mark does not belong to either player. Something went horribly wrong");
        }

        return -1;
    }

    private bool isBoardFull()
    { 
        foreach(TTT_Tile tile in m_board_tiles)
        { 
            if(tile.getTileMarkState() == TTT_Tile.TileState.NONE)
            {
                return false;
            }
        }
        return true;
    }


    #endregion

    #region Utility

    private void highlightWinningSequence()
    {
        //the last possible winning sequence is the one that registered a win, use it again here
        for (int n = 0; n < m_possible_winning_tiles.Length; n++)
        {
            Debug.Log("Pulsing tile " + name);
            TTT_Tile winning_tile = m_possible_winning_tiles[n];
            winning_tile.m_scale_pulser.activatePulsingButton(n * WIN_PULSE_DELAY);
        }
    }

    private void showBoard()
    {
        m_board_image.color = Color.white;
    }

    private void hideBoard()
    {
        m_board_image.color = Color.clear;
    }

    #endregion

#if UNITY_EDITOR
    #region Editor Only Debugging methods

    private void verifyWinConditions(int grid_size)
    {
        Debug.Log("Verifying win conditions : " + m_active_win_conditions);
        //make sure that all conditions match the side length of the grid
        foreach(WinConditions.TileSequence tile_sequence in m_active_win_conditions.WinningTileSequences)
        { 

            if(tile_sequence.Tiles.Length != grid_size)
            {
                Debug.LogError("Win condition does not match grid size");
                return;
            }
        }
        Debug.Log("Win conditions match grid size!");
    }

    #endregion
#endif
}
