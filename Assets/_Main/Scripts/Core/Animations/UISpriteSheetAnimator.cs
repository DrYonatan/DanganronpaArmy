using UnityEngine;
using UnityEngine.UI;

public class UISpriteSheetAnimator : MonoBehaviour
{
    public string spriteSheetName; // Name of the sprite sheet file (without extension)
    public float frameRate = 10f;

    private Sprite[] frames;
    private Image image;
    private int currentFrame;
    private float timer;

    void Start()
    {
        image = GetComponent<Image>();

        // Load all sliced sprites from the sprite sheet in Resources folder
        frames = Resources.LoadAll<Sprite>("Images/" + spriteSheetName);

        if (frames == null || frames.Length == 0)
        {
            Debug.LogError("No sprites found. Make sure your sprite sheet is sliced and located in a Resources folder.");
        }
    }

    void Update()
    {
        if (frames == null || frames.Length == 0) return;

        timer += Time.deltaTime;
        if (timer >= 1f / frameRate)
        {
            currentFrame = (currentFrame + 1) % frames.Length;
            image.sprite = frames[currentFrame];
            timer = 0f;
        }
    }
}
