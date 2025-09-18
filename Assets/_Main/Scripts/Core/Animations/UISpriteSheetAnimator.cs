using UnityEngine;
using UnityEngine.UI;

public class UISpriteSheetAnimator : MonoBehaviour
{
    public string spritesheetPath; // Name of the sprite sheet file (without extension)
    public float frameRate = 10f;
    public bool isLoop;

    private Sprite[] _frames;
    private Image _image;
    private int _currentFrame;
    private float _timer;

    void Start()
    {
        _image = GetComponent<Image>();

        // Load all sliced sprites from the sprite sheet in Resources folder
        _frames = Resources.LoadAll<Sprite>("Images/" + spritesheetPath);

        if (_frames == null || _frames.Length == 0)
        {
            Debug.LogError("No sprites found. Make sure your sprite sheet is sliced and located in a Resources folder.");
        }

    }


    void Update()
    {
        if (_frames == null || _frames.Length == 0 || _image == null) return;

        _timer += Time.deltaTime;
        float frameDuration = 1f / frameRate;

        int framesToAdvance = (int)(_timer / frameDuration);
        if (framesToAdvance > 0)
        {
            _timer -= framesToAdvance * frameDuration;

            _currentFrame += framesToAdvance;

            if (isLoop)
            {
                _currentFrame %= _frames.Length;
            }
            else
            {
                _currentFrame = Mathf.Min(_currentFrame, _frames.Length - 1);
            }

            _image.sprite = _frames[_currentFrame];
        }
    }

}
