using System;

[Serializable]
public class UIState
{
    public bool namePlateVisible = true;
    public ImageState overlayImage;
    public ImageState backgroundImage;
    public AnimatedImageState animatedImage;
}

[Serializable]
public class ImageState
{
    public string spriteId;
    public bool visible;
}

[Serializable]
public class AnimatedImageState
{
    public string prefabId;
    public int currentStateIndex;
    public bool visible;
}