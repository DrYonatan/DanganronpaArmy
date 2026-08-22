using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class SequenceEffect : RoomIntroEffect
{
    public RoomIntroEffect effect;
    public float delayBetweenEffects;

    public override IEnumerator PlayEffect()
    {
        if (effect == null)
            yield break;

        List<RoomIntroEffect> childEffects = new List<RoomIntroEffect>();
        float accumulatedDelay = 0f;

        foreach (Transform child in transform)
        {
            RoomIntroEffect childEffect = (RoomIntroEffect)child.gameObject.AddComponent(effect.GetType());
            CopyFields(effect, childEffect);
            accumulatedDelay += delayBetweenEffects;
            childEffect.delay += accumulatedDelay;
            childEffects.Add(childEffect);
        }

        yield return null;

        foreach (RoomIntroEffect childEffect in childEffects)
        {
            StartCoroutine(childEffect.PlayEffect());
        }
    }

    private static void CopyFields(RoomIntroEffect source, RoomIntroEffect destination)
    {
        foreach (FieldInfo field in source.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
        {
            field.SetValue(destination, field.GetValue(source));
        }
    }
}
