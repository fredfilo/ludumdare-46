using UnityEngine;

public class PlayerCannotWalk : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        player.OnCannotWalk();
    }
}
