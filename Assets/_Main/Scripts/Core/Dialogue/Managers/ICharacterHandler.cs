namespace DIALOGUE
{
    public interface ICharacterHandler
    {
        public void OnLineParsed(DIALOGUE_LINE line);
        public void OnStopConversation();
    }
}