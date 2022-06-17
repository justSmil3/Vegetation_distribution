using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Biom", menuName = "WCObjects/Biom")]
public class Biom : ScriptableObject
{
    [Range(0, 100)]
    public int precipitation;
    [Range(0, 100)]
    public int temperature;
}
