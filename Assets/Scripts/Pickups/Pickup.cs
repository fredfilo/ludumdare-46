using UnityEngine;

public class Pickup : MonoBehaviour, Interactable
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
    
    // PUBLIC METHODS
    // -------------------------------------------------------------------------
    
    public void Interact(Player player)
    {
        if (!player.Pickup(m_type)) {
            return;
        }
        
        Destroy(transform.parent.gameObject);
    }
    
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
