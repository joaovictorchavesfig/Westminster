using UnityEngine;

public class TriggerUI : MonoBehaviour
{
    [SerializeField] private GameObject painel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerMovement>();

        if(player != null && painel != null)
        {
            painel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerMovement>();

        if(player != null && painel != null)
        {
            painel.SetActive(false);
        }
    }
}