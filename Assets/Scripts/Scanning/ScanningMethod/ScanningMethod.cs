
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScanningMethod : MonoBehaviour
{
    protected GameObject marker;
    protected ScanningMovement scanningMovement;
    protected CardComparator cardComparator;
    protected ArrayList elementList;
    protected GameObject markedElement;


    public static float waitTime = 0.5f;
    public static float moveTime = 0.5f;

    public void setWaitTime(float mwaitTime){
        waitTime = mwaitTime;
    }

    public void setMoveTime(float mmoveTime){
        moveTime = mmoveTime;
    }


    public abstract void start(GameObject marker, ScanningMovement scanningMovement);
    public abstract void stop();

    public void activate(GameObject marker, ScanningMovement scanningMovement)
    {
        this.scanningMovement = scanningMovement;
        this.marker = marker;
        cardComparator = GetComponent<CardComparator>();

        // Reset Position
        elementList = scanningMovement.getScanElements();
        GameObject _object = elementList[0] as GameObject;
        this.marker.transform.position = _object.transform.position;
        markedElement = _object;
    }
    public void shutdown(params object[] coroutinesToStop)
    {
        // Reset all values
        marker = null;
        scanningMovement = null;
        cardComparator = null;
        markedElement = null;
        elementList = null;
        scanningMovement = null;
        
        //StopAllCoroutines();
        foreach (Coroutine coroutine in coroutinesToStop)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }
    }

    protected IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        while (elapsedTime < seconds)
        {
            objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = end;
    }

}