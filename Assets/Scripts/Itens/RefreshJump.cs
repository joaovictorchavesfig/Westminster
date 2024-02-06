using System.Collections;
using UnityEngine;

public class RefreshJump : MonoBehaviour
{
    [SerializeField] float cooldown;


    private void OnTriggerStay2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerMovement>();

        if (player != null)
        {
            if (!player.isGrounded() && !player.refreshJump)
            {
                player.refreshJump = true;
                player.ItemPickUp(gameObject, cooldown);
            }
        }
    }

}