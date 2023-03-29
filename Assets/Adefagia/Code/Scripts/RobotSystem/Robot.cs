﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using Adefagia.GridSystem;
using Grid = Adefagia.GridSystem.Grid;
using Adefagia.SelectObject;
using Adefagia.Collections;

namespace Adefagia.RobotSystem
{
    public class Robot
    {
        private Grid _grid;

        #region Properties
        
        // Status
        public int ID { get; set; }
        public string Name { get; }
        public Grid Location => _grid;
        public float MaxHealth { get; }
        public float CurrentHealth { get; }
        public float Damage { get; }
        public float DelayMove { get; set; }
        
        // Step Status
        public bool HasMove { get; set; }
        public bool HasAttack { get; set; }
        public bool HasDefend { get; set; }
        
        #endregion

        public Robot(string name, float maxHealth, float damage)
        {
            Name = name;
            MaxHealth = maxHealth;
            CurrentHealth = MaxHealth;
            Damage = damage;
            
            //-----------------------
            ResetStepStat();
        }

        /*------------------------------------------------------------------
         * Change location equal grid where he stand
         *------------------------------------------------------------------*/
        public void ChangeLocation(Grid grid)
        {
            _grid = grid;
        }

        public void ResetStepStat()
        {
            HasMove = false;
            HasAttack = false;
            HasDefend = false;
        }

        public override string ToString()
        {
            return $"{Name}";
        }
        
    }
}