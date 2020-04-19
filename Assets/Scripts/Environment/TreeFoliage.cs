using UnityEngine;

public class TreeFoliage : MonoBehaviour
{
    // PRIVATE METHODS
    // -------------------------------------------------------------------------
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("CannotWalk")) {
            Destroy(transform.parent.gameObject);
        }
    }
}
