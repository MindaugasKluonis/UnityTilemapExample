using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ExtendedTilemap
{
    public class LevelTile
    {
        public Vector3Int LocalLocation { set; get; }
        public Vector3 WorldLocation { set; get; }
        public CustomTile Tile { set; get; }
        public Tilemap ParentTilemap { set; get; }

        //used for BFS
        public bool IsExplored { get; set; }
        public LevelTile ExploredFrom { get; set; }

        public bool IsWalkable()
        {
            return Tile.isWalkable;
        }

        public bool IsOccupied()
        {
            return false;
        }

        public bool IsAvailable()
        {
            //if character is not on the tile and if its walkable, then return true
            return true;
        }
    }
}
