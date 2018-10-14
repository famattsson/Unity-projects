using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Trade : MonoBehaviour {

    public Inventory playerSection;
    public Inventory shopSection;
    public Inventory playerInventory;
    public Inventory shopInventory;
    public int balance;
    public TextMeshProUGUI balanceText;

	// Use this for initialization
	void Start () {
        balance = 0;
        playerSection.isPlayerInventory = true;
        playerSection.isTradeInventory = true;
        playerSection.trade = this;
        shopSection.trade = this;
        shopSection.isPlayerInventory = false;
        shopSection.isTradeInventory = true;
        UpdateTradeBalance();
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void ConfirmTrade()
    {
        if (Mathf.Abs(balance) <= playerInventory.money || balance >= 0)
        {
            playerInventory.money += balance;
            playerInventory.UpdateMoney();
        }
        else
        {
            balanceText.text = "You do not have enough credits!";
            return;
        }
        SwitchInventory(shopSection, playerInventory);
        SwitchInventory(playerSection, shopInventory);
    }

    public void UpdateTradeBalance()
    {
        balance = playerSection.money - shopSection.money;
        balanceText.text = balance + " Credits";
	}

    private void SwitchInventory(Inventory fromInv, Inventory toInv)
    {
        for (int i = 0; i < fromInv.currentInventory.Count; i++)
        {
            GameObject item = fromInv.currentInventory[i];
            ItemPanel[] itemPanels = toInv.gridPanel.transform.GetComponentsInChildren<ItemPanel>();
            for (int j = 0; j < itemPanels.Length; j++)
            {
                Debug.Log(itemPanels[j]);
                if (itemPanels[j].GetItemImage().IsEmpty())
                {
                    item.GetComponent<ShipPart>().UnassignSelf();
                    i--;
                    itemPanels[j].GetItemImage().AssignItem(item);
                    j++;
                    Debug.Log("Found empty slot");
                    break;
                }
            }
        }
    }
}
