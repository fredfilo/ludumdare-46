using System.Collections.Generic;

public class Notifier
{
    // PROPERTIES
    // -------------------------------------------------------------------------
    
    private readonly Dictionary<Notification.Type, List<Notifiable>> m_types = new Dictionary<Notification.Type, List<Notifiable>>();
    
    // PUBLIC METHODS
    // -------------------------------------------------------------------------

    public void Subscribe(Notification.Type type, Notifiable notifiable)
    {
        if (!m_types.ContainsKey(type)) {
            m_types[type] = new List<Notifiable>();
        }

        List<Notifiable> notifiables = m_types[type];
        if (notifiables.Contains(notifiable)) {
            return;
        }
        
        notifiables.Add(notifiable);
    }
    
    public void Unsubscribe(Notification.Type type, Notifiable notifiable)
    {
        if (!m_types.ContainsKey(type)) {
            return;
        }

        List<Notifiable> notifiables = m_types[type];
        notifiables.Remove(notifiable);
    }
    
    public void Notify(Notification notification)
    {
        Notification.Type type = notification.type;
        
        if (!m_types.ContainsKey(type)) {
            return;
        }

        List<Notifiable> notifiables = m_types[type];
        foreach (Notifiable notifiable in notifiables) {
            notifiable.OnNotification(notification, this);
        }
    }
}
