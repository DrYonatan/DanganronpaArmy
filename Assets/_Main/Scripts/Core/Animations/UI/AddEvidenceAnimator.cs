using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DIALOGUE;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AddEvidenceAnimator : MonoBehaviour
{
    public RectTransform bullet;
    public RectTransform imageTransform;
    public CanvasGroup imageContainer;
    public Image image;
    private float bulletOriginalPosX;
    private float imageOriginalPosX;
    private float glowOriginalPosY;
    public GameObject container;
    public Image bulletDuplicate;
    public RectTransform bulletDuplicateTransform;
    public AudioClip evidenceAddedSound;
    public RectTransform blueGlowTransform;
    public Image blueGlow;
    public RectTransform blueGlowDuplicateTransform;
    public Image blueGlowDuplicate;

    void Start()
    {
        bulletOriginalPosX = bullet.anchoredPosition.x;
        imageOriginalPosX = imageTransform.anchoredPosition.x;
        glowOriginalPosY = imageTransform.anchoredPosition.y;
    }

    public IEnumerator PlayAnimation(Evidence evidence)
    {
        container.SetActive(true);
        image.sprite = evidence.icon;

        MusicManager.instance.PauseSong();
        SoundManager.instance.PlaySoundEffect(evidenceAddedSound);
        Sequence imageSeq = DOTween.Sequence();
        imageSeq.Append(imageContainer.DOFade(0, 0));
        imageSeq.Join(imageTransform.DOAnchorPosX(imageOriginalPosX + 200f, 0));
        imageSeq.Append(imageContainer.DOFade(1, 0.3f));
        imageSeq.Join(imageTransform.DOAnchorPosX(imageOriginalPosX, 0.3f));

        Sequence bulletSeq = DOTween.Sequence();
        bulletSeq.Append(bullet.DOAnchorPosX(bulletOriginalPosX + 1400f, 0));
        bulletSeq.Append(bullet.DOAnchorPosX(bulletOriginalPosX, 0.3f));
        bulletSeq.Join(blueGlow.DOFade(0.3f, 0.3f));
        bulletSeq.AppendCallback(() => BulletDuplicateAnimation());
        bulletSeq.Append(blueGlowDuplicate.DOFade(0.6f, 0.5f));
        bulletSeq.Join(blueGlow.DOFade(0.6f, 0.5f));
        bulletSeq.Append(bullet.DOAnchorPosX(bulletOriginalPosX - 1400f, 0.2f));
        bulletSeq.Join(blueGlowTransform.DOAnchorPosY(glowOriginalPosY - 100f, 0.2f));
        bulletSeq.Join(blueGlowDuplicateTransform.DOAnchorPosY(glowOriginalPosY + 100f, 0.2f));
        bulletSeq.Join(blueGlow.DOFade(0, 0.2f));
        bulletSeq.Join(blueGlowDuplicate.DOFade(0, 0.2f));
        Sequence masterSeq = DOTween.Sequence();
        masterSeq.Join(imageSeq);
        masterSeq.Join(bulletSeq);

        yield return masterSeq.WaitForCompletion();

        yield return SayText(evidence.Name);
    }

    void BulletDuplicateAnimation()
    {
        bulletDuplicate.gameObject.SetActive(true);
        bulletDuplicate.DOFade(0, 0.4f);
        bulletDuplicateTransform.DOScale(1.8f, 0.4f);
    }

    void ResetValues()
    {
        bulletDuplicateTransform.DOScale(1f, 0);
        bulletDuplicate.DOFade(1, 0);
        blueGlowDuplicate.DOFade(0, 0);
        blueGlow.DOFade(0, 0);
        blueGlowTransform.DOAnchorPosY(glowOriginalPosY, 0);
        blueGlowDuplicateTransform.DOAnchorPosY(glowOriginalPosY, 0);
        bulletDuplicate.gameObject.SetActive(false);
        container.SetActive(false);
        MusicManager.instance.ResumeSong();
    }

    IEnumerator SayText(string evidenceName)
    {
        List<DialogueNode> nodes = UtilityNodesRuntimeBank.instance.nodesCollection.evidenceAdded;
        (nodes[0].textData as VNTextData).text = (nodes[0].textData as VNTextData).text.Replace("<>", evidenceName);
        yield return VNNodePlayer.instance.RunNode(nodes[0]);
        DialogueSystem.instance.TurnOnSingleTimeAuto();
        imageContainer.DOFade(0, 0.3f);
        imageTransform.DOAnchorPosX(imageOriginalPosX - 200f, 0.3f).OnComplete(ResetValues);
    }
}   