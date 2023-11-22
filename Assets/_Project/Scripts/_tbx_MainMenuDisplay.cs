using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System;

#if UNITY_EDITOR
using ParrelSync;
#endif

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
            var options = new InitializationOptions();

            #if UNITY_EDITOR
            options.SetProfile(ClonesManager.IsClone() ? ClonesManager.GetArgument() : "PrimaryBuild");
            #endif

            await UnityServices.InitializeAsync(options);
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

    public async void StartClient()
    {
        await _tbx_ClientManager.Instance.StartClient(joinCodeInputField.text);
    }
}
