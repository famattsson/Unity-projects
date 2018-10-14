using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemPanel : MonoBehaviour, IDropHandler, IPointerEnterHandler , IPointerExitHandler
{

    void Start()
    {
        
    }

    public void AssignItem(GameObject item, bool ignoreEmpty = false)
    {
        transform.GetChild(0).GetComponent<ItemImage>().AssignItem(item, ignoreEmpty);
    }
    
    public ItemImage GetItemImage()
    {
        return transform.GetChild(0).gameObject.GetComponent<ItemImage>();
    }


    public void OnDrop(PointerEventData eventData)
    {
        GetItemImage().SwapDragDropItems();         
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Image>().color = new Color(150, 0, 0);
        GetItemImage().OnPointerEnter(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().color = new Color(0, 0, 0);
        GetItemImage().OnPointerExit(eventData);
    }
}
