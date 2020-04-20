using UnityEngine;
using UnityEngine.UI;

public class RescuePanel : MonoBehaviour
{
    // PROPERTIES
    // -------------------------------------------------------------------------
    
    [SerializeField] private Image m_helicopterImage;

    [SerializeField] private float m_rescueCountDown = 240f;

    [SerializeField] private bool m_started = false;
    [SerializeField] private float m_rescueStartedAt;
    [SerializeField] private bool m_failed;
    
    // PUBLIC METHODS
    // -------------------------------------------------------------------------

    public void StartRescue()
    {
        m_rescueStartedAt = Time.time;
        m_started = true;
    }

    public void StopRescue()
    {
        m_failed = true;
    }
    
    // PRIVATE METHODS
    // -------------------------------------------------------------------------

    private void Update()
    {
        if (m_failed) {
            return;
        }

        if (!m_started) {
            m_rescueStartedAt = Time.time;
        } else if (m_rescueStartedAt < 0.0001f) {
            m_rescueStartedAt = Time.time;
        }
        
        float percent = (Time.time - m_rescueStartedAt) / m_rescueCountDown;
        percent = Mathf.Clamp(percent, 0f, 1f);
        
        RectTransform rt = m_helicopterImage.GetComponent<RectTransform>();
        Vector2 anchor = rt.anchoredPosition;
        anchor.x = 32f + (436f * percent);
        rt.anchoredPosition = anchor;

        if (percent > 0.999f) {
            GameController.instance.notifier.Notify(new Win());
        }
    }
}
