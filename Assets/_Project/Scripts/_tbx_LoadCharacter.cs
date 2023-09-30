using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class _tbx_LoadCharacter : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    public Transform[] spawnPoints;
    public TMP_Text label;

    void Start()
    {
        int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");
        GameObject prefab = characterPrefabs[selectedCharacter];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject clone = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        label.text = prefab.name;
    }
}