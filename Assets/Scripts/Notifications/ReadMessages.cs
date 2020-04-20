using System;
using System.Collections.Generic;

public class ReadMessages : Notification
{
    // PROPERTIES
    // -------------------------------------------------------------------------

    private String m_title;
    private List<String> m_messages;
    private Messages.Type m_messagesType;
    
    // ACCESSORS
    // -------------------------------------------------------------------------

    public String title => m_title;

    public List<String> messages => m_messages;

    public Messages.Type messagesType => m_messagesType;
    
    // CONSTRUCTOR
    // -------------------------------------------------------------------------
    
    public ReadMessages(Messages.Type messagesType, String title, List<String> messages)
    {
        m_type = Type.READ_MESSAGES;
        m_messagesType = messagesType;
        m_title = title;
        m_messages = messages;
    }
}
