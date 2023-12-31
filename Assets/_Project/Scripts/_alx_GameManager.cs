using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class _alx_GameManager : NetworkBehaviour
{
    public static _alx_GameManager singleton;
    public GameStates gameStates;

    [Header("Lista de clases")]
    // Esta lista se crea en caso de que en un futuro requiramos m�s clases
    public List<GameObject> listaClases = new List<GameObject>();


    private void Awake()
    {
        // Esto verifica que nunca se duplique para lograr que el singleton perdure en escenas
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetClassUtility()
    {
        for (int i=0;i<listaClases.Count;i++)
        {
            // Checa cual es utility
            if (listaClases[i].gameObject.CompareTag("Utility"))
            {
                // Aqu� instancia el prefab

            }
        }
    }

    public void SetClassAsault()
    {
        for (int i = 0; i < listaClases.Count; i++)
        {
            // Checa cual es utility
            if (listaClases[i].gameObject.CompareTag("Asault"))
            {
                // Aqu� instancia el prefab

            }
        }
    }

    public void SetClassSuport()
    {
        for (int i = 0; i < listaClases.Count; i++)
        {
            // Checa cual es utility
            if (listaClases[i].gameObject.CompareTag("Suport"))
            {
                // Aqu� instancia el prefab

            }
        }
    }

}

public enum GameStates
{
    // Aqu� se a�aden los estados del juego que se vayan a usar
    // Requiero ver con Portly como hacer esta parte si quiere que sea unica o netcode
    mainMenu,
    pause,

    // Estos tres tal vez de network behaviour
    playerSelector,
    defeat,    
    gameOver
}