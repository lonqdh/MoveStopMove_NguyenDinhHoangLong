using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinShopContent : MonoBehaviour
{
    [SerializeField] private HatDataSO hatData;
    [SerializeField] private SkinButton skinBtnPrefab;
    [SerializeField] private Transform skinItemParent;
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


}
