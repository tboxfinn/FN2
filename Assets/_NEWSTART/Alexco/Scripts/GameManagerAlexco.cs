using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton pattern

    public GameStates currentGameState;

    private int currentSceneIndex; // Índice de la escena actual

    // Este método se llama antes de que Start() en el primer frame
    private void Awake()
    {
        // Configurar el GameManager como un singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SetNewGameState(GameStates.inGame);
    }
    
    void Update() 
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
    }

    // Método para cargar una nueva escena por índice
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        currentSceneIndex = sceneIndex;
    }

    // Método para cargar una nueva escena por nombre
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        currentSceneIndex = SceneManager.GetSceneByName(sceneName).buildIndex;
    }

    // Método para obtener el índice de la escena actual
    public int GetCurrentSceneIndex()
    {
        return currentSceneIndex;
    }

    // Método para salir del juego
    public void QuitGame()
    {
        Application.Quit();
    }
    
    // Otros métodos para la gestión del juego pueden ir aquí
    public void SetNewGameState(GameStates newState){
        this.currentGameState = newState;

        switch(newState)
        {
            case GameStates.config:
                
            break;
            case GameStates.defeat:

            break;
            case GameStates.gameOver:

            break;
            case GameStates.inGame:

            break;
            case GameStates.mainMenu:

            break;
            case GameStates.pause:

            break;
            case GameStates.playerSelector:

            break;
        }
    }
}