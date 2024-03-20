using System.Globalization;
using System.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using TMPro;

public class _chr_GUIManager : MonoBehaviour
{

    [Header ("UI Manager")]
    public StateMachine<_chr_GUIManager> Estados;
    public static _chr_GUIManager instance;

    [Header ("UI Manager Values")]
    public bool IsScreenWindowed;

    [SerializeField] string EstadoActual = null;
    [Header ("Paneles")]
    public GameObject MainMenuPanel;
    public GameObject PauseGamePanel;
    //public GameObject ExtensionPausePanel;
    public GameObject InGamePanel;
    public GameObject ConfigPanel;
    public GameObject PlayerSelectorPanel;
    public GameObject DefeatPanel;
    public GameObject GameOverPanel;

    [Header ("Componentes")]
    public TextMeshProUGUI WindowedStatus;
    public TMP_Dropdown resolucion;
    public _chr_CamShake CamShake;

    [Header ("Materiales UI")]
    [Space (15)]
    [SerializeField] private Material areaMat1;
    [SerializeField] private Material areaMat2;
    [SerializeField] private Material areaMat3;

    [Range (-10, 10)]
    public float LifeArea1;

    [Range (-10, 10)]
    public float LifeArea2;

    [Range (-10, 10)]

    public float LifeArea3;

    [Header ("Extras")]
    public float MagnitudCamShake;

    public void OnEnable()
    {
        
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
            
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        Estados = new StateMachine<_chr_GUIManager>(this);
        Estados.SetCurrentState(E1_Menu.instance);
    }

    void Update()
    {
        //Ui Manager
//        EstadoActual = _alx_GameManager.singleton.currentGameState.ToString();
        Estados.Updating();


        //Materiales UI
        areaMat1.SetFloat("_Cutoff_height", LifeArea1);
        areaMat2.SetFloat("_Cutoff_height", LifeArea2);
        areaMat3.SetFloat("_Cutoff_height", LifeArea3);

        //UI Manager Values

        
    }

    public void MainMenu(){
       // _alx_GameManager.singleton.SetNewGameState(GameStates.mainMenu);
        Estados.ChangeState(E1_Menu.instance);
    }

    public void PauseGame(){
        //_alx_GameManager.singleton.SetNewGameState(GameStates.pause);
        Estados.ChangeState(E2_Pause.instance);
    }

    public void InGame(){
        //_alx_GameManager.singleton.SetNewGameState(GameStates.inGame);
        Estados.ChangeState(E3_Game.instance); 
    }

    public void Config(){
        //_alx_GameManager.singleton.SetNewGameState(GameStates.config);
        Estados.ChangeState(E4_Config.instance);
    }

    public void PlayerSelector(){
        _alx_GameManager.singleton.SetNewGameState(GameStates.playerSelector);
    }

    public void Defeat(){
        _alx_GameManager.singleton.SetNewGameState(GameStates.defeat);
    }

    public void GameOver(){
        _alx_GameManager.singleton.SetNewGameState(GameStates.gameOver);
    }

    public void WindowedState(){
        IsScreenWindowed = !IsScreenWindowed;
    }

    //Acciones
    public void DañoRecibido(){
        DamagePool.instance.RequestDamage();
    }

    public void DesactivarPaneles(){
        MainMenuPanel.SetActive(false);
        PauseGamePanel.SetActive(false);
        //ExtensionPausePanel.SetActive(false);
        InGamePanel.SetActive(false);
        ConfigPanel.SetActive(false);
        PlayerSelectorPanel.SetActive(false);
        DefeatPanel.SetActive(false);
        GameOverPanel.SetActive(false);
    }
}
