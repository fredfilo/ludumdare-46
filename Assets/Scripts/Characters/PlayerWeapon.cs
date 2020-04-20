using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    // PROPERTIES
    // -------------------------------------------------------------------------

    [SerializeField] private int m_damage = 1;
    private readonly List<Damageable> m_damageables = new List<Damageable>();

    // PUBLIC METHODS
    // -------------------------------------------------------------------------

    public void Attack()
    {
        foreach (Damageable damageable in m_damageables) {
            damageable.ApplyDamage(m_damage);
        }
    }
    
    // PRIVATE METHODS
    // -------------------------------------------------------------------------

    private void OnTriggerEnter2D(Collider2D other)
    {
        Damageable damageable = other.GetComponent<Damageable>();
        if (damageable != null && !m_damageables.Contains(damageable)) {
            m_damageables.Add(damageable);
            // Debug.Log("Added damageable");
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        Damageable damageable = other.GetComponent<Damageable>();
        if (damageable != null && m_damageables.Contains(damageable)) {
            m_damageables.Remove(damageable);
            // Debug.Log("Removed damageable");
        }
    }
}
