
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class _tbx_PlayerCard : MonoBehaviour
{
    [SerializeField] private _tbx_CharacterDataBase characterDataBase;
    [SerializeField] private GameObject visuals;
    [SerializeField] private Image characterIconImage;
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private TMP_Text characterNameText;

    public void UpdateDisplay(_tbx_CharacterSelectState state)
    {
        if(state.CharacterId != -1)
        {
            var character = characterDataBase.GetCharacterById(state.CharacterId);
            characterIconImage.sprite = character.Icon;
            characterIconImage.enabled = true;
            characterNameText.text = character.DisplayName;
        }
        else
        {
            characterIconImage.enabled = false;
        }

        playerNameText.text = state.IsLockedIn ? $"Player {state.ClientId}": $"Player {state.ClientId} (Choosing...)";

        visuals.SetActive(true);
    }

    public void DisableDsplay()
    {
        visuals.SetActive(false);
    }
}
