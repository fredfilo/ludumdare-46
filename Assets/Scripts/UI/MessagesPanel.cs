using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MessagesPanel : MonoBehaviour, Notifiable
{
    // PROPERTIES
    // -------------------------------------------------------------------------

    [SerializeField] private TextMeshProUGUI m_titleText;
    [SerializeField] private TextMeshProUGUI m_messageText;

    private List<String> m_messages;
    
    private int m_currentMessageIndex = -1;
    
    // PUBLIC METHODS
    // -------------------------------------------------------------------------

    public void ReadMessages(String title, List<String> messages)
    {
        if (messages.Count < 1) {
            return;
        }
        
        GameController.instance.notifier.Notify(new UIInteractionStarted());
        
        m_titleText.text = title;
        m_messages = messages;
        
        m_currentMessageIndex = -1;
        ShowNextMessage();
    }

    public void OnInputInteract(InputAction.CallbackContext context)
    {
        Debug.Log(context.phase);
        
        if (context.phase == InputActionPhase.Canceled) {
            ShowNextMessage();
        }
    }
    
    public void OnNotification(Notification notification, Notifier notifier)
    {
        if (notification.type == Notification.Type.PLAYER_WANTS_TO_INTERACT) {
            ShowNextMessage();
        }
    }
    
    // PRIVATE METHODS
    // -------------------------------------------------------------------------

    private void Start()
    {
        GameController.instance.notifier.Subscribe(Notification.Type.PLAYER_WANTS_TO_INTERACT, this);
    }

    private void OnDestroy()
    {
        GameController.instance.notifier.Unsubscribe(Notification.Type.PLAYER_WANTS_TO_INTERACT, this);
    }

    private void ShowNextMessage()
    {
        if (m_messages == null) {
            return;
        }
        
        m_currentMessageIndex++;

        if (m_currentMessageIndex >= m_messages.Count) {
            m_messages = null;
            gameObject.SetActive(false);
            GameController.instance.notifier.Notify(new UIInteractionEnded());
            return;
        }
        
        m_messageText.text = m_messages[m_currentMessageIndex];
    }
}
