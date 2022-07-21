using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Plant Data", menuName = "WCObjects/Plant", order = 1)]
public class Plant : ScriptableObject
{
    public GameObject vegetationPrefab;
    public Zone biom;

    // TODO there just has to be a better solution to this shit
    
    public RenderTexture map {get; set;}

    [Range(0, 400)]
    public float precipitation;
    [Range(-10, 30)]
    public float temerature;
    public float radius;
    public float viability;
    public float priorityRadius;
    public float priority;
    
}
