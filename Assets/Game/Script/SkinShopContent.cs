using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinShopContent : Singleton<SkinShopContent>
{
    [SerializeField] private HatDataSO hatData;
    [SerializeField] private SkinButton skinBtnPrefab;
    [SerializeField] private Transform skinItemParent;
    public List<GameObject> hatList = new List<GameObject>();
    //[SerializeField] private GameObject menu;


    private void Start()
    {
        SpawnHatSkin();
    }

    private void SpawnHatSkin()
    {
        for (int i = 0; i < hatData.hatDataList.Count; i++)
        {
            SkinButton skinButton = Instantiate(skinBtnPrefab, skinItemParent);
            skinButton.hatData = hatData.hatDataList[i];
            skinButton.hatImage.sprite = hatData.hatDataList[i].hatImage;
        }
    }

    private void SpawnPantSkin()
    {

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
