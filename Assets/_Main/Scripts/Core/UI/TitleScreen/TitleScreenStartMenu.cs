public class TitleScreenStartMenu : TitleScreenSubMenu, IAuthenticationListener
{
    public TitleScreenMenuButton signInButton;
    public TitleScreenActionButton signOutButton;
    
    void Start()
    {
        FirebaseManager.instance.AddAuthenticationListener(this);
        OnAuthentication();
    }
    
    public void OnAuthentication()
    {
        if (FirebaseManager.instance.user != null)
        {
            OnSignIn();
        }
        else
        {
            OnSignOut();
        }
    }
    
    void OnSignIn()
    {
        signInButton.gameObject.SetActive(false);
        signOutButton.gameObject.SetActive(true);
    }

    void OnSignOut()
    {
        signInButton.gameObject.SetActive(true);
        signOutButton.gameObject.SetActive(false);
    }
}