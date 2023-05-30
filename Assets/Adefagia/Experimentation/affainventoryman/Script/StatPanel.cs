using System;
using System.Collections;
using System.Collections.Generic;
using Adefagia;
using Adefagia.BattleMechanism;
using UnityEngine;
using adefagia.CharacterStats;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StatPanel : MonoBehaviour
{
    [SerializeField] StatDisplay[] statDisplays;
    [SerializeField] string[] statNames;

    [SerializeField] Text characterName;

    private CharacterStat[] stats;
    
    private List<RobotStat> robotSelected;
    private static int indexRobot = 0;
    
    private TeamManager teamManager;
    private List<List<RobotStat>> _teams;
    private static int countTeam = 0;

    private ItemState _itemState = ItemState.Initialize;


    private void Start()
    {
        // Initiate Robots
        teamManager = GameManager.instance.gameObject.GetComponent<TeamManager>();

        // Initial Name 
        characterName.text = "Robot " + indexRobot.ToString() + " | Team " + GetTeamName() ;


        // Add team
        _teams = new List<List<RobotStat>>
        {
            teamManager.robotsA,
            teamManager.robotsB
        };
        
    }

    public String GetTeamName(){
        if(countTeam == 0){
            return teamManager.teamA.teamName;
        }else{
            return teamManager.teamB.teamName;
        }
    }

    private void Update()
    {
        if (_itemState == ItemState.Update)
        {
            // Update damage value
            _teams[countTeam][indexRobot].damage = statDisplays[0].Stat.Value;
            
            // Update armor value
            _teams[countTeam][indexRobot].armor = statDisplays[1].Stat.Value;

        }

    }

    private void OnValidate()
    {
        statDisplays = GetComponentsInChildren<StatDisplay>();
        
        Debug.Log("OnValidate");
        _itemState = ItemState.Update;

        UpdateStatNames();
    }

    // charStats = Armor, Attack
    public void SetStats(params CharacterStat[] charStats)
    {
        stats = charStats;

        // below stat display length
        if (stats.Length > statDisplays.Length)
        {
            Debug.LogError("Not Enough Stat Display!");
            return;
        }

        for (int i = 0; i < statDisplays.Length; i++)
        {
            statDisplays[i].gameObject.SetActive(i < stats.Length);

            if (i < stats.Length)
            {
                statDisplays[i].Stat = stats[i];
            }
        }
    }

    public void UpdateStatValues()
    {
        for (int i = 0; i < stats.Length; i++)
        {
            statDisplays[i].UpdateStatValue();
        }
    }
    
    public void UpdateStatNames()
    {
        for (int i = 0; i < statNames.Length; i++)
        {
            statDisplays[i].Name = statNames[i];
        }
    }
    
    public void UpdateEquipmentId(Item item, string type){    
        if(type == EquipmentType.Top.ToString()){
            
            // Update helmet id
            _teams[countTeam][indexRobot].helmetId = item;
        
        }else if(type == EquipmentType.Body.ToString()){

            // Update armor id
            _teams[countTeam][indexRobot].armorId = item;
        
        }else if(type == EquipmentType.Weapon.ToString()){

            // Update weapon id
            _teams[countTeam][indexRobot].weaponId = item;
        }    
    }

    public void UnequipId(string type){    
        if(type == EquipmentType.Top.ToString()){
            
            // Update helmet id
            _teams[countTeam][indexRobot].helmetId = null;
        
        }else if(type == EquipmentType.Body.ToString()){

            // Update armor id
            _teams[countTeam][indexRobot].armorId = null;
        
        }else if(type == EquipmentType.Weapon.ToString()){

            // Update weapon id
            _teams[countTeam][indexRobot].weaponId = null;
        }    
    }

    // Unity Event
    public void ChangeTeam()
    {
        countTeam++;
        characterName.text = "Robot " + indexRobot.ToString() + " | Team " + GetTeamName();
        
        // Reset index robot
        indexRobot = 0;
        
        // Make sure 2 team have been selected equipment
        if (countTeam > _teams.Count-1)
        {
            _itemState = ItemState.Finish;
            SceneManager.LoadScene("DimasTesting");
        }
        
        
    }

    public void ChangeRobotIndex(int index)
    {
        indexRobot = index;
        characterName.text = "Robot " + indexRobot.ToString() + " | Team " + GetTeamName() ;
    }

    public Dictionary<String, Item> GetDetailEquipment(int index){
        Dictionary<String, Item> statRobot = new Dictionary<String, Item>();
        
        statRobot.Add("helmetId", _teams[countTeam][index].helmetId);
        statRobot.Add("armorId", _teams[countTeam][index].armorId);
        statRobot.Add("weaponId", _teams[countTeam][index].weaponId);

        return statRobot;
    }

    public Dictionary<String, Item> GetDetailEquipmentTeam(int count){
        Dictionary<String, Item> statRobot = new Dictionary<String, Item>();
        // for (int i = 0; i < equipmentSlots.Length; i++)
        // {
            // statRobot.Add("helmetId", _teams[count][index].helmetId);
            // statRobot.Add("armorId", _teams[count][index].armorId);
            // statRobot.Add("weaponId", _teams[count][index].weaponId);
        // }
        return statRobot;
    }

}

public enum ItemState
{
    Initialize,
    Update,
    Finish,
}