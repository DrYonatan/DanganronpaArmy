using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogicShootUIAnimator : MonoBehaviour
{
    public TextMeshProUGUI mikbazTextPrefab;
    public RectTransform holePrefab;
    public Image enemyHpBar;

    public void ShowMikbazText(int number)
    {
        TextMeshProUGUI mikbazText = Instantiate(mikbazTextPrefab, transform);
        mikbazText.text = "מקבץ " + number + " !!";

        Sequence seq = DOTween.Sequence();
        seq.Append(mikbazText.DOFade(0f, 0.05f).SetLoops(6, LoopType.Yoyo));
        seq.Append(mikbazText.rectTransform.DOAnchorPos(enemyHpBar.rectTransform.anchoredPosition, 1f)
            .SetEase(Ease.InCubic));
        seq.Join(mikbazText.rectTransform.DOScale(0.1f, 1f).SetEase(Ease.InCubic));
        
        seq.OnComplete(() => Destroy(mikbazText.gameObject));
    }
}