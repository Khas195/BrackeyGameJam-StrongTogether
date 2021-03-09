using NaughtyAttributes;
using UnityEngine;

public class NeedData : ScriptableObject
{
    [SerializeField]
    [ShowAssetPreview]
    Sprite icon;
    [SerializeField]
    float satisfyInterval = 0;
    [SerializeField]
    float satisfyAmount = 0;

    public float SatisfyInterval { get => satisfyInterval; }
    public float SatisfyAmount { get => satisfyAmount; }
}
