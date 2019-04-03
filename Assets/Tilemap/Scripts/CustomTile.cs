using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ExtendedTilemap
{
    [CreateAssetMenu]
    public class CustomTile : TileBase
    {
        public Sprite tileSprite;
        public bool isWalkable;

        public override void RefreshTile(Vector3Int location, ITilemap tilemap)
        {
            base.RefreshTile(location, tilemap);
        }

        public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(location, tilemap, ref tileData);
            tileData.sprite = tileSprite;
        }
    }
}
