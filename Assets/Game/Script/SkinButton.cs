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
    [NonSerialized] public PantData pantData;
    private GameObject previewHat;
    private Material previewPant;
    private bool previewingHat;
    public Image skinImage;

    //public delegate void SkinBoughtDelegate();
    //public static event SkinBoughtDelegate OnSkinBought;

    private void Start()
    {
        skinButton.onClick.AddListener(SkinButtonOnClick);
    }

    private void SkinButtonOnClick()
    {
        if (/*SkinShopManager.Instance.currentSession != 1*/ this.pantData != null)
        {
            SkinShopManager.Instance.skinPrice.SetText(this.pantData.PantPrice.ToString());
            SkinShopManager.Instance.skinStats.SetText("+" + this.pantData.Speed.ToString() + " MoveSpeed");
        }
        else
        {
            SkinShopManager.Instance.skinPrice.SetText(this.hatData.hatPrice.ToString());
            SkinShopManager.Instance.skinStats.SetText("+" + this.hatData.range.ToString() + " AtkRange");
            
        }
        
        PreviewSkin();

    }

    private void PreviewSkin()
    {
        if(this.hatData != null)
        {
            if (LevelManager.Instance.player.hatInstance != null)
            {
                LevelManager.Instance.player.hatInstance.SetActive(false); //an mu dang mac di de preview mu~ trong shop
            }

            SkinShopContent.Instance.DespawnHat();
            SkinShopManager.Instance.ShowSkinAvailability(this);
            SkinShopContent.Instance.SetCurrentHatSelected(this); // set current hat selected
            previewHat = LeanPool.Spawn(this.hatData.hatPrefab, LevelManager.Instance.player.hatPos);
            SkinShopContent.Instance.hatList.Add(previewHat);
        }
        else
        {
            LevelManager.Instance.player.pantInstance.material = this.pantData.PantMaterial;
            SkinShopManager.Instance.ShowSkinAvailability(this);
            SkinShopContent.Instance.SetCurrentPantSelected(this); // set current pant selected
        }

    }

}
