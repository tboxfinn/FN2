using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _alx_GameManager : MonoBehaviour
{
    public static _alx_GameManager singleton;

    private void Awake()
    {
        singleton = this;
    }

}
