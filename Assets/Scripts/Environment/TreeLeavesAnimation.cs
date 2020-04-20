using UnityEngine;

public class TreeLeavesAnimation : MonoBehaviour
{
    public void OnAnimationFinished()
    {
        Destroy(gameObject);
    }
}
