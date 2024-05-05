using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class PlacementSystem : MonoBehaviour
{
    // visualize what position in being selected
    [SerializeField] private GameObject mouseIndicator;

    // unity grid object por position snapping
    [SerializeField] private Grid grid;
    
    [SerializeField] private InputManager inputManager;

    // SO database to store buildings and their attributes
    [SerializeField] private ObjectsDatabaseSO databaseSO;
    private int selectetObjectIndex = -1;

    [SerializeField] private GameObject gridVisualization;

    // stores  the grid data and occupied cells
    private GridData buildingData;
    
    private List<GameObject> placedGameObjects = new();

    [SerializeField] private PreviewSystem previewSystem;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;
    

    private void Start()
    {
        StopPlacement();
        buildingData = new();
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
        previewSystem.StartShowingPlacementPreview(
            databaseSO.objectsData[selectetObjectIndex].Prefab, 
            databaseSO.objectsData[selectetObjectIndex].Size);
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

        bool placementValidity = CheckPlacementValidity(gridPosition, selectetObjectIndex);
        if (placementValidity == false)
        {
            return;
        }


        GameObject newObject = Instantiate(databaseSO.objectsData[selectetObjectIndex].Prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);
        placedGameObjects.Add(newObject);
        buildingData.AddObjectAt(gridPosition, databaseSO.objectsData[selectetObjectIndex].Size,
            databaseSO.objectsData[selectetObjectIndex].ID,
            placedGameObjects.Count -1);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectetObjectIndex)
    {
        return buildingData.CanPlaceObjectAt(gridPosition, databaseSO.objectsData[this.selectetObjectIndex].Size);
    }

    private void StopPlacement()
    {
        lastDetectedPosition = Vector3Int.zero;
        selectetObjectIndex = -1;
        gridVisualization.SetActive(false);
        previewSystem.StopShowingPreview();
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

        // if selecting same position as before, skip calculations
        if (lastDetectedPosition != gridPosition)
        {   
            bool placementValidity = CheckPlacementValidity(gridPosition, selectetObjectIndex);
            previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
        
            mouseIndicator.transform.position = mousePosition;
            lastDetectedPosition = gridPosition;
        }
    }
}
    
