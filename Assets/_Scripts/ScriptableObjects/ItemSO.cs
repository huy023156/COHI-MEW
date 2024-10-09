using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemSO")]
public class ItemSO : ScriptableObject
{
    public string nameString;
    public Sprite itemIcon;
    public Transform itemPrefab;
    public float generateTime;
    public float consumeTime;
}
