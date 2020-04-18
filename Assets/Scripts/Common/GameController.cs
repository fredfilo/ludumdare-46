using UnityEngine;

public class GameController : MonoBehaviour
{
    // STATIC
    // -------------------------------------------------------------------------

    private static GameController s_instance;

    // ACCESSORS
    // -------------------------------------------------------------------------

    public static GameController instance => s_instance;

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
