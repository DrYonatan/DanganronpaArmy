using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TextEffect : ScriptableObject
{
    public abstract IEnumerator Apply(Transform target);
}
