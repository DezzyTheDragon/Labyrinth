using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinItem : MonoBehaviour
{
    public TextMeshProUGUI skinName;
    public Button enableButton;
    public Image backdrop;

    private int SkinId;

    public void SetName(string name)
    {
        skinName.text = name;
    }

    public void SetEnabled(bool isEnabled)
    {
        enableButton.interactable = isEnabled;
    }

    public void SetBackdropColor(Color color)
    {
        backdrop.color = color;
    }

    public void SetID(int id)
    {
        SkinId = id;
    }

    public void ButtonPress()
    {
        EnabledSkin.enabledPistolSkinID = SkinId;
    }
}
