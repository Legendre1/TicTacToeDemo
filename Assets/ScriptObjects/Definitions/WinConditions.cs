using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Win Conditions")]
public class WinConditions : ScriptableObject {

    [System.Serializable]
	public class TileSequence
    {
        public int[] Tiles;
    }

    public TileSequence[] WinningTileSequences;
}
