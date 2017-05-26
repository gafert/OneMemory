using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScanningMovement : MonoBehaviour {

	protected ArrayList scanElements;
	protected CardManager cardManager;
	protected ArrayList allObjects;

	public abstract string getName();
	public abstract void start(CardManager cardManager);
	public abstract void stop();
	public abstract void action(GameObject element);
	public abstract ArrayList getScanElements();

	public int getNumOfElements(){
		return scanElements.Count;
	}

	protected void activate (CardManager cardManager) {
		this.cardManager = cardManager;
		this.allObjects = cardManager.getCards();
		this.scanElements = new ArrayList();
	}
	
	protected void shutdown () {
		cardManager = null;
		scanElements = null;
		allObjects = null;
	}

	// Used by rowSelection
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
