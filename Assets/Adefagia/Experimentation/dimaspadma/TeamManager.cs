using System;
using System.Collections;
using System.Collections.Generic;
using Adefagia.BattleMechanism;
using Adefagia.RobotSystem;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    // TeamA
    public Team teamA;
    public List<RobotStat> robotsA; 

    // TeamB
    public Team teamB;
    public List<RobotStat> robotsB; 
}

[Serializable]
public class RobotStat
{
    public float maxHealth;
    public float maxStamina;
    public float damage;
    public float armor;

    public Item armorId;

    public Item helmetId;
    
    public Item weaponId;
    
}