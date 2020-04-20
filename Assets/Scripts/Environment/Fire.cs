using System;
using UnityEngine;

public class Fire : MonoBehaviour
{
    // PROPERTIES
    // -------------------------------------------------------------------------

    [SerializeField] private int m_combustible = 200;
    [SerializeField] private int m_maxCombustible = 400;
    [SerializeField] private int m_combustibleConsumedPerSecond = 1;
    [SerializeField] private ParticleSystem m_particles;

    private float m_consumedAt;
    
    // PUBLIC METHODS
    // -------------------------------------------------------------------------
    
    public void IncreaseCombustible(int value)
    {
        m_combustible = Mathf.Clamp(m_combustible + value, 0, m_maxCombustible);
        UpdateParticles();
    }
    
    // PRIVATE METHODS
    // -------------------------------------------------------------------------
    
    private void Update()
    {
        if (Time.time - m_consumedAt >= 1f) {
            m_consumedAt = Time.time;
            IncreaseCombustible(-m_combustibleConsumedPerSecond);
        }
    }

    private void UpdateParticles()
    {
        if (!m_particles) {
            return;
        }

        int rateOverTime = Mathf.Clamp(m_combustible, 0, 100);
        
        ParticleSystem.EmissionModule emission = m_particles.emission;
        emission.rateOverTime = new ParticleSystem.MinMaxCurve(rateOverTime);

        ParticleSystem.MainModule main = m_particles.main;
        if (m_combustible < 25) {
            main.startLifetime = 1.5f;
        } else if (m_combustible < 5) {
            main.startLifetime = 2.2f;
        } else if (m_combustible < 100) {
            main.startLifetime = 3f;
        } else if (m_combustible < 200) {
            main.startLifetime = 4f;
        } else {
            main.startLifetime = 5f;
        }

        if (m_combustible <= 0) {
            GameController.instance.notifier.Notify(new Lose());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wood")) {
            IncreaseCombustible(30);
            Destroy(other.gameObject);
        }
    }
}
