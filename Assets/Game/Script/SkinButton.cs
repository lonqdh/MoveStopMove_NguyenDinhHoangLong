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

    public void SetData()
    {
        skinButton.onClick.AddListener(() => SkinButtonOnClick());
    }

    private void SkinButtonOnClick()
    {
        SkinShopManager.Instance.skinPrice.SetText(this.hatData.hatPrice.ToString());
        SkinShopManager.Instance.skinStats.SetText("+" + this.hatData.range.ToString());
        PreviewHat();
    }

    private void PreviewHat()
    {
        if (previewingHat = true) //trick lord
        {
            Debug.Log("Despawning previous hat");
            LeanPool.DespawnAll();

        }

        Debug.Log("Spawning new hat");
        previewHat = LeanPool.Spawn(this.hatData.hatPrefab, Vector3.zero, Quaternion.identity);
        previewHat.transform.SetParent(LevelManager.Instance.player.hatPos, false);
        previewingHat = true;
    }
}
