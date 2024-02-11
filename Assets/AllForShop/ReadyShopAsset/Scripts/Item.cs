using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public ShopItem i;
    public int amountMin;
    public int amountMax;
    public float price;

    [HideInInspector]
    public int finalAmount;
    [HideInInspector]
    public int position;
    [HideInInspector] 
    public bool randomSelected;
}
