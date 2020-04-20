using UnityEngine;

public class TreeFoliage : MonoBehaviour
{
    // PROPERTIES
    // -------------------------------------------------------------------------

    [SerializeField] private GameObject m_hitGroundAnimationPrefab;
    
    // PRIVATE METHODS
    // -------------------------------------------------------------------------
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("CannotWalk")) {
            for (int i = 0; i < 5; i++) {
                Vector3 position = new Vector3(
                    transform.position.x + (-1.5f + (Random.value * 3f)),
                    transform.position.y + (0f + (Random.value * 2f)),
                    0
                    );
                Instantiate(m_hitGroundAnimationPrefab, position, Quaternion.identity);
            }

            GameController.instance.PlayTreeDead();
            Destroy(transform.parent.gameObject);
        }
    }
}
