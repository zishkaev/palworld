//using UnityEngine;
//using System.Linq;
//using System;



//public class Shop : MonoBehaviour
//{

//    [SerializeField] private ShopContent _contentItems;

//    [SerializeField] private ShopCategoryButton _charactersWeaponsButton;
//    [SerializeField] private ShopCategoryButton _pokemonsButton;

//    [SerializeField] private ShopPanel _shopPanel;

//    private void OnEnable()
//    {
//        _charactersWeaponsButton.Click += OnCharactersWeaponsButtonClick;
//        _pokemonsButton.Click += OnPokemonsButtonClick;

//    }

//    private void OnDisable()
//    {

//        _charactersWeaponsButton.Click -= OnCharactersWeaponsButtonClick;
//        _pokemonsButton.Click -= OnPokemonsButtonClick;

//    }



//    private void OnPokemonsButtonClick()
//    {
//        _pokemonsButton.Select();
//        _charactersWeaponsButton.Unselect();
//        _shopPanel.Show(_contentItems.PokemonSkinItem.Cast<ShopItem>());
//    }

//    private void OnCharactersWeaponsButtonClick()
//    {

//        _charactersWeaponsButton.Unselect();
//        _pokemonsButton.Select();
//        _shopPanel.Show(_contentItems.WeapoonSkinItem.Cast<ShopItem>());
//    }


//}
