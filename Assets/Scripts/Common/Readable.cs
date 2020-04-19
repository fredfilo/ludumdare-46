using System;
using System.Collections.Generic;
using UnityEngine;

public class Readable : MonoBehaviour, Interactable
{
    // PROPERTIES
    // -------------------------------------------------------------------------
    
    [SerializeField] private Messages.Type m_messagesType;
    
    // PUBLIC METHODS
    // -------------------------------------------------------------------------

    public void Interact(Player player)
    {
        List<String> messages = GameController.instance.messages.GetMessages(m_messagesType);
        GameController.instance.notifier.Notify(new ReadMessages("Note:", messages));
    }
}
