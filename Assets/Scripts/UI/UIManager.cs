using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*=====================================
 * Author: Destiny Dahlgren
 * Description: This script helps manage
 *      UI panales and navagation
 *=====================================*/

public class UIManager : MonoBehaviour
{
    //A list of the UIPanels that exist in the scene
    public List<GameObject> uiPanels = new List<GameObject>();
    //A history of panels traveled by the player so that when the
    //  back button is pressed it can return to whatever panel was
    //  previously up
    private FILO<GameObject> navHistory = new FILO<GameObject>();
    //The current active panel
    public GameObject currentPanel = null;


    //Desc: Button function that hides the current panel and
    //      displays the given panel
    //Param: string panelName (The object name of the panel in the hierarcy)
    //Return: None
    public void navigateButtonPress(string panelName)
    {
        if (currentPanel != null)
        {
            currentPanel.SetActive(false);
            navHistory.PushBack(currentPanel);
        }
        currentPanel = uiPanels.Find(x => x.name == panelName);
        currentPanel.SetActive(true);
    }

    //Desc: Button function that hides the current panel and
    //      displays the given panel off the top of the queue
    //Param: None
    //Return: None
    public void backButtonPress()
    {
        currentPanel.SetActive(false);
        currentPanel = navHistory.Pop();
        if (EqualityComparer<GameObject>.Default.Equals(currentPanel, default(GameObject)))
        {
            Debug.LogError("FILO is empty");
        }
        else
        { 
            currentPanel.SetActive(true);
        }
    }

    //Desc: Button function that serves to stop the game
    //Param: None
    //Return: None
    public void quitGameButton()
    {
        Application.Quit();
    }
}
