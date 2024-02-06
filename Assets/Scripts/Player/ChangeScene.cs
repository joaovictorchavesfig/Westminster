using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    private PlayerMovement player;
    private bool hasTraveled = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        if (player != null)
        {
            hasTraveled = player.hasTraveledTime();
            if (!this.CompareTag("FirstPortal") && !hasTraveled)
            {
                this.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (player != null && player.canTravelTime)
        {
            if (!hasTraveled && this.CompareTag("FirstPortal"))
            {
                player.firstPortal();
            }
            FindObjectOfType<LevelLoader>().TravelTime();
        }
    }
}