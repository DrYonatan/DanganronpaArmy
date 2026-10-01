using DG.Tweening;
using TMPro;
using UnityEngine;

public class SignInMenu : TitleScreenSubMenu
{
    public TMP_InputField emailField;
    public TMP_InputField passwordField;
    public TitleScreenMainMenu mainMenu;
    public CanvasGroup errorCanvasGroup;
    public void SignIn()
    {
        FirebaseManager.instance.SignIn(emailField.text, passwordField.text, () =>
        {
            mainMenu.ReturnToPrevMenu();
        }, (error) =>
        {
            if (error == ErrorTypes.INCORRECT_DATA)
            {
                ShowIncorrectDataMessage();
            }
        });
    }

    void ShowIncorrectDataMessage()
    {
        errorCanvasGroup.DOKill();

        Sequence seq = DOTween.Sequence();
        seq.Append(errorCanvasGroup.DOFade(0.8f, 0.5f));
        seq.AppendInterval(1.5f);
        seq.Append(errorCanvasGroup.DOFade(0f, 0.5f));
    }

    public override void AppearAnimation()
    {
        base.AppearAnimation();
        menuNavigationActive = true;
        emailField.text = "";
        passwordField.text = "";
    }

    public void SignOut()
    {
        FirebaseManager.instance.SignOut();
    }

    public void FocusInput()
    {
        menuNavigationActive = false;
    }

    public void UnFocusInput()
    {
        menuNavigationActive = true;
    }
}