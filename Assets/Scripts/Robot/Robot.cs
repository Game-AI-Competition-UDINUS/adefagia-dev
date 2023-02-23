﻿using System.Collections.Generic;
using System.Linq;
using adefagia.Collections;
using adefagia.Graph;
using adefagia.PercobaanPathfinding;
using Unity.VisualScripting;
using UnityEngine;
using Grid = adefagia.Graph.Grid;

namespace adefagia.Robot
{
    
    public class Robot : ISelectableObject
    {
        public int Id { get; }
        private GameObject RobotGameObject { get; }
        public Grid Grid { get; private set; }

        private readonly Spawner _spawner;
        private RobotManager _robotManager;
        
        public float Health { get; set; }
        public float Damage { get; set; }
        
        public bool IsHover { get; private set; }
        public bool IsSelect { get; private set; }

        private List<Grid> _gridRange;
        private Vector2[] _dirs;

        public Robot(Spawner spawner, RobotPrefab prefab)
        {
            _spawner = spawner;

            Id = prefab.id;
            
            Grid = GameManager.instance.gridManager.GetGridByLocation(prefab.location, true);

            RobotGameObject = Instantiate(prefab.gameObject);

            Health = prefab.status.healthPoint;
            Damage = prefab.status.attackDamage;
            
            _gridRange = new List<Grid>();
            _dirs = new[]
            {
                Vector2.right, Vector2.up, Vector2.left, Vector2.down,
                Vector2.down + Vector2.left, Vector2.down + Vector2.right,
                Vector2.up + Vector2.left, Vector2.up + Vector2.right
            };
        }

        private GameObject Instantiate(GameObject prefab)
        {
            if (prefab.IsUnityNull()) return null;

            if (Grid != null)
            {
                if (GridManager.CheckGround(Grid) && !Grid.IsOccupied)
                {
                    Debug.Log("Instantiate Robot");
                    var robot = Object.Instantiate(prefab, Grid.GetLocation(), prefab.transform.rotation, _spawner.SpawnerGameObject.transform);
                    robot.name = "Robot " + Id;

                    robot.GetComponent<RobotStatus>().Robot = this;
                    robot.GetComponent<RobotMovement>().Robot = this;
                    
                    // Grid Occupied
                    Grid.Occupy();
                    Grid.Robot = this;

                    return robot;
                }
            }

            Debug.LogWarning($"Robot ID:{Id} Must be place on Ground Grid & not Occupied");
            return null;
        }

        public void Attack(Robot robot)
        {
            robot.Health -= Damage;
        }
        
        public void Move(Grid grid)
        {
            Grid = grid;
            if (Grid != null) Grid.Robot = this;
            
        }
        
        public void SetGridRange()
        {
            var dirs = _dirs.Select(dir => Grid.Location + dir).ToList();

            var aStar = new AStar();
            
            aStar.BFS(Grid, dirs);
            // aStar.BFSLine(AStar.BFSLineType.Right, Grid);
            // aStar.BFSLine(AStar.BFSLineType.Up, Grid);
            // aStar.BFSLine(AStar.BFSLineType.Left, Grid);
            // aStar.BFSLine(AStar.BFSLineType.Down, Grid);

            _gridRange = aStar.Reached;
            // aStar.DebugListRobot(aStar.Robots);
        }

        public void ClearGridRange()
        {
            _gridRange.Clear();
        }

        public void ResetDefaultGrid()
        {
            foreach (var grid in _gridRange)
            {
                grid.IsHighlight = false;
                grid.Hover(false);
                grid.Selected(false);
            }
        }

        public bool IsInGridRange(Grid grid)
        {
            return _gridRange.Contains(grid);
        }
        
        public void Selected(bool value)
        {
            IsSelect = value;
        }
        
        public void Hover(bool value)
        {
            IsHover = value;
        }

    }
}