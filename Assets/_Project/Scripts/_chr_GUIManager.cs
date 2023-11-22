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
    [Header("GUI States")]
    public GUIState GUIStat;
    [Space(15)]
    [Header("Telon")]
    [SerializeField] PlayableDirector TelonAmim;
    [Space(15)]
    [Header ("Menu Panel")]
    [SerializeField] GameObject MenuPanel;
    [SerializeField] PlayableDirector StartBtnAnim;

    [Space(15)]
    [Header("Character Selector Panel")]
    [SerializeField] GameObject CharacterSelectorPanel;

    [Space(15)]
    [Header("Other Stuff")]
    [SerializeField] bool isCoroutineRunning;

    [Space (15)]
    [SerializeField] private Material areaMat1, areaMat2, areaMat3;

    public void OnEnable()
    {
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        switch(GUIStat){
            case GUIState.Menu:
            //TelonAnim.SetTrigger("AnimCompleted");
            break;
        }
    }

    public void StartGame(){

        StartBtnAnim.Play();
            // StartCoroutine(EsperaYRealizaAccion(0.5f, () => {
            //     start
            //     CharacterSelectorPanel.SetActive(true);
            // }));
    }


    //Este IEnumerator permite el poder atrasar las acciones para que las animaciones fluyan
    private IEnumerator EsperaYRealizaAccion(float tiempoDeEspera, Action accion)
    {
        if (isCoroutineRunning)
        {
            yield break; // Si ya hay una instancia de la corrutina ejecutándose, no inicies otra.
        }

        isCoroutineRunning = true;

        yield return new WaitForSeconds(tiempoDeEspera);

        // Llama a la acción después del tiempo de espera.
        accion.Invoke();

        isCoroutineRunning = false; // Marca la corrutina como finalizada.
    }
}

public enum GUIState{
    Menu,
    CharacterSelector,
    Pause
}
