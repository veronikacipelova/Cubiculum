using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    This code has been borrowed from this tutorial:
    https://www.youtube.com/watch?v=AoD_F1fSFFg
    from @Solo Game Dev
*/

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
}
