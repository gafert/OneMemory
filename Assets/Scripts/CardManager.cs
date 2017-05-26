using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField]
    private GameObject cardPrefab;

    [SerializeField]
    private Sprite[] memorySprites; // Pictures of the cards

    // Default Values
    [SerializeField]
    private int numberOfCards = 8;
    [SerializeField]
    private int columns = 4; // Cards in a row (e.g 4 in a row with 8 cards -> 2 in a cloumn)
    [SerializeField]
    private float spacingBetweenCards = 0.4f;
    [SerializeField]
    private int secondsToShuffle = 1;

    // Calculated Values
    private ArrayList cards; // All Card GameObjects are saved here
    private float cardWidth; // For horizontal placing
    private float cardHeight; // For verdical placing




    // Getter and Setter
    public ArrayList getCards()
    {
        return cards;
    }

    public int getColumns(){
        return columns;
    }

    public int getNumberOfCards(){
        return numberOfCards;
    }

    public float getShuffleTime(){
        return secondsToShuffle;
    }




    /*
     * Places all cards in the right position and assignes sprites and ids
     */
    public void place(Transform parent)
    {
        // Limit card number
        if (numberOfCards % 2 != 0)
        {
            numberOfCards--;
        }
        if (numberOfCards / 2 > memorySprites.Length)
        {
            numberOfCards = memorySprites.Length * 2;
        }

        cards = new ArrayList();

        // Get size of Card
        cardWidth = GetRealSize(cardPrefab).size.x;
        cardHeight = GetRealSize(cardPrefab).size.z;

        // Position cards
        float xPos = 0;
        float zPos = 0;
        float cardRandomHeight = 0; // To stop flickering of merging edges

        // Instantiates all Cards and positions
        for (int i = 0; i < numberOfCards; i++)
        {
            cardRandomHeight = Random.Range(0f, 0.1f);
            cards.Add(Instantiate(cardPrefab, new Vector3(xPos, cardRandomHeight, zPos), Quaternion.identity, parent));
            xPos = xPos + spacingBetweenCards + cardWidth;
            // Make new row / colum
            if ((i + 1) % columns == 0)
            {
                zPos = zPos - spacingBetweenCards - cardHeight;
                xPos = 0;
            }
        }

        // Assign memorySprites and IDs to the Cards
        // IDs of the cards are equal to the sprite index
        int cardCounter = 0;
        for (int spriteID = 0; spriteID < memorySprites.Length; spriteID++)
        {

            // Only make cards when there is room for 2 more (+ index 0)
            if ((spriteID * 2 + 1) >= numberOfCards || cardCounter >= numberOfCards)
            {
                break;
            }

            // Make a pair of cards
            for (int l = 0; l < 2; l++)
            {
                GameObject card = cards[cardCounter] as GameObject;
                Card cardScript = card.GetComponent<Card>();
                cardScript.setSprite(memorySprites
                 [spriteID]);
                cardScript.setID(spriteID);
                cardCounter++;
            }
        }
    }




    // Starts the coroutine
    public void flipCards(bool up)
    {
        StartCoroutine(flipCardsRoutine(up));
    }

    // Flips all cards
    IEnumerator flipCardsRoutine(bool up)
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            GameObject _card = cards[i] as GameObject;
            _card.GetComponent<Card>().flipCard(up);
            yield return new WaitForSeconds(0.01f);
        }
    }

    /*
     * Shuffle the cards
     * shuffle activates coroutine
     */
    public void shuffle()
    {
        StartCoroutine(shuffleRoutine());
    }

    private IEnumerator shuffleRoutine()
    {
        // Randomise card indexes
        for (int i = 0; i < numberOfCards; i++)
        {
            var tempCard = cards[i];
            int randomIndex = Random.Range(i, numberOfCards);
            cards[i] = cards[randomIndex];
            cards[randomIndex] = tempCard;
        }

        float xPos = 0;
        float zPos = 0;

        // Change position according to new indexes
        for (int i = 0; i < numberOfCards; i++)
        {
            GameObject _card = cards[i] as GameObject;
            StartCoroutine(MoveOverSeconds(_card, new Vector3(xPos, _card.transform.localPosition.y, zPos), secondsToShuffle));
            xPos = xPos + spacingBetweenCards + cardWidth;
            if ((i + 1) % columns == 0)
            {
                zPos = zPos - spacingBetweenCards - cardHeight;
                xPos = 0;
            }
        }

        yield return new WaitForSeconds(secondsToShuffle);
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

    private IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 endPosition, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.localPosition;
        while (elapsedTime < seconds)
        {
            objectToMove.transform.localPosition = Vector3.Lerp(startingPos, endPosition, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.localPosition = endPosition;
    }
}
