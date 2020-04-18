using System;
using System.Collections.Generic;
using UnityEngine;

public class Readable : MonoBehaviour, Interactable
{
    // PROPERTIES
    // -------------------------------------------------------------------------
    
    [SerializeField] private List<String> m_messages = new List<string>();
    
    // PUBLIC METHODS
    // -------------------------------------------------------------------------

    public void Interact(Player player)
    {
        GameController.instance.notifier.Notify(new ReadMessages("Note:", m_messages));
    }
}
