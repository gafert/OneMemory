using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autoscan : ScanningMethod
{    
    private Coroutine moveRoutineRef;

    void Update()
    {
        if (markedElement != null)
        {
            if (Input.GetButtonDown("Action"))
            {
                // If the marker is on top of a card and action is pressed add it
                scanningMovement.action(markedElement);
            }
        }
    }
    
    public override void start(GameObject marker, ScanningMovement scanningMovement)
    {
        base.activate(marker, scanningMovement);

        // Autostart autoscan
        moveRoutineRef = StartCoroutine(moveRoutine());
    }

    public override void stop()
    {
        // Stop all couroutines and reset Class
        base.shutdown(moveRoutineRef);
    }

    // Reference is needed to start and stop this coroutine
    // Changes the position of the marker to the position of a card
    private IEnumerator moveRoutine()
    {
        bool hasCards = true; // To stop while loop from infinit loop
        while (hasCards)
        {       
            // Default has no cards -> if there are cards set to true and
            // continue loop 
            hasCards = false; 

            // Goes through all cards
            foreach (GameObject _object in elementList)
            {
                // Marker only moves to downwards facing cards only on Linear Selection mode
                if(scanningMovement.getName() == LinearSelection.NAME){
                    if (_object.GetComponent<Card>().isActive())
                    {
                        continue;
                    }
                }
                hasCards = true;
                markedElement = _object;
                
                // Position the marker slightly above the card to prevent flickering of merging faces (card and marker)
                yield return StartCoroutine(MoveOverSeconds(marker, new Vector3(_object.transform.position.x, _object.transform.position.y + 0.1f, _object.transform.position.z), moveTime));
                yield return new WaitForSeconds(waitTime);
            }
        }
    }

}
