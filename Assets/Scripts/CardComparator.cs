using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardComparator : MonoBehaviour
{

    private GameObject card1;
    private GameObject card2;

    public void addCard(GameObject card)
    {
        // Only add card if its not upwards facing e.g. active
        if (!card.GetComponent<Card>().isActive())
        {
            if (card1 == card)
            {
                // Fast clicked, card already assigned
            }
            else if (card1 == null)
            {
                // Assign card
                card1 = card;
                card.GetComponent<Card>().flipCard(true);
            }
            else if (card2 == null)
            {
                // Assign card2
                card2 = card;
                card.GetComponent<Card>().flipCard(true);
                // All cards assignes -> compare
                StartCoroutine(compareCards());
            }
        }
    }

    private IEnumerator compareCards()
    {
        if (card1.GetComponent<Card>().getID() == card2.GetComponent<Card>().getID())
        {
            // Correct pair
            Debug.Log("Thats right!");
            GetComponent<AudioSource>().clip = GetComponent<GameMaster>().partyHorn;
            GetComponent<AudioSource>().Play();
            foreach (GameObject confetti in GameObject.FindGameObjectsWithTag("Confetti"))
            {
                confetti.GetComponent<ParticleSystem>().Play();
            }

            // Check if we won
            bool thereAreHiddenCards = false;
            foreach (GameObject card in GetComponent<CardManager>().getCards())
            {
                if (!card.GetComponent<Card>().isActive())
                {
                    thereAreHiddenCards = true;
                    Debug.Log("Cards active");
                }
            }
            if (!thereAreHiddenCards)
            {
                GetComponent<GameMaster>().resetGame();
                Debug.Log("No cards active");
            }

        }
        else
        {
            // Incorrect pair
            Debug.Log("Wrong Pair!");
            // Wait and then flip wrong cards
            yield return new WaitForSeconds(1f);
            card1.GetComponent<Card>().flipCard(false);
            card2.GetComponent<Card>().flipCard(false);
        }
        card1 = null;
        card2 = null;
    }

}
