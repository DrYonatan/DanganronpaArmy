using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadioBoxAnimator : MonoBehaviour
{
    public RectTransform rectTransform;
    public List<Image> musicBars;

    [Header("Fill Range")]
    [Range(0f, 1f)] public float minFill = 0.1f;
    [Range(0f, 1f)] public float maxFill = 1f;

    [Header("Beat")]
    public float bpm = 120f;
    public float beatStrength = 0.6f;
    public float beatDecay = 8f;

    [Header("Noise")]
    public float noiseSpeed = 3f;
    public float noiseStrength = 0.4f;

    private float beatTimer;
    private float beatPulse;

    private float[] noiseOffsets;

    private bool isActive;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        noiseOffsets = new float[musicBars.Count];
        for (int i = 0; i < musicBars.Count; i++)
            noiseOffsets[i] = Random.Range(0f, 100f);
    }

    void Update()
    {
        if (isActive)
        {
            UpdateBeat();
            AnimateBars();
        }
    }

    void UpdateBeat()
    {
        float beatInterval = 60f / bpm;
        beatTimer += Time.deltaTime;

        if (beatTimer >= beatInterval)
        {
            beatTimer -= beatInterval;
            beatPulse = 1f; // beat hit
        }

        // fast decay = punchy beat
        beatPulse = Mathf.MoveTowards(beatPulse, 0f, Time.deltaTime * beatDecay);
    }

    public void AnimateBars()
    {
        for (int i = 0; i < musicBars.Count; i++)
        {
            float noise = Mathf.PerlinNoise(
                noiseOffsets[i],
                Time.time * noiseSpeed
            );

            float individual = noise * noiseStrength;
            float beat = beatPulse * beatStrength;

            float fill = minFill + individual + beat;

            musicBars[i].fillAmount = Mathf.Clamp01(fill);
        }
    }

    public void StartBars()
    {
        isActive = true;
    }
    
    public void StopBars()
    {
        foreach (var bar in musicBars)
            bar.fillAmount = 0f;
        isActive = false;
    }
}