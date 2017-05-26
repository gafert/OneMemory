using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Marker : MonoBehaviour
{

    public GameObject markerPrototype; // Marker prefab for instatiation
    private GameObject markerObject; // Actual marker in the worls

    // Scanning Methods
    public bool autoscan;
    public bool userscan;
    public bool stepscan;

    // Scanning Movement
    public bool linearScanning;
    public bool rowScanning;

    public GameObject settingsTab;

    private ScanningMethod usedScanningMethod;
    private ScanningMovement usedScanningMovement;
    private CardManager cardManager;

    // Use this for initialization
    void Start()
    {
        markerObject = Instantiate(markerPrototype);
        markerObject.SetActive(false);

        // Get scanning method
        if (autoscan)
        {
            usedScanningMethod = GetComponent<Autoscan>();
        }
        else if (userscan)
        {
            usedScanningMethod = GetComponent<Userscan>();
        }
        else if (stepscan)
        {
            usedScanningMethod = GetComponent<Stepscan>();
        }

        // Get scanning movement
        if (rowScanning)
        {
            usedScanningMovement = GetComponent<RowSelection>();
        }
        else if (linearScanning)
        {
            usedScanningMovement = GetComponent<LinearSelection>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Change scanning Method
        if (Input.GetButtonDown("Userscan"))
        {
            settingsTab.SetActive(true);
            GameObject.FindWithTag("UserscanCheckbox").GetComponent<Toggle>().isOn = true;
            //setUserscan(true);
        }
        if (Input.GetButtonDown("Stepscan"))
        {
            settingsTab.SetActive(true); 
            GameObject.FindWithTag("StepscanCheckbox").GetComponent<Toggle>().isOn = true;
            //setStepscan(true);
        }
        if (Input.GetButtonDown("Autoscan"))
        {
            settingsTab.SetActive(true); 
            GameObject.FindWithTag("AutoscanCheckbox").GetComponent<Toggle>().isOn = true;
            //setAutoscan(true);
        }

        // Scanning Movement
        if (Input.GetButtonDown("Row Selection"))
        {
            settingsTab.SetActive(true); 
            GameObject.FindWithTag("RowSelectionCheckbox").GetComponent<Toggle>().isOn = true;
            //setRowSelection(true);
        }
        if (Input.GetButtonDown("Linear Selection"))
        {
            settingsTab.SetActive(true); 
            GameObject.FindWithTag("LinearSelectionCheckbox").GetComponent<Toggle>().isOn = true;
            //setLinearSelection(true);
        }
    }


    public void setWaitTime(float waitTime)
    {
        usedScanningMethod.setWaitTime(waitTime);
    }

    public void setMoveTime(float moveTime)
    {
        usedScanningMethod.setMoveTime(moveTime);
    }


    public void setAutoscan(bool on)
    {
        if (on)
        {
            autoscan = true;
            stepscan = false;
            userscan = false;

            usedScanningMethod.stop();
            usedScanningMethod = GetComponent<Autoscan>();
            usedScanningMethod.start(markerObject, usedScanningMovement);
        }
    }

    public void setUserscan(bool on)
    {
        if (on)
        {
            autoscan = false;
            stepscan = false;
            userscan = true;

            usedScanningMethod.stop();
            usedScanningMethod = GetComponent<Userscan>();
            usedScanningMethod.start(markerObject, usedScanningMovement);
        }
    }

    public void setStepscan(bool on)
    {
        if (on)
        {
            autoscan = false;
            stepscan = true;
            userscan = false;

            usedScanningMethod.stop();
            usedScanningMethod = GetComponent<Stepscan>();
            usedScanningMethod.start(markerObject, usedScanningMovement);
        }
    }

    public void setRowSelection(bool on)
    {
        if (on)
        {
            rowScanning = true;
            linearScanning = false;

            usedScanningMethod.stop();
            usedScanningMovement.stop();
            usedScanningMovement = GetComponent<RowSelection>();
            usedScanningMovement.start(cardManager);
            usedScanningMethod.start(markerObject, usedScanningMovement);
        }
    }

    public void setLinearSelection(bool on)
    {
        if (on)
        {
            rowScanning = false;
            linearScanning = true;

            usedScanningMethod.stop();
            usedScanningMovement.stop();
            usedScanningMovement = GetComponent<LinearSelection>();
            usedScanningMovement.start(cardManager);
            usedScanningMethod.start(markerObject, usedScanningMovement);
        }
    }

    // Give the marker new positions according to the cardManager
    // Also hides and stops the marker
    public void activate(CardManager cardManager)
    {
        this.cardManager = cardManager;
        markerObject.SetActive(false);
        usedScanningMethod.stop();
        usedScanningMovement.stop();
        usedScanningMovement.start(cardManager);
    }

    public void move()
    {
        markerObject.SetActive(true);
        usedScanningMethod.start(markerObject, usedScanningMovement);
    }

    public ScanningMethod getUsedScanningMethod()
    {
        return usedScanningMethod;
    }

    public GameObject getMarker()
    {
        return markerObject;
    }

    // Shows the Marker

}
