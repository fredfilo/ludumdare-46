using UnityEngine;

public class UI : MonoBehaviour, Notifiable
{
    // PROPERTIES
    // -------------------------------------------------------------------------

    [SerializeField] private MessagesPanel m_messagesPanel;
    
    // PUBLIC METHODS
    // -------------------------------------------------------------------------
    
    public void OnNotification(Notification notification, Notifier notifier)
    {
        if (notification is ReadMessages readMessagesNotification) {
            m_messagesPanel.ReadMessages(readMessagesNotification.title, readMessagesNotification.messages);
            m_messagesPanel.gameObject.SetActive(true);
        }
    }
    
    // PRIVATE METHODS
    // -------------------------------------------------------------------------
    
    private void Start()
    {
        GameController.instance.notifier.Subscribe(Notification.Type.READ_MESSAGES, this);
    }

    private void OnDestroy()
    {
        GameController.instance.notifier.Unsubscribe(Notification.Type.READ_MESSAGES, this);
    }
}
