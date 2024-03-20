using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;

public class _chr_OnHoverBtns : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ButtonType BtnType;

    [Header("Normal Buttons (Resume, Quit, Config, Etc)")]
    [SerializeField] private UnityEngine.UI.Image AuraNormalBtn;

    [Header("Character Buttons")]
    [SerializeField] private UnityEngine.UI.Image AuraCharacterBtn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        switch(BtnType){
            case ButtonType.Normal:
            AuraNormalBtn.color = new Color(AuraNormalBtn.color.r, AuraNormalBtn.color.g, AuraNormalBtn.color.b, 1f);
            break;

            case ButtonType.Character:
            AuraCharacterBtn.color = new Color(AuraNormalBtn.color.r, AuraNormalBtn.color.g, AuraNormalBtn.color.b, 1f);
            break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
            switch(BtnType){
            case ButtonType.Normal:
            AuraNormalBtn.color = new Color(AuraNormalBtn.color.r, AuraNormalBtn.color.g, AuraNormalBtn.color.b, 0.5f);
            break;

            case ButtonType.Character:
            AuraCharacterBtn.color = new Color(AuraNormalBtn.color.r, AuraNormalBtn.color.g, AuraNormalBtn.color.b, 0f);
            break;
        }
    }
}

public enum ButtonType{
    Normal,
    Character
}
