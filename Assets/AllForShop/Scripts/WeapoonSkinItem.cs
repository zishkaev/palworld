using UnityEngine;

[CreateAssetMenu(fileName = "WeapoonSkinItem", menuName = "Shop/WeapoonSkinItem")]

public class WeapoonSkinItem : ShopItem
{
   [field: SerializeField] public CharacterWeapoons WeapoonsType { get; private set; }





}
