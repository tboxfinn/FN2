using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using System;

public class _ply_NetworkButtons : MonoBehaviour
{
    public Button hostButton, clientButton;

    public void InitHost()
    {
        NetworkManager.Singleton.StartHost();
        hostButton.enabled = false;
        clientButton.enabled = false;
    }

    public void InitClient()
    {
        NetworkManager.Singleton.StartClient();
        hostButton.enabled = false;
        clientButton.enabled = false;
    }
}
