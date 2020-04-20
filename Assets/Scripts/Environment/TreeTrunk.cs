using System;
using UnityEngine;

public class TreeTrunk : MonoBehaviour, Damageable
{
    // PROPERTIES
    // -------------------------------------------------------------------------

    [SerializeField] private int m_healthPoints = 4;

    [SerializeField] private GameObject m_pickupPrefab;

    [SerializeField] private SpriteRenderer m_spriteRenderer;
    [SerializeField] private Sprite m_sprite1Point;
    [SerializeField] private Sprite m_sprite2Points;
    [SerializeField] private Sprite m_sprite3Points;
    
    // PUBLIC METHODS
    // -------------------------------------------------------------------------
    
    public bool IsAlive()
    {
        return m_healthPoints > 0;
    }

    public int GetHealthPoints()
    {
        return m_healthPoints;
    }

    public void ApplyDamage(int damage)
    {
        if (!IsAlive()) {
            return;
        }

        m_healthPoints = Mathf.Clamp(m_healthPoints - damage, 0, m_healthPoints);
        
        UpdateSprite();
    }
    
    // PRIVATE METHODS
    // -------------------------------------------------------------------------

    private void UpdateSprite()
    {
        switch (m_healthPoints) {
            case 3:
                m_spriteRenderer.sprite = m_sprite3Points;
                break;
            case 2:
                m_spriteRenderer.sprite = m_sprite2Points;
                break;
            case 1:
                m_spriteRenderer.sprite = m_sprite1Point;
                break;
            case 0:
                SpawnPickup();
                Destroy(transform.parent.gameObject);
                break;
        }
    }

    private void SpawnPickup()
    {
        if (m_pickupPrefab == null) {
            return;
        }
        
        GameObject pickupObject = Instantiate(m_pickupPrefab, transform.position, Quaternion.identity);
        Rigidbody2D pickupRigidBody = pickupObject.GetComponent<Rigidbody2D>();
        if (pickupRigidBody) {
            pickupRigidBody.AddForce(new Vector2(100f, 100f));
        }
    }
}
