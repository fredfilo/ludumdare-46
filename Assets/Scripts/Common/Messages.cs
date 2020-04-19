using System;
using System.Collections.Generic;

public class Messages
{
    // STATIC
    // -------------------------------------------------------------------------

    public enum Type
    {
        NOTE_1
    }
    
    // PROPERTIES
    // -------------------------------------------------------------------------
    
    private Dictionary<Type, List<String>> m_messages = new Dictionary<Type, List<String>>();
    
    // CONSTRUCTOR
    // -------------------------------------------------------------------------

    public Messages()
    {
        m_messages[Type.NOTE_1] = new List<String>() {
            "To whoever read this,",
            "This is the Fire!",
            "Take care of it, that's your only chance to survive here...",
            "Keep it alive!",
        };
    }
    
    // PUBLIC METHODS
    // -------------------------------------------------------------------------

    public List<String> GetMessages(Type type)
    {
        if (!m_messages.ContainsKey(type)) {
            return new List<String>();
        }

        return m_messages[type];
    }
}
