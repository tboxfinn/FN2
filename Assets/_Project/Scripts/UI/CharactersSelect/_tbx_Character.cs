using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Characters/Character")]
public class _tbx_Character : ScriptableObject
{
    [SerializeField] private int id = -1;
    [SerializeField] private string displayName = string.Empty;
    [SerializeField] private Sprite icon;
    //public GameObject characterPrefab;
    //public RuntimeAnimatorController characterAnimator;

    public int Id => id;
    public string DisplayName => displayName;
    public Sprite Icon => icon;
}
