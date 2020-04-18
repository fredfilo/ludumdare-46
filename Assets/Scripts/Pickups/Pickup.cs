using UnityEngine;

public class Pickup : MonoBehaviour
{
    // STATIC
    // -------------------------------------------------------------------------

    public enum Type
    {
        AXE
    }
    
    // PROPERTIES
    // -------------------------------------------------------------------------

    [SerializeField] private Type m_type;
    [SerializeField] private GameObject m_proximityText;

    // ACCESSORS
    // -------------------------------------------------------------------------

    public Type type => m_type;
    
    // PRIVATE METHODS
    // -------------------------------------------------------------------------

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || m_proximityText == null) {
            return;
        }

        m_proximityText.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || m_proximityText == null) {
            return;
        }

        m_proximityText.SetActive(false);
    }
}
