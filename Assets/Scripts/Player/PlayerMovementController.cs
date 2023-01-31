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
    public GameObject playerModel;
    public GameObject headObject;
    public GameObject playerCamera;
    public GameObject pausePanel;

    [SerializeField]
    private float movementSpeed = 1.0f;
    [SerializeField]
    private float sensitivity = 1.0f;

    private bool mouseLocked = false;
    private bool isDowned = false;
    private Vector2 cameraRotation;


    private Controlls playerControlls;
    private CharacterController characterController;
    private PlayerHealth playerHealth;
    private PlayerInventory playerInventory;

    private void Awake()
    {
        playerControlls = new Controlls();
        playerControlls.PlayerControlls.Enable();
        characterController = GetComponent<CharacterController>();
        playerHealth = GetComponent<PlayerHealth>();
        playerInventory = GetComponent<PlayerInventory>();

        //Open settings file and set them
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "PrototypeMap")
        {

            if (!playerModel.activeSelf)
            {
                playerModel.SetActive(true);
                if (isOwned)
                {
                    playerCamera.SetActive(true);
                }
                else
                {
                    playerCamera.SetActive(false);
                }
                SetPosition();
                ToggleMouse();
            }
            //if (hasAuthority)
            if (isOwned)
            {
                HandleInput();
            }
        }
    }


    public void setSensitivity(float sens)
    {
        sensitivity = sens;
    }

    public float getSensitivity()
    {
        return sensitivity;
    }

    public void Escape(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            //Application.Quit();
            openPauseMenu();
        }
    }

    private void openPauseMenu()
    {
        pausePanel.SetActive(true);
        ToggleMouse();
        playerControlls.PlayerControlls.Disable();
        playerControlls.MenuControlls.Enable();
    }

    public void Renable()
    {
        closePauseMenu();
    }

    private void closePauseMenu()
    {
        pausePanel.SetActive(false);
        ToggleMouse();
        playerControlls.PlayerControlls.Enable();
        playerControlls.MenuControlls.Disable();
    }


    public void Heal(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && isOwned)
        {
            playerInventory.UseHealthPack();
        }
    }

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
                //Debug.Log("Hit: " + hit.transform.name);
                if (hit.transform.tag == "Object")
                {
                    ItemBase item = hit.transform.GetComponent<ItemBase>();
                    item.OnPickup();
                    playerInventory.AddItem(item);
                }
                else if (hit.transform.tag == "Player")
                {
                    hit.transform.GetComponent<PlayerMovementController>().RevivePlayer();
                }
                else
                {
                    //Nothing wanted was hit assume placeing marker
                    //Debug.Log("Hit position: " + hit.point);
                    playerInventory.CmdPlaceMarker(hit.point);
                }
            }
        }
    }

    /*[Command]
    public void CmdPickup(GameObject obj)
    {
        
    }*/

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
