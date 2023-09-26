using UnityEngine;
using UnityEngine.UI;

public class _tbx_CharacterSelectButton : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    private _tbx_CharacterSelectDisplay characterSelect;
    private _tbx_Character character;

    public void SetCharacter(_tbx_CharacterSelectDisplay characterSelect, _tbx_Character character)
    {
        iconImage.sprite = character.Icon;

        this.characterSelect = characterSelect;
        this.character = character;
    }

    public void SelectCharacter()
    {
        characterSelect.Select(character);
    }
    
}
