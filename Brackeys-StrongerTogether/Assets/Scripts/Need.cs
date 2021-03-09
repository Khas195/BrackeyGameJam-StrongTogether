using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Need", menuName = "Data/Need")]
public class Need : ScriptableObject
{
    public Sprite icon;
    public float satisfactionValue;
}
