using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Biom", menuName = "WCObjects/Biom")]
public class Biom : ScriptableObject
{
    public Zone bioZone;
    [Range(0, 400)]
    public int averagePrecipitation;
    [Range(-10, 30)]
    public int averageTemperature;
}
