using System;
using System.Collections.Generic;

public class ReadMessages : Notification
{
    // PROPERTIES
    // -------------------------------------------------------------------------

    private String m_title;
    private List<String> m_messages;
    
    // ACCESSORS
    // -------------------------------------------------------------------------

    public String title => m_title;

    public List<String> messages => m_messages;
    
    // CONSTRUCTOR
    // -------------------------------------------------------------------------
    
    public ReadMessages(String title, List<String> messages)
    {
        m_type = Type.READ_MESSAGES;
        m_title = title;
        m_messages = messages;
    }
}
