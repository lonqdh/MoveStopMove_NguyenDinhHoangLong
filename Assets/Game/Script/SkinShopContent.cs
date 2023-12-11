using Lean.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkinShopContent : Singleton<SkinShopContent>
{
    public HatDataSO hatDataSO;
    public PantDataSO pantDataSO;
    [SerializeField] private SkinButton skinBtnPrefab;
    [SerializeField] private Transform skinItemParent;
    [NonSerialized] public List<GameObject> hatList = new List<GameObject>();
    [NonSerialized] public List<SkinButton> skinButtonList = new List<SkinButton>();
    public HatType currentHatType;
    public PantType currentPantType;


    public void SpawnSkin()
    {
        DestroyAllGameObjects();

        if (SkinShopManager.Instance.currentSession == 1)
        {
            for (int i = 0; i < hatDataSO.hatDataList.Count; i++)
            {
                SkinButton skinButton = Instantiate(skinBtnPrefab, skinItemParent);
                skinButton.hatData = hatDataSO.hatDataList[i];
                skinButton.skinImage.sprite = hatDataSO.hatDataList[i].hatImage;
                skinButtonList.Add(skinButton);
            }
        }
        else
        {
            for (int i = 0; i < pantDataSO.pantDataList.Count; i++)
            {
                SkinButton skinButton = Instantiate(skinBtnPrefab, skinItemParent);
                skinButton.pantData = pantDataSO.pantDataList[i];
                skinButton.skinImage.sprite = pantDataSO.pantDataList[i].PantImage;
                skinButtonList.Add(skinButton);
            }
        }

    }

    private void DestroyAllGameObjects()
    {
        foreach (SkinButton obj in skinButtonList)
        {
            Destroy(obj.gameObject);
        }

        // Clear the list after destroying all objects
        skinButtonList.Clear();
    }

    public HatType SetCurrentHatSelected(SkinButton skinBtn) // set current hat selected de biet thg nao con mua, con equip
    {
        currentHatType = skinBtn.hatData.hatType;
        return currentHatType;
    }

    public PantType SetCurrentPantSelected(SkinButton skinBtn) // set current pant selected de biet thg nao con mua, con equip
    {
        currentPantType = skinBtn.pantData.PantType;
        return currentPantType;
    }

    public void DespawnHat()
    {
        if (hatList.Count > 0)
        {
            LeanPool.Despawn(hatList[0].gameObject);
            hatList.Clear();
        }
    }

}
