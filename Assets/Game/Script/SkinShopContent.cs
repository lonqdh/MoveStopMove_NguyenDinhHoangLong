using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinShopContent : Singleton<SkinShopContent>
{
    [SerializeField] public HatDataSO hatDataSO;
    [SerializeField] private SkinButton skinBtnPrefab;
    [SerializeField] private Transform skinItemParent;
    public List<GameObject> hatList = new List<GameObject>();
    public List<SkinButton> skinButtonList = new List<SkinButton>();
    public HatType currentHatType;
    //[SerializeField] private GameObject menu;


    private void Start()
    {
        SpawnHatSkin();
    }

    private void SpawnHatSkin()
    {
        for (int i = 0; i < hatDataSO.hatDataList.Count; i++)
        {
            SkinButton skinButton = Instantiate(skinBtnPrefab, skinItemParent);
            skinButton.hatData = hatDataSO.hatDataList[i];
            skinButton.hatImage.sprite = hatDataSO.hatDataList[i].hatImage;
            skinButtonList.Add(skinButton);
        }
    }

    public HatType SetCurrentHatSelected(SkinButton skinBtn) //set equpped hat
    {
        currentHatType = skinBtn.hatData.hatType;
        return currentHatType;
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
