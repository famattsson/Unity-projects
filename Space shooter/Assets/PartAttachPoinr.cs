using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartAttachPoinr : MonoBehaviour, IDropHandler {

    private DragDropInfo dragDropInfo;

    void Start()
    {
        dragDropInfo = GameObject.FindGameObjectWithTag("DragDropInfo").GetComponent<DragDropInfo>();
    }

    public void AssignItem(GameObject item)
    {
        transform.GetChild(0).GetComponent<ItemImage>().AssignItem(item);
    }

    public void OnDrop(PointerEventData eventData)
    {
        RectTransform itemPanel = transform as RectTransform;

        if (!RectTransformUtility.RectangleContainsScreenPoint(itemPanel, Input.mousePosition))
        {
            AssignItem(dragDropInfo.GetDraggingItem().GetComponent<ItemImage>().GetItem());
            dragDropInfo.GetDraggingItem().GetComponent<ItemImage>().UnassignItem();
        }
    }
}
