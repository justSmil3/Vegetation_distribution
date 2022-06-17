using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Plant Data", menuName = "WCObjects/Plant", order = 1)]
public class Plant : ScriptableObject
{
    public GameObject vegetationPrefab;

    public float radius;
    public float viability;
    public float priorityRadius;
    public float priority;
}
