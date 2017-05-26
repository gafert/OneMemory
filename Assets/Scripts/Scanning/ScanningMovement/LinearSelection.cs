using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearSelection : ScanningMovement
{
	public static string NAME = "LinearSelection";
    public override string getName()
    {
        return NAME;
    }
    public override void start(CardManager cardManager)
    {
		base.activate(cardManager);
    }

    public override void stop()
    {
        base.shutdown();
    }

	public override void action(GameObject element){
		GetComponent<CardComparator>().addCard((GameObject)element);
	}
    public override ArrayList getScanElements()
    {
		scanElements = cardManager.getCards();
		return scanElements;
    }
}
