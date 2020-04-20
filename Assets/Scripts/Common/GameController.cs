using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour, Notifiable
{
    // STATIC
    // -------------------------------------------------------------------------

    private static GameController s_instance;

    // PROPERTIES
    // -------------------------------------------------------------------------

    [SerializeField] private Fire m_fire;
    [SerializeField] private GameObject m_monkeyPrefab;
    [SerializeField] private List<Transform> m_monkeysSpawnPositions;
    
    [Header("Audio")]
    
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private AudioSource m_audioSourceMusic;
    [SerializeField] private AudioClip m_soundAttackTree;
    [SerializeField] private AudioClip m_soundJump;
    [SerializeField] private AudioClip m_soundShowOff;
    [SerializeField] private AudioClip m_soundThrow;
    [SerializeField] private AudioClip m_soundTreeDead;
    [SerializeField] private AudioClip m_soundMusic;
    
    private readonly Notifier m_notifier = new Notifier();
    private readonly Messages m_messages = new Messages();
    
    // ACCESSORS
    // -------------------------------------------------------------------------

    public static GameController instance => s_instance;

    public Notifier notifier => m_notifier;

    public Messages messages => m_messages;

    public Fire fire => m_fire;
    
    // PUBLIC METHODS
    // -------------------------------------------------------------------------
    
    public void OnNotification(Notification notification, Notifier notifier)
    {
        if (notification is FinishedReadingMessages messagesNotification) {
            if (messagesNotification.messagesType == Messages.Type.NOTE_3) {
                SpawnMonkeys();
            }
        }
    }
    
    public void PlayAttackTree()
    {
        m_audioSource.PlayOneShot(m_soundAttackTree);
    }
    
    public void PlayJump()
    {
        m_audioSource.PlayOneShot(m_soundJump);
    }
    
    public void PlayShowOff()
    {
        m_audioSource.PlayOneShot(m_soundShowOff);
    }
    
    public void PlayThrow()
    {
        m_audioSource.PlayOneShot(m_soundThrow);
    }
    
    public void PlayTreeDead()
    {
        m_audioSource.PlayOneShot(m_soundTreeDead);
    }
    
    
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
        
        notifier.Subscribe(Notification.Type.FINISHED_READING_MESSAGES, this);
        
        DontDestroyOnLoad(gameObject);
    }

    private void SpawnMonkeys()
    {
        foreach (Transform position in m_monkeysSpawnPositions) {
            GameObject monkeyObject = Instantiate(m_monkeyPrefab, position.position, Quaternion.identity);
            Monkey monkey = monkeyObject.GetComponent<Monkey>();
            float distance = position.position.x - m_fire.transform.position.x;
            
            if (Random.value < 0.5f) {
                monkey.currentPathTo = Types.PathTo.FIRE;
                monkey.isFull = true;
                if (distance > 0) {
                    monkey.ChangeOrientation();
                }
            }
            else {
                monkey.currentPathTo = Types.PathTo.WATER;
                monkey.isFull = false;
                if (distance < 0) {
                    monkey.ChangeOrientation();
                }
            }
        }
    }
}
