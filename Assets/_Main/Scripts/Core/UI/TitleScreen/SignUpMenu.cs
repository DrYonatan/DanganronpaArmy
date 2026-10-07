using System.Text.RegularExpressions;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignUpMenu : TitleScreenSubMenu
{
    public TMP_InputField emailField;
    public TMP_InputField passwordField;
    public TMP_InputField passwordConfirmField;
    public TMP_InputField usernameField;
    public TitleScreenMainMenu mainMenu;
    public CanvasGroup passwordError;
    public CanvasGroup emailError;
    public CanvasGroup passwordConfirmError;
    public CanvasGroup usernameError;

    public void SignUp()
    {
        if (ValidateFields(emailField.text, usernameField.text, passwordField.text, passwordConfirmField.text))
        {
            FirebaseManager.instance.SignUp(emailField.text, passwordField.text,
                (userId) =>
                {
                    UserDataManager.instance.OnSignup(userId, usernameField.text); 
                    mainMenu.ReturnToPrevMenu();
                    mainMenu.ReturnToPrevMenu();
                });
        }
    }
    
    public override void AppearAnimation()
    {
        base.AppearAnimation();
        menuNavigationActive = true;
        emailField.text = "";
        passwordField.text = "";
        passwordConfirmField.text = "";
        usernameField.text = "";
        FadeOutError(emailError);
        FadeOutError(passwordError);
        FadeOutError(passwordConfirmError);
        FadeOutError(usernameError);
    }


    bool ValidatePassword(string password)
    {
        return password.Length >= 6;
    }

    bool ValidatePasswordConfirm(string password, string passwordConfirm)
    {
        return password.Equals(passwordConfirm);
    }

    bool ValidateFields(string email, string username, string password, string passwordConfirm)
    {

        if (!ValidateEmail(email))
        {
            NotifyEmailValidationFailed();
            return false;
        }
        
        if (!ValidateUsername(username))
        {
            NotifyUsernameValidationFailed();
            return false;
        }
        
        if (!ValidatePassword(password))
        {
            NotifyPasswordValidationFailed();
            return false;
        }
        
        if (!ValidatePasswordConfirm(password, passwordConfirm))
        {
            NotifyPasswordConfirmValidationFailed();
            return false;
        }

        return true;
    }

    bool ValidateEmail(string email)
    {
        return Regex.IsMatch(email, @"^[a-z0-9](\.?[a-z0-9]){5,}@g(oogle)?mail\.com$");
    }

    bool ValidateUsername(string username)
    {
        return !username.Equals("");
    }

    void NotifyPasswordValidationFailed()
    {
        passwordError.DOKill();
        passwordError.DOFade(1f, 0.1f);
        Debug.Log("PASSWORD IS NOT VALID");
    }
    
    void NotifyPasswordConfirmValidationFailed()
    {
        passwordConfirmError.DOKill();
        passwordConfirmError.DOFade(1f, 0.1f);
        Debug.Log("PASSWORD IS NOT VALID");
    }

    void NotifyUsernameValidationFailed()
    {
        usernameError.DOKill();
        usernameError.DOFade(1f, 0.1f);
        Debug.Log("USERNAME IS NOT VALID");

    }

    void NotifyEmailValidationFailed()
    {
        emailError.DOKill();
        emailError.DOFade(1f, 0.1f);
        Debug.Log("EMAIL IS NOT VALID");

    }

    public void FocusInput()
    {
        menuNavigationActive = false;
    }

    public void UnFocusInput()
    {
        menuNavigationActive = true;
    }

    void Awake()
    {
        emailField.onValueChanged.AddListener(_ => FadeOutError(emailError));
        passwordField.onValueChanged.AddListener(_ => FadeOutError(passwordError));
        passwordConfirmField.onValueChanged.AddListener(_ => FadeOutError(passwordConfirmError));
        usernameField.onValueChanged.AddListener(_ => FadeOutError(usernameError));
    }

    void FadeOutError(CanvasGroup errorImage)
    {
        errorImage.DOKill();
        errorImage.DOFade(0f, 0.1f);
    }
}