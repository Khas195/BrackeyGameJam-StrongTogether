using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Need Data", menuName = "Data/Need Type", order = 1)]

public class NeedData : ScriptableObject
{
    [SerializeField]
    [ShowAssetPreview]
    public Sprite icon;
    [SerializeField]
    float satisfyInterval = 0;
    [SerializeField]
    float satisfyAmount = 0;

    public float SatisfyInterval { get => satisfyInterval; }
    public float SatisfyAmount { get => satisfyAmount; }
}
