using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPart : MonoBehaviour {
    [TextArea]
    public string description;


    public int sellValue;
    public int buyValue;

    [HideInInspector]
    public ItemImage itemImage;

    public void UnassignSelf()
    {
        itemImage.UnassignItem();
    }

    void Start () {

	}
	
	void Update () {
		
	}
}
