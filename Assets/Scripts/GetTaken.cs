using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Own code written by Veronika
    Temporary until code is merged with Sofia's part
*/

public class GetTaken : MonoBehaviour
{
    public Item Item;
    public void Disappear()
    {
        InventoryManager.Instance.Add(Item);
        InventoryManager.Instance.refreshMenu();
        Destroy(gameObject);
    }
}
