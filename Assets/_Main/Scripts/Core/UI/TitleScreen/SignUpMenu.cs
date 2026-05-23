using TMPro;

public class SignUpMenu : TitleScreenSubMenu
{
    public TMP_InputField emailField;
    public TMP_InputField passwordField;
    public TMP_InputField passwordConfirmField;

    public void SignUp()
    {
        if (passwordConfirmField.text.Equals(passwordField.text))
        {
            FirebaseManager.instance.SignUp(emailField.text, passwordField.text);
        }
        else
        {
            
        }
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