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

public class _chr_GUIManager : MonoBehaviour
{

    [Header ("UI Manager")]
    public StateMachine<_chr_GUIManager> Estados;
    public static _chr_GUIManager instance;

    [SerializeField] string EstadoActual = null;
    [Header ("Paneles")]
    public GameObject MainMenuPanel;
    public GameObject PauseGamePanel;
    public GameObject InGamePanel;
    public GameObject ConfigPanel;
    public GameObject PlayerSelectorPanel;
    public GameObject DefeatPanel;
    public GameObject GameOverPanel;

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
    }

    public void DesactivarPaneles(){
        MainMenuPanel.SetActive(false);
        PauseGamePanel.SetActive(false);
        InGamePanel.SetActive(false);
        ConfigPanel.SetActive(false);
        PlayerSelectorPanel.SetActive(false);
        DefeatPanel.SetActive(false);
        GameOverPanel.SetActive(false);
    }
}
