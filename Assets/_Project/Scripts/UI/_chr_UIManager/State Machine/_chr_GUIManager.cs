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

    [SerializeField] string EstadoActual = null;
    [Header ("Paneles")]
    [SerializeField] GameObject MainMenuPanel;
    [SerializeField] GameObject PauseGamePanel;
    [SerializeField] GameObject InGamePanel;
    [SerializeField] GameObject ConfigPanel;
    [SerializeField] GameObject PlayerSelectorPanel;
    [SerializeField] GameObject DefeatPanel;
    [SerializeField] GameObject GameOverPanel;

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

    void Start()
    {
        
    }

    void Update()
    {
        //Ui Manager
        EstadoActual = _alx_GameManager.singleton.currentGameState.ToString();


        //Materiales UI
        areaMat1.SetFloat("_Cutoff_height", LifeArea1);
        areaMat2.SetFloat("_Cutoff_height", LifeArea2);
        areaMat3.SetFloat("_Cutoff_height", LifeArea3);
    }

    public void DesactivarPaneles(){
        
    }

    public void MainMenu(){

    }

    public void PauseGame(){

    }

    public void InGame(){

    }

    public void Config(){
        
    }
}
