using UnityEngine;

[CreateAssetMenu(fileName = "PokemonSkinItem", menuName = "Shop/PokemonSkinItem")]
public class PokemonSkinItem : ShopItem
{
    [field: SerializeField] public Pokemons PokemonsType { get; private set; }



}
