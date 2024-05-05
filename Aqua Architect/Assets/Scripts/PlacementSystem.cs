using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class PlacementSystem : MonoBehaviour
{
    // visualize what position in being selected
    [SerializeField] private GameObject mouseIndicator;
    [SerializeField] private GameObject cellIndicator;

    [SerializeField] private Grid grid;
    
    [SerializeField] private InputManager inputManager;

    [SerializeField] private ObjectsDatabaseSO databaseSO;
    private int selectetObjectIndex = -1;

    [SerializeField] private GameObject gridVisualization;

    private void Start()
    {
        StopPlacement();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        selectetObjectIndex = databaseSO.objectsData.FindIndex(data => data.ID == ID);
        if (selectetObjectIndex < 0)
        {
            Debug.LogError($"No ID found {ID}");
            return;
        }
        gridVisualization.SetActive(true);
        cellIndicator.SetActive(true);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.isPointerOverUI())
        {
            return;
        }
        
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        GameObject newObject = Instantiate(databaseSO.objectsData[selectetObjectIndex].Prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);
    }

    private void StopPlacement()
    {
        selectetObjectIndex = -1;
        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
    }

    private void Update()
    {
        if (selectetObjectIndex < 0)
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }
}
    
