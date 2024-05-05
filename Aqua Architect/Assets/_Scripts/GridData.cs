using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    // stores all placed objects, key is EVERY position value is PlacementData instance
    private Dictionary<Vector3Int, PlacementData> placedObjects = new Dictionary<Vector3Int, PlacementData>();

    // adds object to the grid data storage
    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int id, int placedObjectIndex)
    {
        List<Vector3Int> positionsToOccupy = CalculatePositions(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionsToOccupy, id, placedObjectIndex);
        foreach (var pos in positionsToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
            {
                throw new Exception($"Dictionary already contains this cell position {pos}");
            }

            placedObjects[pos] = data;
        }
    }
    
    // returns a list of all grid positions object occupies 
    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVals = new();
        for (int i = 0; i < objectSize.x; i++)
        {
            for (int j = 0; j < objectSize.y; j++)
            {
                returnVals.Add(gridPosition + new Vector3Int(i, 0, j));
            }
        }

        return returnVals;
    }

    // checks if object can be placed on the grid
    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
            {
                return false;
            }
        }
        return true;
    }
}


// class to store Placement Data for a single building
public class PlacementData
{
    // all occupied positions on the grid
    public List<Vector3Int> occupiedPositions;
    // id of placed item
    public int ID { get; private set; }
    public int PacedObjectIndex { get; private set; }

    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int pacedObjectIndex)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PacedObjectIndex = pacedObjectIndex;
    }
}