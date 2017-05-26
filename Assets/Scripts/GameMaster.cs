using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour
{

    public AudioClip partyHorn; // AudioClip used by CardComparator of the cards match
    private GameObject cardContainer; // Contains all cards
    private Marker marker; // Marker Object 
    private CardManager cardManager; // Card Object containing all cards and functions of them

    // Use this for initialization
    void Start()
    {
        marker = GetComponent<Marker>();
        cardManager = GetComponent<CardManager>();

        // Time the game
        StartCoroutine(gameHandler());

        // Hide the Settings text if on Android
#if UNITY_ANDROID
			GameObject.FindGameObjectWithTag("DesktopSettings").SetActive(false);
#endif
    }

    void Update()
    {
        if (Input.GetButtonDown("Quit"))
        {
            Application.Quit();
        }
    }

    IEnumerator gameHandler()
    {
        // Place the cards in the container
        cardContainer = new GameObject("CardContainer");
        cardManager.place(cardContainer.transform);
        // Center the container in the screen
        // 0.3f is the height of the container --> just for shadow effects
        cardContainer.transform.position = new Vector3(0, 0.3f, 0) - GetRealSize(cardContainer).center;
        yield return new WaitForSeconds(1);
        cardManager.flipCards(false);
        yield return new WaitForSeconds(1);
        cardManager.shuffle();
        yield return new WaitForSeconds(cardManager.getShuffleTime() + 1);
        marker.activate(cardManager);
        marker.move();
    }

    public void resetGame()
    {
        marker.activate(cardManager);
        StartCoroutine(resetGameRoutine());
    }

    private IEnumerator resetGameRoutine()
    {
        // Rotate and move the camera to see the winning screen
        StartCoroutine(RotateOverSeconds(GameObject.FindGameObjectWithTag("MainCamera"), Quaternion.Euler(50, 0, 0), 1f));
        StartCoroutine(MoveOverSeconds(GameObject.FindGameObjectWithTag("MainCamera"), new Vector3(0, 10, -7), 1));
        yield return new WaitForSeconds(3);
        // Wait and rotate it back to position facing the cards
        StartCoroutine(RotateOverSeconds(GameObject.FindGameObjectWithTag("MainCamera"), Quaternion.Euler(90, 0, 0), 1f));
        StartCoroutine(MoveOverSeconds(GameObject.FindGameObjectWithTag("MainCamera"), new Vector3(0, 13, 0), 1));
        cardManager.flipCards(false);
        yield return new WaitForSeconds(1);
        cardManager.shuffle();
        yield return new WaitForSeconds(cardManager.getShuffleTime() + 1);
        marker.move();
    }

    // General function to rotate objects
    private IEnumerator RotateOverSeconds(GameObject objectToRotate, Quaternion end, float seconds)
    {
        float elapsedTime = 0;
        Quaternion startingPos = objectToRotate.transform.rotation;
        while (elapsedTime < seconds)
        {
            objectToRotate.transform.rotation = Quaternion.Slerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToRotate.transform.rotation = end;
    }

    // General funciton to move objects
    private IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 endPosition, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        while (elapsedTime < seconds)
        {
            objectToMove.transform.position = Vector3.Lerp(startingPos, endPosition, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = endPosition;
    }

    // General functions to get the size of objects
    public static Bounds GetRealSize(GameObject parent)
    {
        MeshFilter[] childrens = parent.GetComponentsInChildren<MeshFilter>();

        Vector3 minV = childrens[0].transform.position - MultVect(childrens[0].sharedMesh.bounds.size, childrens[0].transform.localScale) / 2;
        Vector3 maxV = childrens[0].transform.position + MultVect(childrens[0].sharedMesh.bounds.size, childrens[0].transform.localScale) / 2;
        for (int i = 1; i < childrens.Length; i++)
        {
            maxV = Vector3.Max(maxV, childrens[i].transform.position + MultVect(childrens[i].sharedMesh.bounds.size, childrens[i].transform.localScale) / 2);
            minV = Vector3.Min(minV, childrens[i].transform.position - MultVect(childrens[i].sharedMesh.bounds.size, childrens[i].transform.localScale) / 2);
        }
        Vector3 v3 = maxV - minV;
        return new Bounds(minV + v3 / 2, v3);
    }

    private static Vector3 MultVect(Vector3 a, Vector3 b)
    {
        a.x *= b.x;
        a.y *= b.y;
        a.z *= b.z;
        return a;
    }

}
