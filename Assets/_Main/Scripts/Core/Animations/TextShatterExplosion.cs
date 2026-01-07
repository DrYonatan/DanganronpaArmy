using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TextShatterExplosion : MonoBehaviour
{
    public float speed = 1f;
    
    public Image orangeLines;
    public Image darkBlue;
    public Image lightBlue;
    public Image glow;
    public Image blueRing;
    public Image shards;

    void Awake()
    {
        orangeLines.DOFade(0f, 0f);
        darkBlue.DOFade(0f, 0f);
        lightBlue.DOFade(0f, 0f);
        glow.DOFade(0f, 0f);
        blueRing.DOFade(0f, 0f);
        shards.DOFade(0f, 0f);

        Explode();
    }

    private void Explode()
    {
        Sequence master = DOTween.Sequence();

        // Fade sequence
        master.Append(glow.DOFade(1f, 0.05f));
        master.Join(orangeLines.DOFade(1f, 0.05f));
        master.Join(lightBlue.DOFade(1f, 0.2f / speed));
        master.Join(darkBlue.DOFade(1f, 0.2f / speed));
        master.Join(blueRing.DOFade(0.8f, 0.2f / speed));
        
        master.Join(orangeLines.rectTransform.DOScale(5f, 0.7f / speed));
        master.Join(glow.rectTransform.DOScale(3f, 0.7f / speed));
        master.Join(lightBlue.rectTransform.DOScale(3f, 0.7f / speed));
        master.Join(darkBlue.rectTransform.DOScale(3f, 0.7f / speed));
        master.Join(blueRing.rectTransform.DOScale(4f, 1.2f / speed));
        master.Join(shards.rectTransform.DOScale(2.5f, 1f / speed));
        
        master.Join(orangeLines.rectTransform.DOLocalRotate(new Vector3(0, 0, 180f), 1f / speed));
        
        master.Append(shards.DOFade(1f, 0.1f));
        
        master.Join(glow.DOFade(0f, 0.3f / speed));
        master.Join(lightBlue.DOFade(0f, 0.3f / speed));
        master.Join(darkBlue.DOFade(0f, 0.3f / speed));
        master.Join(orangeLines.DOFade(0f, 0.3f / speed));
        master.Join(blueRing.DOFade(0f, 0.3f / speed));
        master.Append(shards.DOFade(0f, 0.3f / speed));

        master.OnComplete(() => Destroy(gameObject));
    }

}