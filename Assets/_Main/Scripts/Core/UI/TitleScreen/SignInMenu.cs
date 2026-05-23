using TMPro;

public class SignInMenu : TitleScreenSubMenu
{
    public TMP_InputField emailField;
    public TMP_InputField passwordField;
    
    public void SignIn()
    {
        FirebaseManager.instance.SignIn(emailField.text, passwordField.text);
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