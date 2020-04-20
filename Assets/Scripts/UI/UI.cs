using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour, Notifiable
{
    // PROPERTIES
    // -------------------------------------------------------------------------

    [SerializeField] private MessagesPanel m_messagesPanel;
    [SerializeField] private RescuePanel m_rescuePanel;
    [SerializeField] private GameObject m_losePanel;
    [SerializeField] private GameObject m_winPanel;
    
    // PUBLIC METHODS
    // -------------------------------------------------------------------------
    
    public void OnNotification(Notification notification, Notifier notifier)
    {
        if (notification is ReadMessages readMessagesNotification) {
            m_messagesPanel.ReadMessages(
                readMessagesNotification.messagesType,
                readMessagesNotification.title,
                readMessagesNotification.messages
            );
            m_messagesPanel.gameObject.SetActive(true);
            return;
        }

        if (notification is FinishedReadingMessages finishedMessages) {
            if (finishedMessages.messagesType == Messages.Type.NOTE_3) {
                m_rescuePanel.gameObject.SetActive(true);
                m_rescuePanel.StartRescue();
            }
            return;
        }

        if (notification.type == Notification.Type.WIN) {
            m_winPanel.SetActive(true);
            return;
        }
        
        if (notification.type == Notification.Type.LOSE) {
            m_rescuePanel.StopRescue();
            m_losePanel.SetActive(true);
            return;
        }
    }

    public void OnRetry()
    {
        SceneManager.LoadScene(0);
    }
    
    // PRIVATE METHODS
    // -------------------------------------------------------------------------
    
    private void Start()
    {
        GameController.instance.notifier.Subscribe(Notification.Type.FINISHED_READING_MESSAGES, this);
        GameController.instance.notifier.Subscribe(Notification.Type.READ_MESSAGES, this);
        GameController.instance.notifier.Subscribe(Notification.Type.LOSE, this);
        GameController.instance.notifier.Subscribe(Notification.Type.WIN, this);
    }

    private void OnDestroy()
    {
        GameController.instance.notifier.Unsubscribe(Notification.Type.FINISHED_READING_MESSAGES, this);
        GameController.instance.notifier.Unsubscribe(Notification.Type.READ_MESSAGES, this);
        GameController.instance.notifier.Unsubscribe(Notification.Type.LOSE, this);
        GameController.instance.notifier.Unsubscribe(Notification.Type.WIN, this);
    }
}
