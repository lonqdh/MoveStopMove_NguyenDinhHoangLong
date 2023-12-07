using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public WeaponDataSO weaponDataSO;
    public HatDataSO hatDataSO;

    //public WeaponData GetWeaponData(WeaponType weaponType)
    //{
    //    List<WeaponData> weaponData = weaponDataSO.weaponDataList;
    //    for (int i = 0; i < weaponData.Count; i++)
    //    {
    //        if(weaponType == weaponData[i].weaponType)
    //        {
    //            //return weaponData[i];
    //            return weaponData[i].Clone();
    //        }
    //    }

    //    return null;
    //}

    public WeaponData GetWeaponData(int weaponIndex)
    {
        return weaponDataSO.weaponDataList[weaponIndex];
    }

    public HatData GetHatData(HatType hatType)
    {
        for (int i = 0; i < hatDataSO.hatDataList.Count; i++)
        {
            if (hatDataSO.hatDataList[i].hatType == hatType)
            {
                return hatDataSO.hatDataList[i];
            }
        }
        return null;
        
    }
}
