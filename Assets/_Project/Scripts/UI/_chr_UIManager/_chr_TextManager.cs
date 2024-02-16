using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextManager : MonoBehaviour
{
    // Entradas de Texto
    [Header ("Texto en Español")]
    [TextArea(15, 20)]
    [SerializeField] private string Spanish;

    [Header ("Texto en Inglés")]
    [TextArea(15,20)]
    [SerializeField] private string English;
    // Recursos
    [SerializeField] private languaje Language;
    //private GameManager gameManager;

    private TextMeshProUGUI TMPro;
    // Start is called before the first frame update
    void Start()
    {
        TMPro = GetComponent<TextMeshProUGUI>();
        //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Obtiene el lenguaje desde GameManager
        //Language = gameManager.GetComponent<GameManager>().Language.ToString();

        //Revisa qué idioma se está utilizando y utiliza el texto en el idioma seleccionado
        switch(Language){
            case (languaje.Spanish):
                TMPro.text = Spanish;
            break;

            case (languaje.English):
                TMPro.text = English;
            break;
        }
    }
}

public enum languaje{
    Spanish,
    English
}
