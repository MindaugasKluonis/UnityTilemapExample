using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ExtendedTilemap
{
    public class LevelTiles : MonoBehaviour
    {
        public Tilemap tilemap;

        private Dictionary<Vector3, LevelTile> tiles;

        private void Awake()
        {
            InitializeLevelTiles();
        }

        private void Update()//ONLY FOR TESTING
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 worldPoint = new Vector3(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), 0);
                LevelTile resultTile;

                if(tiles.TryGetValue(worldPoint, out resultTile))
                {
                    Stack<LevelTile> path = GeneratePathToGoal(tiles[Vector3.zero], resultTile);

                    if (path != null)
                    {
                        while (path.Count > 0)
                        {
                            Debug.Log(path.Count);
                            LevelTile tile = path.Pop();
                            tile.ParentTilemap.SetTileFlags(tile.LocalLocation, TileFlags.None);//settting tile to dirty
                            tile.ParentTilemap.SetColor(tile.LocalLocation, Color.blue);
                        }
                    }
                }
            }
        }

        public void InitializeLevelTiles()
        {
            tiles = new Dictionary<Vector3, LevelTile>();

            foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
            {
                Vector3Int localPosition = new Vector3Int(pos.x, pos.y, pos.z);

                if (!tilemap.HasTile(localPosition)) continue;

                LevelTile tile = new LevelTile
                {
                    LocalLocation = localPosition,
                    WorldLocation = tilemap.CellToWorld(localPosition),
                    Tile = tilemap.GetTile<CustomTile>(localPosition),
                    ParentTilemap = tilemap
                };

                tiles.Add(tile.WorldLocation, tile);
            }
        }

        public int GetManhattanDistance(LevelTile tileOne, LevelTile tileTwo)
        {
            return Math.Abs(tileOne.LocalLocation.x - tileTwo.LocalLocation.x) + Math.Abs(tileOne.LocalLocation.y - tileTwo.LocalLocation.y);
        }

        public Stack<LevelTile> GeneratePathToGoal(LevelTile startTile, LevelTile goalTile)//BFS
        {
            Queue<LevelTile> queue = new Queue<LevelTile>();
            HashSet<LevelTile> visitedNodes = new HashSet<LevelTile>();

            queue.Enqueue(startTile);

            while (queue.Count != 0)
            {
                LevelTile currentNode = queue.Dequeue();

                if (currentNode == goalTile)
                {
                    Debug.Log("Found");
                    return GetPathToGoal(startTile, goalTile);
                }

                List<LevelTile> neighbours = GetTileNeighbours(currentNode);

                foreach (LevelTile tile in neighbours)
                {
                    if (!visitedNodes.Contains(tile))
                    {
                        visitedNodes.Add(tile);
                        tile.ExploredFrom = currentNode;
                        queue.Enqueue(tile);
                    }
                }
            }

            return null;
        }

        private Stack<LevelTile> GetPathToGoal(LevelTile startTile, LevelTile goalTile)
        {
            LevelTile curr = goalTile;
            Stack<LevelTile> path = new Stack<LevelTile>();

            while (curr != startTile)
            {
                path.Push(curr);
                curr = curr.ExploredFrom;
            }

            return path;
        }

        public List<LevelTile> GetTileNeighbours(LevelTile tile)
        {
            List<LevelTile> result = new List<LevelTile>();
            Vector3 parentPos = tile.WorldLocation;
            LevelTile currentTile = null;

            if (tiles.TryGetValue(parentPos + Vector3.up, out currentTile)) { result.Add(currentTile); }
            if (tiles.TryGetValue(parentPos + Vector3.down, out currentTile)) { result.Add(currentTile); }
            if (tiles.TryGetValue(parentPos + Vector3.left, out currentTile)) { result.Add(currentTile); }
            if (tiles.TryGetValue(parentPos + Vector3.right, out currentTile)) { result.Add(currentTile); }

            return result;
        }
    }

    
}