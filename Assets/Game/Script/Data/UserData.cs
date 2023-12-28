using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData
{
    [SerializeField]
    private int equippedWeapon = 0;
    public int EquippedWeapon { get => equippedWeapon; set { equippedWeapon = value; } }
    
    [SerializeField]
    private int equippedHat = 0;
    public int EquippedHat { get => equippedHat; set { equippedHat = value; } }
    
    [SerializeField]
    private int equippedPant = 0;
    public int EquippedPant { get => equippedPant; set { equippedPant = value; } }

    [SerializeField]
    private int currentCoins = 100;
    public int CurrentCoins { get => currentCoins; set { currentCoins = value; } }

    [SerializeField]
    private List<int> boughtWeapons = new List<int>();
    public List<int> BoughtWeapons { get => boughtWeapons; set { boughtWeapons = value; } }
    
    [SerializeField]
    private List<int> boughtHats = new List<int>();
    public List<int> BoughtHats { get => boughtHats; set { boughtHats = value; } }
    
    [SerializeField]
    private List<int> boughtPants = new List<int>();
    public List<int> BoughtPants { get => boughtPants; set { boughtPants = value; } }
    
    [SerializeField]
    private string username = "New Player";
    public string Username { get => username; set { username = value; } }

    public UserData()
    {
        EquippedWeapon = 0;
        EquippedHat = 1;
        EquippedPant = 0;
        CurrentCoins = 10000;
        BoughtWeapons.Add(0);
        BoughtHats.Add(1);
        BoughtPants.Add(0);
        Username = "Hoang Long";
    }
}


