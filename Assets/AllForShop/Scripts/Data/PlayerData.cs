using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    private CharacterWeapoons _selectedCharacterWeapoons;
    private Pokemons _selectedPokemons;

    private List<CharacterWeapoons> _openCharacterWeapoons;
    private List<Pokemons> _openPokemons;

    private int _money;


    public PlayerData()
    {
        _money = 10000;

        _selectedCharacterWeapoons = CharacterWeapoons.TheGun;
        _selectedPokemons = Pokemons.Poke_mon_1;

        _openCharacterWeapoons = new List<CharacterWeapoons> { _selectedCharacterWeapoons};
        _openPokemons = new List<Pokemons> {_selectedPokemons};

    }

    public int Money
    { 
        get=> _money;
        set
        {

            if(value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            _money = value;

        }
        
    
    }

    public CharacterWeapoons SelectedWeaponSkin
    {

        get => _selectedCharacterWeapoons;
        set
        {
            if(_openCharacterWeapoons.Contains(value) == false)
                throw new ArgumentOutOfRangeException(nameof(value));
            _selectedCharacterWeapoons = value;


        }

    }

    public Pokemons SelectedPokemonsSkin
    {

        get => _selectedPokemons;
        set
        {
            if (_openPokemons.Contains(value) == false)
                throw new ArgumentOutOfRangeException(nameof(value));
            _selectedPokemons = value;
        }
    }

    public IEnumerable<CharacterWeapoons> OpenWeaponSkins => _openCharacterWeapoons;
    public IEnumerable<Pokemons> OpenPokemons => _openPokemons;


    public void OpenWeaponSkin(CharacterWeapoons weapoons)
    {
        if (_openCharacterWeapoons.Contains(weapoons))
            throw new ArgumentException(nameof(weapoons));

        _openCharacterWeapoons.Add(weapoons);
    }
    public void OpenPokemonsSkin(Pokemons pokemons)
    {
        if (_openPokemons.Contains(pokemons))
            throw new ArgumentException(nameof(pokemons));

        _openPokemons.Add(pokemons);
    }






}
