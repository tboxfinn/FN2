using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Characters/Character")]
public class _tbx_Character : ScriptableObject
{
    [SerializeField] private int id = -1;
    [SerializeField] private string displayName = string.Empty;
    [SerializeField] private Sprite icon;
    [SerializeField] private GameObject introPrefab;
    [SerializeField] private NetworkObject gameplayPrefab;

    public int Id => id;
    public string DisplayName => displayName;
    public Sprite Icon => icon;
    public GameObject IntroPrefab => introPrefab;
    public NetworkObject GameplayPrefab => gameplayPrefab;
}
