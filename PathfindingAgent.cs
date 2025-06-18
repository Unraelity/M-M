using System.Collections.Generic;
using UnityEngine;

public class PathfindingAgent : MonoBehaviour 
{

    // class for holding agent data on each Tile
    private class AgentPathData {
        public int gCost;     // cost from the start node
        public int hCost;     // cost to the goal
        public int fCost => gCost + hCost;     // total cost
        public Tile parent;     // parent tile in the path
    }

    private const int REGULAR_COST = 10;
    private const int DIAGONAL_COST = 14;

    [SerializeField] private bool drawPath = false;

    private Tile[,] grid;
    private Dictionary<Tile, AgentPathData> agentPathData;
    private Vector2[] directions = {
        new Vector2(1f, 0),
        new Vector2(-1f, 0),
        new Vector2(0, 1f),
        new Vector2(0, -1f),
        new Vector2(1f, 1f),
        new Vector2(-1f, 1f),
        new Vector2(1f, -1f),
        new Vector2(-1f, -1f)
    };

    private void Start() {

        grid = AStarGrid.Instance.Grid;
        // if grid is null, subscribe to the SetGrid Action
        if (grid == null)
        {
            AStarGrid.Instance.GridComplete += SetGrid;
        }
    }

    private void SetGrid(Tile[,] grid) {
        this.grid = grid;
    }

    public List<Tile> FindPath(Vector2 targetPos)
    {
        if (grid == null)
        {
            return null;
        }

        Vector2Int startingPosIndex = AStarGrid.Instance.MapToCoordinates(transform.position);
        Vector2Int targetPosIndex = AStarGrid.Instance.MapToCoordinates(targetPos);

        //Debug.Log("Starting Pos Inded:" + fixedStartingPos.x + ", " + fixedStartingPos.y);
        //Debug.Log("Target Pos Index:" + fixedTargetPos.x + ", " + fixedTargetPos.y);

        Tile startTile = grid[startingPosIndex.x, startingPosIndex.y];
        Tile targetTile = grid[targetPosIndex.x, targetPosIndex.y];

        //Debug.Log("Starting Pos:" + startTile.Position);
        //Debug.Log("Target Pos:" + targetTile.Position);

        if (!CheckInBounds(targetTile.Position))
        {
            Debug.LogWarning("Attempting to get to tile outside bounds");
            return null;
        }

        List<Tile> openList = new List<Tile>();
        HashSet<Tile> closedList = new HashSet<Tile>();
        agentPathData = new Dictionary<Tile, AgentPathData>();

        openList.Add(startTile);

        SetAgentPathData(startTile, 0, CalculateHeuristic(startTile, targetTile), null);

        while (openList.Count > 0) {

            Tile currentTile = GetTileWithLowestCost(openList);

            if (currentTile == targetTile) 
            {
                //Debug.Log("Path found");
                List<Tile> path = RetracePath(startTile, targetTile);

                return path;
            }

            openList.Remove(currentTile);
            closedList.Add(currentTile);
            
            foreach (Tile neighbor in GetNeighbors(currentTile)) {

                neighbor.UpdateTile(gameObject);

                // if spot already checked or tile is blocked, check next tile
                if (closedList.Contains(neighbor) || neighbor.IsBlocked)
                    continue;

                // add the GCost of current spot by 1 and add to neighbor tiles
                Vector2 direction = neighbor.Position - currentTile.Position;

                int movementCost = REGULAR_COST;

                // if is diagonal set cost to diagonal cost
                if ((Mathf.Abs(direction.x) > 0) && (Mathf.Abs(direction.y) > 0)) {
                    movementCost = DIAGONAL_COST;
                }

                // add the GCost of current spot by 1 and add to neighbor tiles
                int tentativeGCost = agentPathData[currentTile].gCost + movementCost;

                // set new data for path and add to list if the cost to tile is smaller than previously stored value
                if ((!agentPathData.ContainsKey(neighbor)) || (tentativeGCost < agentPathData[neighbor].gCost)) {

                    SetAgentPathData(neighbor, tentativeGCost, CalculateHeuristic(neighbor, targetTile), currentTile);

                    if (!openList.Contains(neighbor))
                        openList.Add(neighbor);
                }
            }
        }

        //Debug.LogWarning("Cannot find path to target");
        return null;
    }

    // store pathfinding data for the agent in the dictionary
    private void SetAgentPathData(Tile tile, int gCost, int hCost, Tile parent) {
        
        if (!agentPathData.ContainsKey(tile)) {
            agentPathData[tile] = new AgentPathData();
        }

        agentPathData[tile].gCost = gCost;
        agentPathData[tile].hCost = hCost;
        agentPathData[tile].parent = parent;
    }

    int CalculateHeuristic(Tile tileA, Tile tileB) {

        int dx = Mathf.Abs((int)(tileA.Position.x - tileB.Position.x));
        int dy = Mathf.Abs((int)(tileA.Position.y - tileB.Position.y));
        // combination of Manhattan, Euclidean and Didagonal grid formula
        return 10 * (dx + dy) + (4 * Mathf.Min(dx, dy));
    }

    // add tiles to list by iterating through the parents
    private List<Tile> RetracePath(Tile start, Tile end) {

        List<Tile> path = new List<Tile>();
        Tile currentTile = end;

        while (currentTile != start) {
            path.Add(currentTile);
            currentTile = agentPathData[currentTile].parent;
        }

        path.Reverse();

        /*
        foreach (Tile tile in path) {
            Debug.Log(tile.Position);
        }
        */

        if (drawPath) {
            DrawPath(path);
        }

        return path;
    }

    // get tiles around passed through tile (8 directional)
    private List<Tile> GetNeighbors(Tile tile) {

        List<Tile> neighbors = new List<Tile>();
        float tileSize = AStarGrid.Instance.TileSize;

        foreach (Vector2 dir in directions) {

            Vector2 currNeighbor = new Vector2(tile.Position.x + (tileSize * dir.x), tile.Position.y + (tileSize * dir.y));
            if (CheckInBounds(currNeighbor)) {

                Vector2Int currNeighborIndex = AStarGrid.Instance.MapToCoordinates(currNeighbor);
                neighbors.Add(grid[currNeighborIndex.x, currNeighborIndex.y]);
            }
        }

        return neighbors;
    }

    // check if tile position is inside AStar grid bounds
    private bool CheckInBounds(Vector2 position) {
        return (position.x < AStarGrid.Instance.UpperBounds.x) && (position.x > AStarGrid.Instance.LowerBounds.x) && (position.y < AStarGrid.Instance.UpperBounds.y) && (position.y > AStarGrid.Instance.LowerBounds.y);
    }
    
    // search for tile with lowest f cost
    private Tile GetTileWithLowestCost(List<Tile> openList) {

        Tile lowestCostTile = openList[0];

        foreach (Tile tile in openList) {
            
            if (agentPathData[tile].fCost < agentPathData[lowestCostTile].fCost) {
                lowestCostTile = tile;
            }
        }

        return lowestCostTile;
    }

    private static void DrawPath(List<Tile> path) {
        
        for (int i = 0; i < path.Count - 1; i++) {
            Debug.DrawLine(new Vector3(path[i].Position.x, path[i].Position.y, 0), new Vector3(path[i + 1].Position.x, path[i + 1].Position.y, 0), Color.blue);
        }
    }
    
}
