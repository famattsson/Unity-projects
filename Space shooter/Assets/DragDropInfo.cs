using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDropInfo : MonoBehaviour {

    GameObject itemBeingDragged;

    public void AssignDraggingItem (GameObject item)
    {
        itemBeingDragged = item;
    }

    public GameObject GetDraggingItem()
    {
        return itemBeingDragged;
    }

    public void RemoveDraggingItem()
    {
        itemBeingDragged = null;
    }
}
