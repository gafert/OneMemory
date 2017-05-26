using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOverSeconds : MonoBehaviour
{

    public MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds)
    {
        StartCoroutine(move(objectToMove, end, seconds));
    }

    private IEnumerator move(GameObject objectToMove, Vector3 end, float seconds)
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
