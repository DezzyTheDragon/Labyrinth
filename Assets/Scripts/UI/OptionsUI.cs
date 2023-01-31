using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using Unity.VisualScripting;
using System;

public class OptionsUI : MonoBehaviour
{
    [Header("References")]
    public PlayerMovementController playerMovementController;
    public Camera playerCamera;
    [Header("FOV")]
    public Slider fovSlider;
    public TextMeshProUGUI fovValue;
    [Header("Mouse Sensitivity")]
    public Slider sensSlider;
    public TextMeshProUGUI sensValue;
    [Header("Key Bindings")]
    [SerializeField] private InputActionReference movementAction;

    [SerializeField] private InputActionReference interactAction;
    [SerializeField] private InputActionReference escapeAction;
    [SerializeField] private InputActionReference shootAction;
    [SerializeField] private InputActionReference healAction;
    [SerializeField] private InputActionReference primaryAction;
    [SerializeField] private InputActionReference secondaryAction;
    public GameObject forwardBindButton;
    public GameObject backBindButton;
    public GameObject leftBindButton;
    public GameObject rightBindButton;
    public GameObject interactBindButton;
    public GameObject escapeBindButton;
    public GameObject shootBindButton;
    public GameObject healBindButton;
    public GameObject primaryBindButton;
    public GameObject secondaryBindButton;

    //private Controlls playerController;
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;
    //private string cancelString = "<Keyboard>/escape";
    private string requestString = "Press key";

    // Start is called before the first frame update
    void Awake()
    {
        showCurrentSettings();
    }

    private void showCurrentSettings()
    {
        fovSlider.value = playerCamera.fieldOfView;
        fovValue.text = fovSlider.value.ToString();
        sensSlider.value = playerMovementController.getSensitivity();
        sensValue.text = sensSlider.value.ToString("F2");

        forwardBindButton.GetComponentInChildren<TextMeshProUGUI>().text = InputControlPath.ToHumanReadableString(movementAction.action.bindings[1].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
        backBindButton.GetComponentInChildren<TextMeshProUGUI>().text = InputControlPath.ToHumanReadableString(movementAction.action.bindings[2].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
        leftBindButton.GetComponentInChildren<TextMeshProUGUI>().text = InputControlPath.ToHumanReadableString(movementAction.action.bindings[3].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
        rightBindButton.GetComponentInChildren<TextMeshProUGUI>().text = InputControlPath.ToHumanReadableString(movementAction.action.bindings[4].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
        interactBindButton.GetComponentInChildren<TextMeshProUGUI>().text = InputControlPath.ToHumanReadableString(interactAction.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
        escapeBindButton.GetComponentInChildren<TextMeshProUGUI>().text = InputControlPath.ToHumanReadableString(escapeAction.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice); ;
        shootBindButton.GetComponentInChildren<TextMeshProUGUI>().text = InputControlPath.ToHumanReadableString(shootAction.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice); ;
        healBindButton.GetComponentInChildren<TextMeshProUGUI>().text = InputControlPath.ToHumanReadableString(healAction.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice); ;
        primaryBindButton.GetComponentInChildren<TextMeshProUGUI>().text = InputControlPath.ToHumanReadableString(primaryAction.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice); ;
        secondaryBindButton.GetComponentInChildren<TextMeshProUGUI>().text = InputControlPath.ToHumanReadableString(secondaryAction.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice); ;
    }

    public void OnFOVChanged()
    {
        playerCamera.fieldOfView = fovSlider.value;
        fovValue.text = fovSlider.value.ToString();
    }

    public void OnSensChanged()
    {
        playerMovementController.setSensitivity(sensSlider.value);
        sensValue.text = sensSlider.value.ToString("F2");
    }

    public void SetForwardBind()
    {
        forwardBindButton.GetComponentInChildren<TextMeshProUGUI>().text = requestString;

        movementAction.action.Disable();
        rebindingOperation = movementAction.action.PerformInteractiveRebinding(1)
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => FinishBind(forwardBindButton, movementAction, 1))
            .Start();
    }

    public void SetInteractBind()
    {
        interactBindButton.GetComponentInChildren<TextMeshProUGUI>().text = requestString;

        interactAction.action.Disable();
        rebindingOperation = interactAction.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => FinishBind(interactBindButton, interactAction))
            //.WithCancelingThrough(cancelString)
            .Start();
    }

    public void SetEscapeBind()
    {
        escapeBindButton.GetComponentInChildren<TextMeshProUGUI>().text = requestString;

        escapeAction.action.Disable();
        rebindingOperation = escapeAction.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => FinishBind(escapeBindButton, escapeAction))
            //.WithCancelingThrough(cancelString)
            .Start();
    }

    public void SetShootBind()
    {
        shootBindButton.GetComponentInChildren<TextMeshProUGUI>().text = requestString;

        shootAction.action.Disable();
        rebindingOperation = shootAction.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => FinishBind(shootBindButton, shootAction))
            //.WithCancelingThrough(cancelString)
            .Start();
    }

    public void SetHealBind()
    {
        healBindButton.GetComponentInChildren<TextMeshProUGUI>().text = requestString;

        healAction.action.Disable();
        rebindingOperation = healAction.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => FinishBind(healBindButton, healAction))
            //.WithCancelingThrough(cancelString)
            .Start();
    }

    public void SetPrimaryBind()
    {
        primaryBindButton.GetComponentInChildren<TextMeshProUGUI>().text = requestString;

        primaryAction.action.Disable();
        rebindingOperation = primaryAction.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => FinishBind(primaryBindButton, primaryAction))
            //.WithCancelingThrough(cancelString)
            .Start();
    }

    public void SetSecondaryBind()
    {
        secondaryBindButton.GetComponentInChildren<TextMeshProUGUI>().text = requestString;

        secondaryAction.action.Disable();
        rebindingOperation = secondaryAction.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => FinishBind(secondaryBindButton, secondaryAction))
            //.WithCancelingThrough(cancelString)
            .Start();
    }

    private void FinishBind(GameObject button, InputActionReference action, int bindIndex = 0)
    {
        button.GetComponentInChildren<TextMeshProUGUI>().text = InputControlPath.ToHumanReadableString(action.action.bindings[bindIndex].effectivePath, 
            InputControlPath.HumanReadableStringOptions.OmitDevice);
        rebindingOperation.Dispose();
        action.action.Enable();
    }
}
