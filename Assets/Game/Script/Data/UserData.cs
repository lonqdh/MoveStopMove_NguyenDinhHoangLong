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
    private int currentCoins = 100;
    public int CurrentCoins { get => currentCoins; set { currentCoins = value; } }

    [SerializeField]
    private List<int> boughtWeapons = new List<int>();
    public List<int> BoughtWeapons { get => boughtWeapons; set { boughtWeapons = value; } }
    [SerializeField]
    private List<int> boughtHats = new List<int>();
    public List<int> BoughtHats { get => boughtHats; set { boughtHats = value; } }

    //[SerializeField]
    //private List<HatType> boughtHats = new List<HatType>();
    //public List<HatType> BoughtHats { get => boughtHats; set { boughtHats = value; } }

    [SerializeField]
    private string username = "New Player";
    public string Username { get => username; set { username = value; } }

    public UserData()
    {
        EquippedWeapon = 0;
        EquippedHat = 0;
        CurrentCoins = 100;
        BoughtWeapons.Add(0);
        //BoughtHats.Add(HatType.Arrow);
        BoughtHats.Add(0);
        Username = "New Player";
    }
}


