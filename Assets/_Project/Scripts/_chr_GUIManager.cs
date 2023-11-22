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
    [Space (15)]
    [SerializeField] private Material areaMat1, areaMat2, areaMat3;

    [Range (6, 26)]
    public float LifeArea1;

    [Range (6, 26)]
    public float LifeArea2;

    [Range (6, 26)]
    public float LifeArea3;

    public void OnEnable()
    {
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        areaMat1.SetFloat("_Cutoff_height", LifeArea1);
        areaMat2.SetFloat("_Cutoff_height", LifeArea2);
        areaMat3.SetFloat("_Cutoff_height", LifeArea3);
    }
}
