
using UnityEngine;

public interface IFaceable
{
    public void FaceAppear();

    public bool IsVisible();

    public void ChangeFace(Sprite sprite);
}
