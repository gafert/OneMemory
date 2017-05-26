using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stepscan : ScanningMethod
{
    private Coroutine dwellRoutineRef;
    private Coroutine moveRoutineRef;
    private int cardIndex = 0;

    void Update()
    {
        if (Input.GetButtonDown("Action"))
        {
            if (markedElement != null)
            {
                // Move to next card and stop the dwell time
                if (dwellRoutineRef != null)
                {
                    StopCoroutine(dwellRoutineRef);
                }
                moveRoutineRef = StartCoroutine(moveRoutine());
            }
        }
    }

    public override void start(GameObject marker, ScanningMovement scanningMovement)
    {
        base.activate(marker, scanningMovement);

    }

    public override void stop()
    {
        base.shutdown(moveRoutineRef, dwellRoutineRef);
        // Reset marker position to 0
        cardIndex = 0;
    }

    private IEnumerator moveRoutine()
    {
        bool searchingForNotActiveCard = true;
        while (searchingForNotActiveCard)
        {
            cardIndex++;
            if (cardIndex >= scanningMovement.getNumOfElements())
            {
                cardIndex = 0;
            }

            GameObject _object = (GameObject)elementList[cardIndex];
            // Marker only moves to downwards facing cards
            if (scanningMovement.getName() == LinearSelection.NAME)
            {
                if (_object.GetComponent<Card>().isActive())
                {
                    continue;
                }
            }

            // The current object is not active -> select it
            searchingForNotActiveCard = false;
            markedElement = _object;
            yield return StartCoroutine(MoveOverSeconds(marker, new Vector3(_object.transform.position.x, _object.transform.position.y + 0.1f, _object.transform.position.z), moveTime));
            dwellRoutineRef = StartCoroutine(dwellRoutine());
        }
    }

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
