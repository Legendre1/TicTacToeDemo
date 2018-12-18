using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameHistory : ScriptableObject {

    #region Child Data Classes

    [System.Serializable]
	public class PlayerMove
    {
        public PlayerMove(int index, int grid_x, int grid_y)
        {
            PlayerIndex = index;
            SelectedGridPosition = new GridPosition(grid_x, grid_y);
            MoveTime = DateTime.Now;
        }

        public int PlayerIndex;
        public GridPosition SelectedGridPosition;
        public DateTime MoveTime;
    }

    [System.Serializable]
    public class GridPosition
    {
        public GridPosition(int c, int r)//reversed as parameters to fit the more intuitive x,y ordering
        {
            Row = r;
            Column = c;
        }

        public int Row;
        public int Column;
    }

    #endregion

    //history of all moves taken
    public List<PlayerMove> MoveHistory;

    #region Public Methods

    public void Initialize()
    {
        MoveHistory = new List<PlayerMove>();
    }

    public void AppendMove(int player_index, int grid_x, int grid_y)
    {
        PlayerMove new_move = new PlayerMove(player_index, grid_x, grid_y);
        MoveHistory.Add(new_move);
    }

    public void PrintWinHistoryToConsole()
    { 
        //Debug command to allow a printout of the data for a completed game

        for(int n = 0; n < MoveHistory.Count; n++)
        {
            PlayerMove move = MoveHistory[n];
            Debug.Log("Move # " + n + " was made by player (index) " + move.PlayerIndex);
            Debug.Log("Grid position: column " + move.SelectedGridPosition.Column + ", row " + move.SelectedGridPosition.Row);
            Debug.Log("At Date/Time " + move.MoveTime);
        }
    }

    #endregion

}
