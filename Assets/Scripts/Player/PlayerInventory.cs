using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField]
    private WeaponBase primaryWeapon;
    private WeaponBase secondaryWeapon;
    private int healthPackCount = 0;
    private WeaponBase heldWeapon;

    public TextMeshProUGUI healthPackUI;
    public Image secondaryIcon;
    public List<Sprite> weaponIcons = new List<Sprite>();
    public List<GameObject> weaponModels = new List<GameObject>();
    public GameObject primaryWeaponModel;

    private PlayerHealth playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        secondaryWeapon = null;
        secondaryIcon.enabled = false;
        playerHealth = GetComponent<PlayerHealth>();
        heldWeapon = primaryWeapon;
    }

    public WeaponBase CurrentWeapon()
    {
        return heldWeapon;
    }

    public void UseHealthPack()
    {
        if (healthPackCount > 0)
        {
            healthPackCount--;
            //Heal the player
            healthPackUI.text = healthPackCount.ToString();
            playerHealth.Heal(10);
        }
    }

    public void ShowPrimary()
    {
        heldWeapon = primaryWeapon;
        primaryWeaponModel.SetActive(true);
        weaponModels[secondaryWeapon.GetWeaponID() - 1].SetActive(false);
    }

    public void ShowSecondary()
    {
        if (secondaryWeapon == null)
        { 
            heldWeapon = secondaryWeapon;
            primaryWeaponModel.SetActive(false);
            weaponModels[secondaryWeapon.GetWeaponID() - 1].SetActive(true);
        }
    }

    public void AddItem(ItemBase item)
    {
        switch (item.GetTag())
        {
            case ObjectTags.Object:
                Debug.Log("Picked up an item");
                break;
            case ObjectTags.Weapon:
                //Debug.Log("Weapon " + item.GetName() + " picked up");
                AddWeapon(item);
                break;
            case ObjectTags.Healing:
                //Debug.Log("Healing item picked up");
                AddHealthItem();
                break;
        }
    }

    private void AddWeapon(ItemBase item)
    {
        WeaponBase weapon = item as WeaponBase;
        secondaryIcon.enabled = true;
        if (weapon.GetWeaponID() == 0)
        {
            Debug.LogError("Invalid weapon id (must be positive non-zero value)");
        }
        //If there is already a secondary weapon drop it in the real world
        secondaryIcon.sprite = weaponIcons[weapon.GetWeaponID() - 1];
        secondaryWeapon = weapon;
    }

    private void AddHealthItem()
    {
        healthPackCount++;
        healthPackUI.text = healthPackCount.ToString();
    }
}
