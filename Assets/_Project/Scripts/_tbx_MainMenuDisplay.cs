using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _tbx_MainMenuDisplay : MonoBehaviour
{
    public void StartHost()
    {
        _tbx_ServerManager.Instance.StartHost();
        
    }

    public void StartServer()
    {
        _tbx_ServerManager.Instance.StartServer();

    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}
