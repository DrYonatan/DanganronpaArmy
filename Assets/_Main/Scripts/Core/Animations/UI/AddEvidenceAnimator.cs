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
    public GameObject container;
    public Image bulletDuplicate;
    public RectTransform bulletDuplicateTransform;
    public AudioClip evidenceAddedSound;

    void Start()
    {
        bulletOriginalPosX = bullet.anchoredPosition.x;
        imageOriginalPosX = imageTransform.anchoredPosition.x;
    }

    public IEnumerator PlayAnimation(Evidence evidence)
    {
        container.SetActive(true);
        bool bulletAnimationCompleted = false;
        image.sprite = evidence.icon;
        imageContainer.DOFade(0, 0);
        imageTransform.DOAnchorPosX(imageOriginalPosX - 200f, 0f).OnComplete(() =>
        {
            imageContainer.DOFade(1, 0.3f);
            imageTransform.DOAnchorPosX(imageOriginalPosX, 0.3f);
        });
        MusicManager.instance.PauseSong();
        SoundManager.instance.PlaySoundEffect(evidenceAddedSound);
        bullet.DOAnchorPosX(bulletOriginalPosX - 1200f, 0f).OnComplete(() =>
        {
            bullet.DOAnchorPosX(bulletOriginalPosX, 0.3f).OnComplete(() =>
            {
                BulletDuplicateAnimation();
                bullet.DOAnchorPosX(bulletOriginalPosX + 1200f, 0.2f).SetDelay(0.8f).OnComplete(() =>
                {
                    bulletAnimationCompleted = true;
                });
            });
            ;
        });
        yield return new WaitUntil(() => bulletAnimationCompleted);
        yield return SayText(evidence.name);
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
        imageTransform.DOAnchorPosX(imageOriginalPosX + 200f, 0.3f).OnComplete(ResetValues);
    }
}   