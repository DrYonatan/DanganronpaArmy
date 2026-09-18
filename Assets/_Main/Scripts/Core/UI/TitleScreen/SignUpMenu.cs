using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class SignUpMenu : TitleScreenSubMenu
{
    public TMP_InputField emailField;
    public TMP_InputField passwordField;
    public TMP_InputField passwordConfirmField;
    public TMP_InputField usernameField;
    public TitleScreenMainMenu mainMenu;

    public void SignUp()
    {
        if (ValidateFields(emailField.text, usernameField.text, passwordField.text, passwordConfirmField.text))
        {
            FirebaseManager.instance.SignUp(emailField.text, passwordField.text,
                (userId) =>
                {
                    UserDataManager.instance.OnSignup(userId, usernameField.text); 
                    mainMenu.ReturnToPrevMenu();
                });
        }
    }

    bool ValidatePassword(string password, string passwordConfirm)
    {
        return password.Length >= 6 && password.Equals(passwordConfirm);
    }

    bool ValidateFields(string email, string username, string password, string passwordConfirm)
    {
        if (!ValidatePassword(password, passwordConfirm))
        {
            NotifyPasswordValidationFailed();
            return false;
        }

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
        Debug.Log("PASSWORD IS NOT VALID");
    }

    void NotifyUsernameValidationFailed()
    {
        Debug.Log("USERNAME IS NOT VALID");

    }

    void NotifyEmailValidationFailed()
    {
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
}