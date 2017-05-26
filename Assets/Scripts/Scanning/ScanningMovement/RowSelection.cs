using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowSelection : ScanningMovement
{
    public static string NAME = "RowSelection";

    private ArrayList row;
    private ArrayList raster;
    private bool inRow = false;
    private ArrayList rowArrayList;
    private int selectedRow = 0;

    public override string getName()
    {
        return NAME;
    }

    public override void start(CardManager cardManager)
    {
        base.activate(cardManager);
        raster = new ArrayList();
        inRow = false;
    }

    public override void stop()
    {
        inRow = false;
        rowArrayList = null;
        raster = null;
        row = null;
        selectedRow = 0;
        base.shutdown();
    }


    public override void action(GameObject element)
    {
        if (!inRow)
        {
            inRow = true;
            // Picking the row
            selectedRow = Int32.Parse(element.name);
            GetComponent<Marker>().getUsedScanningMethod().stop();
            GetComponent<Marker>().getUsedScanningMethod().start(GetComponent<Marker>().getMarker(), this);
        }
        else
        {
            inRow = false;
            // Picking the card
            GetComponent<CardComparator>().addCard((GameObject)element);
            GetComponent<Marker>().getUsedScanningMethod().stop();
            GetComponent<Marker>().getUsedScanningMethod().start(GetComponent<Marker>().getMarker(), this);
        }
    }

    // Activated by scanning method
    // To get the elements the cursor should move to
    public override ArrayList getScanElements()
    {
        if (inRow)
        {
            // Get the row
            scanElements = (ArrayList)raster[selectedRow];
            return (ArrayList)raster[selectedRow];
        }
        else
        {
            // Get the grid
            if (rowArrayList != null)
            {
                scanElements = rowArrayList;
                return rowArrayList;
            }
            else
                return getRows();
        }
    }


    public ArrayList getRows()
    {
        ArrayList rowArrayList = new ArrayList();
        int countedCards = 0;
        int totalNumOfCards = cardManager.getNumberOfCards();
        bool isCountingRows = true; // Is false when all cards are evaluated
        ArrayList cardArrayList = cardManager.getCards(); // Used to clone cards

        int rowCount = 0; // For the naming convention of the card Container
        while (isCountingRows)
        {

            // Only proceed if all cards are counted
            if (countedCards >= totalNumOfCards)
            {
                isCountingRows = false;
                break;
            }

            GameObject rowGameObject = new GameObject();
            int coutedColumn = 0;
            row = new ArrayList();
            for (coutedColumn = 0; coutedColumn < cardManager.getColumns(); coutedColumn++)
            {


                // Take the current card
                GameObject tmpCard = (GameObject)cardArrayList[countedCards];

                // Add it to the row Array List for further evaluation
                row.Add(tmpCard);

                // Make a clone of the card to measure its size
                GameObject cardClone = Instantiate(tmpCard, tmpCard.transform.position, Quaternion.identity);

                // If its the first card in the row make its position the position if the container
                if (coutedColumn == 0)
                {
                    rowGameObject.transform.position = cardClone.transform.position;
                }

                // Make the clone a child of the container
                cardClone.transform.parent = rowGameObject.transform;

                // Add 1 to the counted cards to keep track of how many cards are already evaluated
                countedCards++;
            }

            // Add the row to the main raster
            raster.Add(row);

            // Set name for identification
            rowGameObject.name = rowCount.ToString();
            rowGameObject.tag = "RowSelection";
            rowCount++;

            /* Center in cards
            rowGameObject.transform.position = new Vector3(
                 GetRealSize(rowGameObject).center.x,
                 rowGameObject.transform.position.y,
                 GetRealSize(rowGameObject).center.z); */

            // Remove clones
            for (int i = 0; i < rowGameObject.transform.childCount; i++)
            {
                Destroy(rowGameObject.transform.GetChild(i).gameObject);
            }

            // Add to the scanable elements
            rowArrayList.Add(rowGameObject);
        }

        this.rowArrayList = rowArrayList;
        scanElements = rowArrayList;
        return rowArrayList;
    }
}