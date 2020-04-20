public class FinishedReadingMessages : Notification
{
    // PROPERTIES
    // -------------------------------------------------------------------------

    private Messages.Type m_messagesType;
    
    // ACCESSORS
    // -------------------------------------------------------------------------

    public Messages.Type messagesType => m_messagesType;
    
    // CONSTRUCTOR
    // -------------------------------------------------------------------------
    
    public FinishedReadingMessages(Messages.Type messagesType)
    {
        m_type = Type.FINISHED_READING_MESSAGES;
        m_messagesType = messagesType;
    }
}
