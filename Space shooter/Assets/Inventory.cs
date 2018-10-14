using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour {

    public GameObject itemPanel;
    public int money;
    private GridLayoutGroup grid;
    public Transform gridPanel;
    public int rows= 8;
    public int columns = 5;
    public TextMeshProUGUI balanceText;
    public List<GameObject> currentInventory;
    public bool isTradeInventory = false;
    public Trade trade;
    public bool isPlayerInventory = false;

	void Start () {
        gridPanel = transform.Find("GridPanel");
        grid = gridPanel.GetComponent<GridLayoutGroup>();
        for (int i = 0; i < (rows) * (columns); i++)
        {
            Instantiate(itemPanel, gridPanel);
        }
        for (int i = 0; i < currentInventory.Count; i++)
        {
            gridPanel.GetChild(i).GetComponent<ItemPanel>().AssignItem(currentInventory[i]);
        }

        UpdateMoney();
    }
	
	void Update () {
        grid.cellSize = new Vector2(gridPanel.GetComponent<RectTransform>().rect.width/columns, gridPanel.GetComponent<RectTransform>().rect.height/rows);
    }

    public void UpdateMoney()
    {
        if (isTradeInventory)
        {
            money = 0;
            foreach (GameObject item in currentInventory)
            {
                if (isPlayerInventory)
                {
                    money += item.GetComponent<ShipPart>().sellValue;
                }
                else if (!isPlayerInventory)
                {
                    money += item.GetComponent<ShipPart>().buyValue;
                }
            }
            trade.UpdateTradeBalance();
        }
        if(balanceText != null)
        balanceText.text = money + " Credits";
    }

    public void AddItem (GameObject item)
    {
        currentInventory.Add(item);
        UpdateMoney();
    }

    public void RemoveItem(GameObject item)
    {
        currentInventory.Remove(item);
        UpdateMoney();
    }
}
