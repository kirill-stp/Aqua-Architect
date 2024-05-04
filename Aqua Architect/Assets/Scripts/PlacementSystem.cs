using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    // visualize what position in being selected
    [SerializeField] private GameObject mouseIndicator;
    [SerializeField] private GameObject cellIndicator;

    [SerializeField] private Grid grid;
    
    [SerializeField] private InputManager inputManager;

    private void Update()
    {
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }
}
    
