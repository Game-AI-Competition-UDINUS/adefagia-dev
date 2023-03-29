using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using Adefagia.BattleMechanism;
using Adefagia.Collections;
using Adefagia.SelectObject;

namespace Adefagia.GridSystem
{
    public class GridManager : MonoBehaviour
    {
        public float gridLength = 1;
        public int gridSizeX, gridSizeY;

        public List<GridElement> listGridPrefab;

        private Dictionary<GridType, GridElement> _gridElements;
        
        private Select _select;

        // List All Grid
        private Grid[,] _listGrid;

        private void Awake()
        {
            StartCoroutine(InitializeGridManager());
            _select = GetComponent<Select>();
        }

        /*----------------------------------------------------------------------
         * Initialize Grid, their Neighbor 
         *----------------------------------------------------------------------*/
        private IEnumerator InitializeGridManager()
        {
            // Wait until GameState is Initialize
            while (BattleManager.gameState != GameState.Initialize)
            {
                yield return null;
            }
            
            // Init gridElements
            CreateGridElements();
            
            // Generate Grids
            GenerateGrids();

            // Set grid neighbor 
            SetNeighbors();
            
            BattleManager.ChangeGameState(GameState.Preparation);
        }

        private void CreateGridElements()
        {
            _gridElements = new Dictionary<GridType, GridElement>();
            var duplicateCount = 0;
            foreach (var gridElement in listGridPrefab)
            {
                try
                {
                    if (_gridElements.ContainsKey(gridElement.gridType)) throw new DictionaryDuplicate();
                    _gridElements.Add(gridElement.gridType, gridElement);
                }
                catch (DictionaryDuplicate)
                {
                    duplicateCount++;
                }
            }
            
            // duplicate error message
            if(duplicateCount > 0) Debug.LogWarning($"Duplicate {duplicateCount} Item.");
        }

        /*----------------------------------------------------------------------------------
         * Generate map grid secara procedural sesuai ukurang x dan y.
         * tipe Grid ditentukan dari urutan character string map.
         *---------------------------------------------------------------------------------*/
        private void GenerateGrids(int x, int y)
        {
            _listGrid = new Grid[x, y];

            // Set all Grid by (x,y)
            // xi = x index
            // yi = y index
            for (int yi = y-1; yi >= 0 ; yi--)
            {
                for (var xi = 0; xi < x; xi++)
                {
                    // Create gameObject of grid
                    var gridObject = Instantiate(_gridElements[GridType.Ground].prefab, transform);
                    gridObject.transform.position = new Vector3(xi * gridLength, 0, yi * gridLength);
                    gridObject.name = $"Grid ({xi}, {yi})";
                    
                    // Add Grid Controller
                    var gridController = gridObject.AddComponent<GridController>();
                    gridController.Grid = new Grid(xi, yi);

                    // Add into List grid Object
                    _listGrid[xi, yi] = gridController.Grid;
                }
            }
        }
        private void GenerateGrids(Vector2 gridSize) => GenerateGrids((int)gridSize.x, (int)gridSize.y);
        private void GenerateGrids() => GenerateGrids(gridSizeX, gridSizeY);
        
        
        /*----------------------------------------------------------------------------------
         * Menyambungkan 1 grid dengan 4 grid di sebelahnya,
         * yaitu: Kanan, Atas, Kiri, Bawah.
         * Dan jika border pada inspector diset true,
         * maka neighbor yang null akan menjadi borderObject.
         *---------------------------------------------------------------------------------*/
        void SetNeighbors()
        {
            // Access all grid
            for (int yi = 0; yi < gridSizeY ; yi++)
            {
                for (var xi = 0; xi < gridSizeX; xi++)
                {
                    _listGrid[xi,yi].AddNeighbor(GridDirection.Right, GetGrid(xi+1, yi  ));
                    _listGrid[xi,yi].AddNeighbor(GridDirection.Up   , GetGrid(xi  , yi+1));
                    _listGrid[xi,yi].AddNeighbor(GridDirection.Left , GetGrid(xi-1, yi  ));
                    _listGrid[xi,yi].AddNeighbor(GridDirection.Down , GetGrid(xi  , yi-1));
                }
            }
        }

        /*-----------------------------------------------------------------------------------------------
         * Fungsi untuk Mengambil Grid
         *-----------------------------------------------------------------------------------------------*/
        public Grid GetGrid(int x, int y, bool debugMessage = false)
        {
            try
            {
                return _listGrid[x, y];
            }
            catch (IndexOutOfRangeException error)
            {
                if(debugMessage) Debug.LogWarning(error.Message);
                return null;
            }
        }
        public Grid GetGrid(Vector2 location, bool debugMessage = false)
        {
            return GetGrid((int) location.x, (int) location.y, debugMessage);
        }

        // Grid hover 
        public Grid GetGrid()
        {
            return GetGridController()?.Grid;
        }

        public GridController GetGridController()
        {
            try
            {
                return _select.GetObjectHit<GridController>();
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        private void OnDrawGizmos()
        {
            var center = (gridSizeX*gridLength + gridSizeY*gridLength) * 0.5f;
            Gizmos.DrawWireCube(transform.position  + new Vector3(center*0.5f-0.5f,0, center*0.5f-0.5f), new Vector3(center,1,center));
        }
    }
}

