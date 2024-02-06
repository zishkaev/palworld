using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;


[CreateAssetMenu(fileName = "ShopContent", menuName = "Shop/ShopContent")]
public class ShopContent :ScriptableObject
{
    [SerializeField] private List<WeapoonSkinItem> _weapoonSkinItems;
    [SerializeField] private List<PokemonSkinItem> _pokemonSkinItems;



    public IEnumerable<WeapoonSkinItem> WeapoonSkinItem => _weapoonSkinItems;
    public IEnumerable<PokemonSkinItem> PokemonSkinItem => _pokemonSkinItems;

    private void OnValidate()
    {
        var weaponsSkinsDublicates = _weapoonSkinItems.GroupBy(item =>item.WeapoonsType )//!Проверить item.Weapoons
            .Where(array => array.Count() >1 );

        if (weaponsSkinsDublicates.Count() > 0)
            throw new InvalidOperationException(nameof(_weapoonSkinItems));



        var pokemonsSkinsDublicates = _pokemonSkinItems.GroupBy(item => item.PokemonsType)//!Проверить item.Weapoons
           .Where(array => array.Count() > 1);

        if (pokemonsSkinsDublicates.Count() > 0)
            throw new InvalidOperationException(nameof(_pokemonSkinItems));




    }






}
