using UnityEngine;

public class GameController : MonoBehaviour
{
    // STATIC
    // -------------------------------------------------------------------------

    private static GameController s_instance;

    // PROPERTIES
    // -------------------------------------------------------------------------
    
    private readonly Notifier m_notifier = new Notifier();
    private readonly Messages m_messages = new Messages();
    
    // ACCESSORS
    // -------------------------------------------------------------------------

    public static GameController instance => s_instance;

    public Notifier notifier => m_notifier;

    public Messages messages => m_messages;
    
    // PRIVATE METHODS
    // -------------------------------------------------------------------------
    
    private void Awake()
    {
        if (s_instance == null) {
            s_instance = this;
        }

        if (this != s_instance) {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);
    }
}
