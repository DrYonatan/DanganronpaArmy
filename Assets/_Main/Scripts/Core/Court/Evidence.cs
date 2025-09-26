using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Evidence")]
public class Evidence : ScriptableObject
{
    public string Name;
    [TextArea]
    public string description;
}
