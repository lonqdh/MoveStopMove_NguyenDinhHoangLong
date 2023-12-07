using Lean.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinButton : MonoBehaviour
{
    [SerializeField] private Button skinButton;
    [NonSerialized] public HatData hatData;
    //[NonSerialized] public PantData pantData;
    private GameObject previewHat;
    private bool previewingHat;
    public Image hatImage;
    private void Start()
    {
        skinButton.onClick.AddListener(SkinButtonOnClick);
    }

    private void SkinButtonOnClick()
    {
        SkinShopManager.Instance.skinPrice.SetText(this.hatData.hatPrice.ToString());
        SkinShopManager.Instance.skinStats.SetText("+" + this.hatData.range.ToString());
        PreviewHat();
    }

    private void PreviewHat()
    {
        if(LevelManager.Instance.player.hatInstance != null)
        {
            LevelManager.Instance.player.hatInstance.SetActive(false); //an mu dang mac di de preview mu~ trong shop
        }
        SkinShopContent.Instance.DespawnHat();
        SkinShopManager.Instance.ShowHatSkinAvailability(this);
        SkinShopContent.Instance.SetCurrentHatSelected(this); // set equipped hat
        previewHat = LeanPool.Spawn(this.hatData.hatPrefab, LevelManager.Instance.player.hatPos);
        SkinShopContent.Instance.hatList.Add(previewHat);
        //previewingHat = true;
    }
   
}
