using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System;

public class _tbx_MainMenuDisplay : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject connectingPanel;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private TMP_InputField joinCodeInputField;

    private async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log($"Signed in anonymously, ID: {AuthenticationService.Instance.PlayerId}");
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return;
        }

        connectingPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void StartHost()
    {
        _tbx_HostManager.Instance.StartHost();
        
    }

    public void StartServer()
    {
        _tbx_HostManager.Instance.StartServer();

    }

    public void StartClient()
    {
        _tbx_ClientManager.Instance.StartClient(joinCodeInputField.text);
    }
}
