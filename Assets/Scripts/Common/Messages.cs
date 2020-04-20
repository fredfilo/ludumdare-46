using System;
using System.Collections.Generic;

public class Messages
{
    // STATIC
    // -------------------------------------------------------------------------

    public enum Type
    {
        NOTE_1,
        NOTE_2,
        NOTE_3,
        NOTE_4,
        NOTE_5
    }
    
    // PROPERTIES
    // -------------------------------------------------------------------------
    
    private Dictionary<Type, List<String>> m_messages = new Dictionary<Type, List<String>>();
    
    // CONSTRUCTOR
    // -------------------------------------------------------------------------

    public Messages()
    {
        m_messages[Type.NOTE_1] = new List<String>() {
            "I'm the only survivor.",
            "For now, I'll have to remember my training:",
            "Press R (Keyboard) or Right Button (Gamepad) to interact/hold objects.",
            "Press E (Keyboard) or Left Button (Gamepad) to attack.",
            "Press SPACE (Keyboard) or Bottom Button (Gamepad) to jump.",
            "Press WASD or Arrows (Keyboard) or Left Stick (Gamepad) to move.",
        };
        
        m_messages[Type.NOTE_2] = new List<String>() {
            "They are everywhere!",
            "I know... they protect their island but...",
        };
        
        m_messages[Type.NOTE_3] = new List<String>() {
            "If you're stuck here like me...",
            "Keep the fire burning at all cost!",
            "That's your only chance to get noticed and escape.",
            "...",
            "Cut the trees...",
            "Keep the fire burning...",
            "KEEP IT ALIVE!",
            "...",
            "They're coming!",
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
