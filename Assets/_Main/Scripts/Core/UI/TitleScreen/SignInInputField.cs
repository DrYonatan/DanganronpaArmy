using TMPro;

public class SignInInputField : TitleScreenMenuButton
{
    public TMP_InputField input;

    public override void Click()
    {
        input.ActivateInputField();
    }
}