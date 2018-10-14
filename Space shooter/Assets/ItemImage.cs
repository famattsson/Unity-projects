using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemImage : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler {

    private GameObject item;
    private GameObject oldItem;
    private DragDropInfo dragDropInfo;
    public Sprite emptySprite;
    private CanvasGroup canvasGroup;
    public float hoverDescDelay = 0.1f;
    private GameObject DescPanel;
    private TextMeshProUGUI text;
    private Inventory inventory;

    public bool IsEmpty()
    {
        if (GetComponent<Image>().sprite == emptySprite)
            return true;
        else
            return false;
    }

    public GameObject GetItem ()
    {
        return item;
    }

    public void UnassignItem()
    {
        if(!IsEmpty())
        {
            if (inventory != null)
            {
                inventory.RemoveItem(item);
            }
            item = null;
            GetComponent<Image>().sprite = emptySprite;
        }
    }
    public GameObject GetOldItem()
    {
        return oldItem;
    }


    public void SwapDragDropItems()
    {
        if(dragDropInfo.GetDraggingItem() != null)
        {
            oldItem = item;
            AssignItem(dragDropInfo.GetDraggingItem().GetComponent<ItemPanel>().GetItemImage().GetItem(), true);
            dragDropInfo.GetDraggingItem().GetComponent<ItemPanel>().GetItemImage().AssignItem(oldItem, true);
        }
    }
    public void SwapItems(ItemImage item, ItemImage item2)
    {
        oldItem = this.item;
        AssignItem(item.GetItem(), true);
        item2.AssignItem(oldItem, true);
    }

    public void AssignItem (GameObject item, bool ignoreEmpty = false)
    {
        if(IsEmpty() || ignoreEmpty)
        {
            this.item = item;

            if (item != null)
            {
                if(DescPanel != null)
                {
                    text = DescPanel.GetComponentInChildren<TextMeshProUGUI>();
                    text.text = item.GetComponent<ShipPart>().description;
                }
                GetComponent<Image>().sprite = item.GetComponent<SpriteRenderer>().sprite;
                if(inventory != null)
                {
                    inventory.AddItem(item);
                }
                item.GetComponent<ShipPart>().itemImage = this;
            }
            else
            {
                GetComponent<Image>().sprite = emptySprite;
                if (inventory != null)
                {
                    inventory.RemoveItem(oldItem);
                }
            }
        }        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!IsEmpty())
        {
            transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        transform.localPosition = Vector2.zero;
        dragDropInfo.AssignDraggingItem(null);
    }

    void Start () {

        dragDropInfo = GameObject.FindGameObjectWithTag("DragDropInfo").GetComponent<DragDropInfo>();
        DescPanel = transform.GetChild(0).gameObject;
        canvasGroup = GetComponent<CanvasGroup>();
        if(item != null)
        {
            text = DescPanel.GetComponentInChildren<TextMeshProUGUI>();
            text.text = item.GetComponent<ShipPart>().description;
        }
        DescPanel.SetActive(false);
        inventory = GetComponentInParent<Inventory>();
    }

    void Update () {
		
	}

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!IsEmpty())
        {
            oldItem = item;
            canvasGroup.blocksRaycasts = false;
            dragDropInfo.AssignDraggingItem(transform.parent.gameObject);
        }
    }


    IEnumerator ShowDescription()
    {
        yield return new WaitForSeconds(0.1f);
        DescPanel.SetActive(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!IsEmpty() && dragDropInfo.GetDraggingItem() == null)
        {
            StartCoroutine(ShowDescription());
            GetComponent<Canvas>().sortingOrder = 2;
//            DescPanel.transform.SetParent(transform.parent.parent.parent.parent.parent.parent);
//            DescPanel.transform.SetAsLastSibling();
            DescPanel.SetActive(true);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Canvas>().sortingOrder = 1;
        StopAllCoroutines();
        DescPanel.SetActive(false);
//        DescPanel.transform.SetParent(transform);
    }
}
