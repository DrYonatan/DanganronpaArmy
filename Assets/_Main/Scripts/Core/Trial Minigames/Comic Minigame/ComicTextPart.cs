using System;
using System.Collections;

[Serializable]
public class ComicTextPart : ComicStep
{
    public IEnumerator Play()
    {
        yield return null;
    }
}
