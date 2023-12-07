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
        SkinShopContent.Instance.DespawnHat();
        previewHat = LeanPool.Spawn(this.hatData.hatPrefab, LevelManager.Instance.player.hatPos);
        SkinShopContent.Instance.hatList.Add(previewHat);
        //previewingHat = true;
    }
   
}
