using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;
using UnityEngine.SceneManagement;
using Mirror.Examples.AdditiveLevels;
using System.Threading;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.HID;

public class PlayerMovementController : NetworkBehaviour
{
    [Header("Object References")]
    public GameObject playerModel;
    public GameObject headObject;
    public GameObject playerCamera;
    public GameObject pausePanel;
    public GameObject playerCanvas;
    public InputActionAsset actionAsset;

    [Header("Variables")]
    [SerializeField] private float movementSpeed = 1.0f;
    [SerializeField] private float sensitivity = 1.0f;

    private bool mouseLocked = false;
    private bool isDowned = false;
    private Vector2 cameraRotation;

    private Controlls playerControlls;
    private CharacterController characterController;
    private PlayerHealth playerHealth;
    private PlayerInventory playerInventory;

    //Runs when the object is enabled
    private void Awake()
    {
        //Initilize and get variables
        playerControlls = new Controlls();
        playerControlls.PlayerControlls.Enable();
        characterController = GetComponent<CharacterController>();
        playerHealth = GetComponent<PlayerHealth>();
        playerInventory = GetComponent<PlayerInventory>();
        loadSettings();
    }

    //Runs on every frame
    private void Update()
    {
        //Check if loaded into the level
        if (SceneManager.GetActiveScene().name == "PrototypeMap")
        {
            //Enable the relevent components for the local player while leaving them
            //  disabled for the remote players
            if (!playerModel.activeSelf)
            {
                playerModel.SetActive(true);
                if (isOwned)
                {
                    playerCamera.SetActive(true);
                    playerCanvas.SetActive(true);
                }
                else
                {
                    playerCamera.SetActive(false);
                    playerCanvas.SetActive(false);
                }
                SetPosition();
                ToggleMouse();
            }
            if (isOwned)
            {
                HandleInput();
            }
        }
    }

    //Load the player preferences
    public void loadSettings()
    {
        string rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds))
        {
            actionAsset.LoadBindingOverridesFromJson(rebinds);
        }
        float prefFOV = PlayerPrefs.GetFloat("FOV");
        if (!EqualityComparer<float>.Default.Equals(prefFOV, default(float)) && prefFOV != 0)
        {
            playerCamera.GetComponent<Camera>().fieldOfView = prefFOV;
        }
        float prefSens = PlayerPrefs.GetFloat("Sens");
        if (!EqualityComparer<float>.Default.Equals(prefSens, default(float)) && prefSens != 0)
        {
            sensitivity = prefSens;
        }
        
    }

    //Set the mouse sensitivity from the options menu
    public void setSensitivity(float sens)
    {
        sensitivity = sens;
    }

    //Gets the mouse sensitivity value for the options to display
    public float getSensitivity()
    {
        return sensitivity;
    }

    //Function for escape action
    public void Escape(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && isOwned)
        {
            openPauseMenu();
        }
    }

    //Opens the pause menu and disables player controlls while in menu
    private void openPauseMenu()
    {
        pausePanel.SetActive(true);
        ToggleMouse();
        playerControlls.PlayerControlls.Disable();
        playerControlls.MenuControlls.Enable();
    }

    //Saves preferences, re-enables player controlls
    public void Renable()
    {
        string rebinds = actionAsset.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
        PlayerPrefs.SetFloat("FOV", playerCamera.GetComponent<Camera>().fieldOfView);
        PlayerPrefs.SetFloat("Sens", sensitivity);
        closePauseMenu();
    }

    //Closes the menu UI
    private void closePauseMenu()
    {
        pausePanel.SetActive(false);
        ToggleMouse();
        playerControlls.PlayerControlls.Enable();
        playerControlls.MenuControlls.Disable();
    }

    //Uses a healthpack in the inventory to heal
    public void Heal(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && isOwned)
        {
            playerInventory.UseHealthPack();
        }
    }

    //Switches to the primary weapon
    public void GetPrimary(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && isOwned)
        {
            playerInventory.ShowPrimary();
        }
    }

    public void GetSecondary(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && isOwned)
        {
            playerInventory.ShowSecondary();
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && isOwned)
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, Mathf.Infinity))
            {
                if (hit.transform.tag == "Enemy")
                {
                    //Debug.Log("Enemy Hit: " + hit.transform.name);
                    CmdShoot(hit.transform.gameObject);
                }
            }
        }
    }

    [Command]
    public void CmdShoot(GameObject enemy)
    {
        //Debug.Log("Enemy Hit: " + enemy.name);
        enemy.GetComponent<EnemyHealth>().DamageEnemy(playerInventory.CurrentWeapon().GetWeaponDamage());
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && isOwned)
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 3))
            {
                if (hit.transform.tag == "Object")
                {
                    ItemBase item = hit.transform.GetComponent<ItemBase>();
                    playerInventory.AddItem(item);
                    CmdPickupItem(item);
                }
                else if (hit.transform.tag == "Player")
                {
                    hit.transform.GetComponent<PlayerMovementController>().RevivePlayer();
                }
                else
                {
                    playerInventory.PlaceMarker(hit.point);
                }
            }
        }
    }

    [Command]
    public void CmdPickupItem(ItemBase item)
    {
        item.OnPickup();
    }

    public void SetIsDowned()
    {
        isDowned = true;
        headObject.transform.localPosition = new Vector3(0, 0.5f, 0);
        //Debug.Log("Player is donwed!");
    }

    public void RevivePlayer()
    {
        isDowned = false;
        headObject.transform.localPosition = new Vector3(0, 1.5f, 0);
        playerHealth.Heal(25);
    }

    private void ToggleMouse()
    {
        if (mouseLocked)
        {
            mouseLocked = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            mouseLocked = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    //Set the player position when spawning
    public void SetPosition()
    {
        characterController.enabled = false;
        transform.position = new Vector3(Random.Range(-5, 5), 0.1f, Random.Range(-5, 5));
        characterController.enabled = true;
    }

    private void HandleInput()
    {
        HandleMovement();
        HandleCamera();
    }

    private void HandleCamera()
    {
        cameraRotation.x += playerControlls.PlayerControlls.MouseX.ReadValue<float>();
        cameraRotation.y += playerControlls.PlayerControlls.MouseY.ReadValue<float>();
        cameraRotation.y = Mathf.Clamp(cameraRotation.y, -90f / sensitivity, 90f / sensitivity);
        gameObject.transform.localRotation = Quaternion.Euler(0, cameraRotation.x * sensitivity, 0);
        headObject.transform.localRotation = Quaternion.Euler(-cameraRotation.y * sensitivity, 0, 0);
    }

    private void HandleMovement()
    {
        if (!isDowned)
        { 
            Vector2 input = playerControlls.PlayerControlls.Movement.ReadValue<Vector2>();
            Vector3 moveForward = transform.forward * input.y;
            Vector3 moveRight = transform.right * input.x;
            Vector3 direction = (moveForward + moveRight).normalized * movementSpeed;
            direction.y = 0;
            characterController.Move(direction * Time.deltaTime);
        }
    }
}
