using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class _alx_GameManager : NetworkBehaviour
{
    public static _alx_GameManager singleton;

    [Header("Estado actual")]
    public GameStates currentGameState;

    [Header("Lista de clases (Ahorita dejarlo en 0)")]
    // Esta lista se crea en caso de que en un futuro requiramos m�s clases
    public List<GameObject> clases = new List<GameObject>();


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

    private void Start() 
    {
        SetNewGameState(GameStates.inGame);    
    }

    private void Update() 
    {
        if(currentGameState == GameStates.inGame){
            if(Input.GetKeyDown(KeyCode.Escape)){
                SetNewGameState(GameStates.pause);
            }
        }
        else if (currentGameState == GameStates.pause)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                SetNewGameState(GameStates.inGame);
            }
        }

        Debug.LogWarning("Estado actual del juego: "+currentGameState);
    }

    public void SetClassUtility()
    {
        for (int i=0;i<clases.Count;i++)
        {
            // Checa cual es utility
            if (clases[i].gameObject.CompareTag("Utility"))
            {
                // Aqu� instancia el prefab

            }
        }
    }

    public void SetClassAsault()
    {
        for (int i = 0; i < clases.Count; i++)
        {
            // Checa cual es utility
            if (clases[i].gameObject.CompareTag("Asault"))
            {
                // Aqu� instancia el prefab

            }
        }
    }

    public void SetClassSuport()
    {
        for (int i = 0; i < clases.Count; i++)
        {
            // Checa cual es utility
            if (clases[i].gameObject.CompareTag("Suport"))
            {
                // Aqu� instancia el prefab

            }
        }
    }

    public void ExitGame(){
        // Para salir desde la pausa
        Application.Quit();
    }

    // Aqui se puede asignar un nuevo gamestate solo llamando la funcion mediante el singelton
    public void SetNewGameState(GameStates newGameState)
    {
        switch (newGameState)
        {
            case GameStates.mainMenu:
            _chr_GUIManager.instance.Estados.ChangeState(E1_Menu.instance);
            break;

            case GameStates.pause:
            _chr_GUIManager.instance.Estados.ChangeState(E2_Pause.instance);
            break;

            case GameStates.inGame:
            _chr_GUIManager.instance.Estados.ChangeState(E3_Game.instance);
            break;

            case GameStates.config:
            _chr_GUIManager.instance.Estados.ChangeState(E4_Config.instance);
            break;

            case GameStates.playerSelector:
            _chr_GUIManager.instance.Estados.ChangeState(E5_PlayerSelector.instance);
            break;

            case GameStates.defeat:
            _chr_GUIManager.instance.Estados.ChangeState(E6_Defeat.instance);
            break;

            case GameStates.gameOver:
            _chr_GUIManager.instance.Estados.ChangeState(E7_GameOver.instance);
            break;

        }
        currentGameState = newGameState;
    }
}

public enum GameStates
{
    // Aqu� se a�aden los estados del juego que se vayan a usar
    // Requiero ver con Portly como hacer esta parte si quiere que sea unica o netcode
    mainMenu,
    pause,
    inGame,
    config,

    // Estos tres tal vez de network behaviour
    playerSelector,
    defeat,    
    gameOver
}