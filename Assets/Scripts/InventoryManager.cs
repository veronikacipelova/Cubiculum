using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    Smaller part of the code - mainly parts related to scriptable object borrowed from @
Solo Game Dev
    Tutorial link: https://www.youtube.com/watch?v=AoD_F1fSFFg
    Larger part is own code written by Veronika
    Most of the code won't change, needs to be merged with Sofia's part
*/

public class InventoryManager : MonoBehaviour
{
    private Outline OutlineSelected;
    private int MenuActive;
    private int ItemAmount;
    private int PlayerChosenItem;
    private int SelectedItem;
    private List<GameObject> ItemsInstances = new List<GameObject>();
    public GameObject InventoryMenu;
    public static InventoryManager Instance;
    public List<Item> Items = new List<Item>();

    public Transform ItemContent;
    public GameObject InventoryItem;

    private void Start()
    {
        InventoryMenu.SetActive(false);
        MenuActive = 0;
        ItemAmount = 0;
        PlayerChosenItem = -1;
        SelectedItem = 0;
    }

    private void Awake() 
    {
        Instance = this;    
    }

    public List<Item> getItems() {
        return Items;
    }

    public void Add(Item item)
    {
        ItemAmount++;
        Items.Add(item);
    }

    public void Remove(Item item)
    {
        ItemAmount--;
        if(Items.IndexOf(item) == PlayerChosenItem)
            PlayerChosenItem = -1;
        else if(Items.IndexOf(item) < PlayerChosenItem)
        {
            PlayerChosenItem--;
        }
        Items.Remove(item);
        refreshMenu();
    }

    private void ListItems()
    {
        ItemsInstances.Clear();

        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in Items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            itemIcon.sprite = item.icon;
            ItemsInstances.Add(obj);
        }
    }

    public void showInventory()
    {
        if(MenuActive == 0)
        {
            InventoryMenu.SetActive(true);
            if(Items.Count != 0)
            {
                ListItems();
                HighlightItem(SelectedItem, Color.yellow);
                if(PlayerChosenItem != -1)
                    HighlightItem(PlayerChosenItem, Color.green);
            }
            MenuActive = 1;
        }
        else
        {
            InventoryMenu.SetActive(false);
            MenuActive = 0;
            SelectedItem = 0;
        }

    }

    public void refreshMenu()
    {
        if(MenuActive == 1)
        {
            ListItems();
            SelectedItem = 0;
            HighlightItem(SelectedItem, Color.yellow);
            if(PlayerChosenItem != -1)
                HighlightItem(PlayerChosenItem, Color.green);
        }
    }

    public void HighlightNextItem(char a)
    {
        if(MenuActive == 1 && ItemAmount > 1)
        {
            var PreviousSelected = SelectedItem;
            if(ItemsInstances[PreviousSelected%ItemAmount].GetComponent<Outline>().effectColor != Color.green)
                HighlightItem(PreviousSelected, Color.gray);
            if(a == 'F')
            {
                SelectedItem++;
            }
            else if(a == 'R')
                SelectedItem = SelectedItem-1+ItemAmount;
            Debug.Log("SelectedItem: "+SelectedItem);
            Debug.Log("Selected modulo "+SelectedItem%ItemAmount);
            Debug.Log("ItemAMount: " + ItemAmount);
            Debug.Log("Color"+ItemsInstances[SelectedItem%ItemAmount].GetComponent<Outline>().effectColor);
            if(ItemsInstances[SelectedItem%ItemAmount].GetComponent<Outline>().effectColor != Color.green)
                HighlightItem(SelectedItem, Color.yellow);
            Debug.Log("Color"+ItemsInstances[SelectedItem%ItemAmount].GetComponent<Outline>().effectColor);
        }
        if(SelectedItem > 500)
            SelectedItem = SelectedItem % ItemAmount;
    }

    private void HighlightItem(int ItemPos, Color color)
    {
        if(ItemAmount != 0)
            ItemsInstances[ItemPos%ItemAmount].GetComponent<Outline>().effectColor = color;
    }

    public void ConfirmSelected()
    {
        if(MenuActive == 1 && ItemAmount > 0)
        {
            PlayerChosenItem = PlayerChosenItem % ItemAmount;
            SelectedItem = SelectedItem % ItemAmount;
            if(PlayerChosenItem != -1 && PlayerChosenItem != SelectedItem)
                ItemsInstances[PlayerChosenItem%ItemAmount].GetComponent<Outline>().effectColor = Color.gray;
            else if(PlayerChosenItem == SelectedItem)
            {
                ItemsInstances[PlayerChosenItem%ItemAmount].GetComponent<Outline>().effectColor = Color.gray;
                PlayerChosenItem = -1;
                return;
            }
            PlayerChosenItem = SelectedItem;
            ItemsInstances[PlayerChosenItem%ItemAmount].GetComponent<Outline>().effectColor = Color.green;
        }
    }

}
