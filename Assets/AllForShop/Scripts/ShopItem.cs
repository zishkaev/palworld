using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class ShopItem : ScriptableObject
{
    [field:SerializeField] public GameObject Model {  get; private set; }
    [field:SerializeField] public Sprite Image {  get; private set; }
    [field:SerializeField, Range(0,1000)] public int Price {  get; private set; }





}
