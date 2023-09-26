using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Database", menuName = "Characters/Database")]
public class _tbx_CharacterDataBase : ScriptableObject
{
    [SerializeField] private _tbx_Character[] characters = new _tbx_Character[0];

    public _tbx_Character[] GetAllCharacters() => characters;

    public _tbx_Character GetCharacterById(int id)
    {
        foreach (_tbx_Character character in characters)
        {
            if (character.Id == id)
            {
                return character;
            }
        }

        return null;
    }
}
