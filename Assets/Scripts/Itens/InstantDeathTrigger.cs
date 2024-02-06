using UnityEngine;

public class InstantDeathTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerMovement>();

        if (player != null)
        {
            player.Die();
        }
    }
}