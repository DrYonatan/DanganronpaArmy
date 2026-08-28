using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CompanySplash : MonoBehaviour
{
    public Image companyLogo;
    void Start()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(companyLogo.DOFade(1f, 0.6f).SetDelay(1f));
        seq.Append(companyLogo.DOFade(0f, 0.6f).SetDelay(2.5f));
        seq.AppendInterval(1f);
        seq.OnComplete(() =>
        {
            LoadTitleScreen();
        });
    }

    void LoadTitleScreen()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}