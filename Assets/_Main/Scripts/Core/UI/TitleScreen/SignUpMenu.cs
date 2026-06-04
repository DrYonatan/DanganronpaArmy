using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class SignUpMenu : TitleScreenSubMenu
{
    public TMP_InputField emailField;
    public TMP_InputField passwordField;
    public TMP_InputField passwordConfirmField;
    public TMP_InputField usernameField;

    public void SignUp()
    {
        if (ValidateFields(emailField.text, usernameField.text, passwordField.text, passwordConfirmField.text))
        {
            FirebaseManager.instance.SignUp(emailField.text, passwordField.text,
                (userId) => { UserDataManager.instance.OnSignup(userId, usernameField.text); });
        }
        else
        {
            NotifyValidationFailed();
        }
    }

    bool ValidateFields(string email, string username, string password, string passwordConfirm)
    {
        return password.Length >= 6 && password == passwordConfirm && !username.Equals("") && ValidateEmail(email);
    }

    bool ValidateEmail(string email)
    {
        return Regex.IsMatch(email, "/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$/");
    }

    void NotifyValidationFailed()
    {
        
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