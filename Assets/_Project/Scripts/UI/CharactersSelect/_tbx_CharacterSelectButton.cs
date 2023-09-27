using UnityEngine;
using UnityEngine.UI;

public class _tbx_CharacterSelectButton : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private GameObject disabledOverlay;
    [SerializeField] private Button button;
    
    private _tbx_CharacterSelectDisplay characterSelect;

    public _tbx_Character Character {get; private set;}
    public bool IsDisabled {get; private set;}

    public void SetCharacter(_tbx_CharacterSelectDisplay characterSelect, _tbx_Character character)
    {
        iconImage.sprite = character.Icon;

        this.characterSelect = characterSelect;
        Character = character;
    }

    public void SelectCharacter()
    {
        characterSelect.Select(Character);
    }

    public void SetDisabled()
    {
        IsDisabled = true;
        disabledOverlay.SetActive(true);
        button.interactable = false;
    }
    
}
