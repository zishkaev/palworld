using System;
using UnityEngine;

[CreateAssetMenu(fileName ="ShopItemViewFactory", menuName =  "Shop/ShopItemViewFactory")]
public class ShopItemViewFactory : ScriptableObject
{
    [SerializeField] private ShopItemView _weaponSkinItemPrefab;
    [SerializeField] private ShopItemView _pokemonSkinItemPrefab;


    public ShopItemView Get(ShopItem shopItem, Transform parent)
    {
        ShopItemView instance;

        switch(shopItem)
        {
            case WeapoonSkinItem weapoonSkinItem:
                instance = Instantiate(_weaponSkinItemPrefab, parent);
                break;

            case PokemonSkinItem pokemonSkinItem:
                instance = Instantiate(_pokemonSkinItemPrefab, parent);
                break;

            default:
                throw new ArgumentException(nameof(shopItem));

        }

        instance.Initialize(shopItem);
        return instance;

    }


}
