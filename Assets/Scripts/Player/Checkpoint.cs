using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerMovement>();

        if(player != null)
        {
            player.SetRespawnPoint(transform.position);
        }
    }
}