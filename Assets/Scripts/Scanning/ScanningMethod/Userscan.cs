using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Userscan : ScanningMethod
{
    private Coroutine moveRoutineRef;
    private Coroutine dwellRoutineRef;

    void Update()
    {
        if (marker != null)
        {
            if (Input.GetButtonDown("Action"))
            {
                // Move and dont wait
                if (dwellRoutineRef != null)
                    StopCoroutine(dwellRoutineRef);
                moveRoutineRef = StartCoroutine(moveRoutine());
            }
            else if (Input.GetButtonUp("Action"))
            {
                // Wait and dont move
                if (moveRoutineRef != null)
                    StopCoroutine(moveRoutineRef);
                dwellRoutineRef = StartCoroutine(dwellRoutine());
            }
        }
    }

    public override void start(GameObject marker, ScanningMovement scanningMovement)
    {
        base.activate(marker, scanningMovement);
    }

    public override void stop()
    {
        // Nullify all set varibles
        base.shutdown(moveRoutineRef, dwellRoutineRef);
    }

    // Reference is needed to start and stop this coroutine
    // Changes the position of the marker to the position of a card
    private IEnumerator moveRoutine()
    {
        bool activeCards = true;
        while (activeCards)
        {
            activeCards = false;
            bool searchingForMarker = true;
            int cardIndex = 0;
            // Goes through all cards
            foreach (GameObject _object in elementList)
            {
                // Seaching for maker and move from that position
                /*cardIndex++;
                if (cardIndex > cards.numberOfCards)
                {
                    Debug.Log("Last Object");
                    activeCards = true;
                    break;   
                }

                if (searchingForMarker)
                {
                    // If the current object does not equal the card -> skip object
                    if (!markedCard.Equals(_object))
                    {
                        continue;
                    }
                    // The marker is the object -> stop searching
                    searchingForMarker = false;
                }*/

                // Marker only moves to downwards facing cards
                if(scanningMovement.getName() == LinearSelection.NAME){
                    if (_object.GetComponent<Card>().isActive())
                    {
                        continue;
                    }
                }
                activeCards = true;
                markedElement = _object;
                // Position the marker slightly above the card to prevent flickering of merging faces (card and marker)
                yield return StartCoroutine(MoveOverSeconds(marker, new Vector3(_object.transform.position.x, _object.transform.position.y + 0.1f, _object.transform.position.z), moveTime));
            }
        }
    }

    // Wait for time then select
    private IEnumerator dwellRoutine()
    {
        float elapsedTime = 0;
        while (elapsedTime < waitTime)
        {
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        if (markedElement != null)
        {
            scanningMovement.action(markedElement);
        }
    }
}