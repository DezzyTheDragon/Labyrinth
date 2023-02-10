using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Mirror;

public class PlayerInventory : NetworkBehaviour
{
    [SerializeField]
    private GameObject markerPrefab;
    [SerializeField]
    private WeaponBase primaryWeapon;
    private WeaponBase secondaryWeapon;
    private int healthPackCount = 0;
    private int markerCount = 10;
    private WeaponBase heldWeapon;

    public TextMeshProUGUI healthPackUI;
    public TextMeshProUGUI markerCountUI;
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
        markerCountUI.text = markerCount.ToString();
    }

    public WeaponBase CurrentWeapon()
    {
        return heldWeapon;
    }

    public void PickupMarker()
    {
        markerCount++;
        markerCountUI.text = markerCount.ToString();
    }

    public void PlaceMarker(Vector3 inPosition)
    {
        markerCount--;
        if (markerCount >= 0)
        {
            markerCountUI.text = markerCount.ToString();
            
            CmdPlaceMarker(inPosition);
        }
        else
        {
            markerCount = 0;
        }
    }

    [Command]
    public void CmdPlaceMarker(Vector3 inPosition)
    {
        //Instanciate object
        GameObject newMarker = Instantiate(markerPrefab, new Vector3(inPosition.x, 0, inPosition.z), gameObject.transform.rotation);
        NetworkServer.Spawn(newMarker);
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
        if (secondaryWeapon != null)
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
                AddWeapon(item);
                break;
            case ObjectTags.Healing:
                AddHealthItem();
                break;
            case ObjectTags.Marker:
                PickupMarker();
                break;
        }
    }

    private void AddWeapon(ItemBase item)
    {
        //WeaponBase weapon = item as WeaponBase;
        WeaponBase weapon = item.gameObject.GetComponent<WeaponBase>();
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
