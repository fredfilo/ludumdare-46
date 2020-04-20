using UnityEngine;

public class Pickup : MonoBehaviour, Interactable
{
    // PROPERTIES
    // -------------------------------------------------------------------------

    [SerializeField] private Types.PickupType m_pickupType;
    [SerializeField] private GameObject m_proximityText;

    // ACCESSORS
    // -------------------------------------------------------------------------

    public Types.PickupType pickupType => m_pickupType;
    
    // PUBLIC METHODS
    // -------------------------------------------------------------------------
    
    public void Interact(Player player)
    {
        player.Pickup(m_pickupType, transform.parent.gameObject);
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
