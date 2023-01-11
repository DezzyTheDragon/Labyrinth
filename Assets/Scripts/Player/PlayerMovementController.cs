using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;
using UnityEngine.SceneManagement;

public class PlayerMovementController : NetworkBehaviour
{
    public GameObject playerModel;
    
    private Controlls playerControlls;

    private void Awake()
    {
        playerControlls = new Controlls();
        playerControlls.PlayerControlls.Enable();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "PrototypeMap")
        {
            if (!playerModel.activeSelf)
            {
                playerModel.SetActive(true);
                SetPosition();
            }
            //if (hasAuthority)
            if(isOwned)
            { 
                HandleInput();
            }
        }
    }

    //Set the player position when spawning
    public void SetPosition()
    {
        transform.position = new Vector3(Random.Range(-5, 5), 0.1f, Random.Range(-5, 5));
    }

    //TODO: Refine the player movement and implement the first person aspect to it
    //      This is for testing the networking bit of player movement
    //=============================================================================
    private void HandleInput()
    {
        HandleMovement(); 
    }

    private void HandleMovement()
    {
        Vector2 input = playerControlls.PlayerControlls.Movement.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(input.x, 0.0f, input.y);
        transform.position += moveDirection * 0.1f;
    }
    //=============================================================================
}
