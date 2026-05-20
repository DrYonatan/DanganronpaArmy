using System.Collections;
using UnityEngine;

public abstract class RoomIntroEffect : MonoBehaviour
{
    public float delay;
    public abstract IEnumerator PlayEffect();
}
